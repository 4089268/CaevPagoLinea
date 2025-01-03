using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Services;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Helpers;

namespace CAEV.PagoLinea.Controllers;

[Authorize]
[Route("/admin/[Controller]")]
public class PaymentsController : Controller
{
    private readonly ILogger<PaymentsController> _logger;
    private readonly IPaymentResume paymentResumeService;
    private readonly PagoLineaContext pagoLineaContext;

    public PaymentsController(ILogger<PaymentsController> logger, IPaymentResume pr, PagoLineaContext context)
    {
        this._logger = logger;
        this.paymentResumeService = pr;
        this.pagoLineaContext = context;
    }

    [HttpGet]
    public ActionResult Index(PaymentsViewModel? viewModel)
    {

        // * get the offices availables
        var oficinas = this.pagoLineaContext.Oficinas.Where(item => item.Inactivo == false).ToList();

        if(viewModel == null)
        {
            viewModel = new PaymentsViewModel
            {
                Oficinas = oficinas,
                OficinaId = 1
            };
        }
        else
        {
            viewModel.Oficinas = this.pagoLineaContext.Oficinas.Where(item => item.Inactivo == false).ToList();
        }

        
        // * prepare the pagination data
        int take = 500;
        int skip = 0;
        int page = 1;

        ViewBag.Page = page;

        // * get the payment resumes
        viewModel.PaymentResumes = this.paymentResumeService.GetPaymentResumes(viewModel.OficinaId, take, skip)
            .Where( item => item.Fecha.Date >= viewModel.StartDate && item.Fecha.Date <= viewModel.EndDate)
            .ToList();

        // * return the view
        return View(viewModel);
    }


    [HttpGet]
    [Route("show/{paymentId}")]
    public ActionResult ShowPayment(int paymentId)
    {
        // * attempt to get the payment details
        var paymentDetails = paymentResumeService.GetPaymentDetails(paymentId);
        if(paymentDetails == null)
        {
            return NotFound();
        }

        return View(paymentDetails);
    }

    [HttpGet]
    [Route("uploadPayments")]
    public IActionResult UploadPayments()
    {
        return View();
    }

    [HttpPost]
    [Route("uploadPayments")]
    public IActionResult UploadPayments(IFormFile file)
    {
        if(file == null)
        {
            ViewBag.ErrorMessage = "Seleccione un archivo valido para subir";
            return View();
        }

        try
        {
            IEnumerable<PaymentFileRecord> records = [];
            using( var stream = file.OpenReadStream())
            {

                var csvRecords = ProcessCSV.LoadFile(stream);
                
                // * map the CSV data to the Transaction model
                records = csvRecords.Skip(1).Select( cols => PaymentFileRecordAdapter.Adapt(cols));
            }

            // * store the records in the session
            HttpContext.Session.SetString("paymentRecords", JsonSerializer.Serialize(records));

            // * return the view with the records
            return View(records);
        }
        catch (System.Exception ex)
        {
            var model = new ErrorViewModel
            {
                RequestId = ex.Message
            };
            return View("Error", model);
        }
    }

    [HttpPost]
    [Route("storePayments")]
    public async Task<IActionResult> StorePayments()
    {
        // * get the records from the session
        var recordsJson = HttpContext.Session.GetString("paymentRecords");
        if( recordsJson == null)
        {
            return Conflict("No records found");
        }
        IEnumerable<PaymentFileRecord> records = JsonSerializer.Deserialize<IEnumerable<PaymentFileRecord>>(recordsJson) ?? [];

        // * store the records in the database
        var recordsStored = await this.paymentResumeService.StorePaymentRecords(records);
        foreach (var record in recordsStored)
        {
            this._logger.LogInformation(">>" + record.ToString());
        }

        // * update the stored flag in the records
        records.Where( item => recordsStored.Contains( item.Id)).ToList().ForEach( item => item.Stored = true);
        
        return View(records);
    }


    [HttpGet("exportPayments")]
    public IActionResult ExportPayments(int oficinaId, DateTime startDate, DateTime endDate)
    {
        // * fetch your data (replace with actual data retrieval logic)
        PaymentDetails[] paymentDetails = this.paymentResumeService.GetPaymentDetailsOffice(oficinaId, startDate, endDate).ToArray();

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Pagos Registrados");

            // * add headers
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Fecha";
            worksheet.Cells[1, 3].Value = "Fecha Dispersion";
            worksheet.Cells[1, 4].Value = "Comercio";
            worksheet.Cells[1, 5].Value = "Unidad";
            worksheet.Cells[1, 6].Value = "Concepto";
            worksheet.Cells[1, 7].Value = "Referencia Comercio";
            worksheet.Cells[1, 8].Value = "Orden";
            worksheet.Cells[1, 9].Value = "Tipo";
            worksheet.Cells[1, 10].Value = "Medio Pago";
            worksheet.Cells[1, 11].Value = "Titular";
            worksheet.Cells[1, 12].Value = "Banco";
            worksheet.Cells[1, 13].Value = "Referencia Tarjeta";
            worksheet.Cells[1, 14].Value = "Tipo Tarjeta";
            worksheet.Cells[1, 15].Value = "Autorizacion";
            worksheet.Cells[1, 16].Value = "Iva Mensajeria";
            worksheet.Cells[1, 17].Value = "Servicio";
            worksheet.Cells[1, 18].Value = "Iva Servicio";
            worksheet.Cells[1, 19].Value = "Comision Comercio";
            worksheet.Cells[1, 20].Value = "Comision Usuario";
            worksheet.Cells[1, 21].Value = "Iva Comision";
            worksheet.Cells[1, 22].Value = "Total Importe";
            worksheet.Cells[1, 23].Value = "Total Cobrado";
            worksheet.Cells[1, 24].Value = "Promocion";
            worksheet.Cells[1, 25].Value = "Estado";
            worksheet.Cells[1, 26].Value = "Contrato";
            worksheet.Cells[1, 27].Value = "Mensaje Medio Pago";
            worksheet.Cells[1, 28].Value = "Email";
            worksheet.Cells[1, 29].Value = "Telefono";
            worksheet.Cells[1, 30].Value = "Plataforma";
            worksheet.Cells[1, 31].Value = "Estatus Reclamacion";
            worksheet.Cells[1, 32].Value = "Localidad";
            worksheet.Cells[1, 33].Value = "Id Padron";
            worksheet.Cells[1, 34].Value = "Af";
            worksheet.Cells[1, 35].Value = "Mf";
            worksheet.Cells[1, 36].Value = "Id Cuenta";
            worksheet.Cells[1, 37].Value = "Razon Social";
            worksheet.Cells[1, 38].Value = "Oficina Id";
            worksheet.Cells[1, 39].Value = "Oficina";

            // * add data
            for (int i = 0; i < paymentDetails.Count(); i++)
            {
                var record = paymentDetails[i];
                worksheet.Cells[i + 2, 1].Value = record.Id;
                worksheet.Cells[i + 2, 2].Value = record.Fecha.ToString("yyyy-MM-dd");
                worksheet.Cells[i + 2, 3].Value = record.FechaDispersion?.ToString("yyyy-MM-dd");
                worksheet.Cells[i + 2, 4].Value = record.Comercio;
                worksheet.Cells[i + 2, 5].Value = record.Unidad;
                worksheet.Cells[i + 2, 6].Value = record.Concepto;
                worksheet.Cells[i + 2, 7].Value = record.ReferenciaComercio;
                worksheet.Cells[i + 2, 8].Value = record.Orden;
                worksheet.Cells[i + 2, 9].Value = record.Tipo;
                worksheet.Cells[i + 2, 10].Value = record.MedioPago;
                worksheet.Cells[i + 2, 11].Value = record.Titular;
                worksheet.Cells[i + 2, 12].Value = record.Banco;
                worksheet.Cells[i + 2, 13].Value = record.ReferenciaTarjeta;
                worksheet.Cells[i + 2, 14].Value = record.TipoTarjeta;
                worksheet.Cells[i + 2, 15].Value = record.Autorizacion;
                worksheet.Cells[i + 2, 16].Value = record.IvaMensajeriaCurrency;
                worksheet.Cells[i + 2, 17].Value = record.ServicioCurrency;
                worksheet.Cells[i + 2, 18].Value = record.IvaServicioCurrency;
                worksheet.Cells[i + 2, 19].Value = record.ComisionComercioCurrency;
                worksheet.Cells[i + 2, 20].Value = record.ComisionUsuarioCurrency;
                worksheet.Cells[i + 2, 21].Value = record.IvaComisionCurrency;
                worksheet.Cells[i + 2, 22].Value = record.TotalImporteCurrency;
                worksheet.Cells[i + 2, 23].Value = record.TotalCobradoCurrency;
                worksheet.Cells[i + 2, 24].Value = record.Promocion;
                worksheet.Cells[i + 2, 25].Value = record.Estado;
                worksheet.Cells[i + 2, 26].Value = record.Contrato;
                worksheet.Cells[i + 2, 27].Value = record.MensajeMedioPago;
                worksheet.Cells[i + 2, 28].Value = record.Email;
                worksheet.Cells[i + 2, 29].Value = record.Telefono;
                worksheet.Cells[i + 2, 30].Value = record.Plataforma;
                worksheet.Cells[i + 2, 31].Value = record.EstatusReclamacion;
                worksheet.Cells[i + 2, 32].Value = record.Localidad;
                worksheet.Cells[i + 2, 33].Value = record.IdPadron;
                worksheet.Cells[i + 2, 34].Value = record.Af;
                worksheet.Cells[i + 2, 35].Value = record.Mf;
                worksheet.Cells[i + 2, 36].Value = record.IdCuenta;
                worksheet.Cells[i + 2, 37].Value = record.RazonSocial;
                worksheet.Cells[i + 2, 38].Value = record.OficinaId;
                worksheet.Cells[i + 2, 39].Value = record.Oficina;
            }

            // * autoFit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // * save the file to a memory stream
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            // * return the file
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "pagos_registrados.xlsx");
        }
    }

}

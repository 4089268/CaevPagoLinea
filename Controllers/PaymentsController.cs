using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
            return Conflict("The file is required");
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


            // * print the recors on the log
            foreach(var record in records)
            {
                this._logger.LogDebug(record.ToString());
            }

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

}

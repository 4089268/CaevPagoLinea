using System;
using System.Collections.Generic;
using System.Globalization;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Helpers;

public static class PaymentFileRecordAdapter
{
    public static PaymentFileRecord Adapt(string[] csvRow)
    {
        return new PaymentFileRecord
        {
            Id = long.Parse(csvRow[0]),
            Fecha = DateTime.ParseExact(csvRow[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
            FechaDeDispersion = DateTime.TryParseExact(csvRow[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dp) ?dp :null,
            Comercio = csvRow[3],
            Unidad = csvRow[4],
            Concepto = csvRow[5],
            ReferenciaComercio = csvRow[6],
            Orden = csvRow[7],
            Tipo = csvRow[8],
            MedioDePago = csvRow[9],
            Titular = csvRow[10],
            Banco = csvRow[11],
            ReferenciaTarjeta = csvRow[12],
            TipoTarjeta = csvRow[13],
            Autorizacion = csvRow[14],
            Mensajeria = ParseDecimal(csvRow[15]),
            IvaMensajeria = ParseDecimal(csvRow[16]),
            Servicio = ParseDecimal(csvRow[17]),
            IvaServicio = ParseDecimal(csvRow[18]),
            ComisionComercio = ParseDecimal(csvRow[19]),
            ComisionUsuario = ParseDecimal(csvRow[20]),
            IvaComision = ParseDecimal(csvRow[21]),
            TotalImporte = ParseDecimal(csvRow[22]),
            TotalCobrado = ParseDecimal(csvRow[23]),
            Promocion = csvRow[24],
            Estado = csvRow[25],
            Contrato = csvRow[26],
            MensajeMedioDePago = csvRow[27],
            Email = csvRow[28],
            Telefono = csvRow[29],
            Plataforma = csvRow[30],
            EstatusReclamacion = csvRow.Length > 31 ? csvRow[31] : string.Empty
        };
    }

    static private decimal ParseDecimal(string value)
    {
        return decimal.TryParse(value.Replace("\"", "")
            .Replace(",","")
            .Trim('$'), NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                ? result
                : 0m;
    }
}
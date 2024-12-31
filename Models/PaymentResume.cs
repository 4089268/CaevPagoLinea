using System;
using System.Data;
using System.Globalization;

namespace CAEV.PagoLinea.Models
{
    public class PaymentResume
    {
        public long Id { get; set; }
        public int IdCuenta { get; set; }
        public string RazonSocial { get; set; } = default!;
        public int Af { get; set; }
        public int Mf { get; set; }
        public DateTime Fecha { get; set; }
        public decimal TotalImporte { get; set; }
        public string Estatus { get; set; } = default!;
        public int OficinaId { get; set; }
        public string Oficina { get; set; } = default!;
        public string Concepto { get; set; } = default!;

        public string TotalImporteCurrency { get => TotalImporte.ToString("c2", new CultureInfo("es-MX")); }

        public static PaymentResume FromDataReader(IDataReader reader)
        {
            return new PaymentResume
            {
                Id = Convert.ToInt64(reader["id"]),
                IdCuenta = Convert.ToInt32(reader["cpad_idCuenta"]),
                RazonSocial = reader["cpad_razonSocial"].ToString()!,
                Af = Convert.ToInt32(reader["ref_af"]),
                Mf = Convert.ToInt32(reader["ref_mf"]),
                Fecha = Convert.ToDateTime(reader["fecha"]),
                TotalImporte = Convert.ToDecimal(reader["total_importe"]),
                Estatus = reader["mensaje_medio_pago"].ToString()!,
                OficinaId = Convert.ToInt32(reader["cpad_oficinaId"]),
                Oficina = reader["oficina"].ToString()!,
                Concepto = reader["concepto"].ToString()!
            };
        }
    }
}
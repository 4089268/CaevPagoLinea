using System;
using System.Data;
using System.Collections.Generic;

namespace CAEV.PagoLinea.Models
{
    public class PadronRecord {
        public int IdPadron { get; set; }
        public int IdCuenta { get; set; }
        public string RazonSocial { get; set; } = default!;
        public string? Localizacion { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string MesFacturado { get; set; } = default!;
        public string Direccion { get; set; } = default!;
        public string Ciudad { get; set; } = default!;

        public static PadronRecord FromDataReader(IDataReader reader ){
            var record = new PadronRecord {
                IdPadron = reader["id_padron"] != DBNull.Value ? Convert.ToInt32(reader["id_padron"]) : 0,
                IdCuenta = reader["id_cuenta"] != DBNull.Value ? Convert.ToInt32(reader["id_cuenta"]) : 0,
                RazonSocial = reader["razon_social"] != DBNull.Value ? reader["razon_social"].ToString() : string.Empty,
                Localizacion = reader["_localizacion"] != DBNull.Value ? reader["_localizacion"].ToString() : string.Empty,
                Subtotal = reader["subtotal"] != DBNull.Value ? Convert.ToDecimal(reader["subtotal"]) : 0m,
                Iva = reader["iva"] != DBNull.Value ? Convert.ToDecimal(reader["iva"]) : 0m,
                Total = reader["total"] != DBNull.Value ? Convert.ToDecimal(reader["total"]) : 0m,
                MesFacturado = reader["_mesFacturado"] != DBNull.Value ? reader["_mesFacturado"].ToString() : string.Empty,
                Direccion = reader["direccion"] != DBNull.Value ? reader["direccion"].ToString() : string.Empty,
                Ciudad = reader["ciudad"] != DBNull.Value ? reader["ciudad"].ToString() : string.Empty
            };
            return record;
        }
    }

}
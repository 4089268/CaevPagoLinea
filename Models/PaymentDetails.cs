using System;
using System.Data;

namespace CAEV.PagoLinea.Models
{
    public class PaymentDetails
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaDispersion { get; set; }
        public string Comercio { get; set; } = default!;
        public string Unidad { get; set; } = default!;
        public string Concepto { get; set; } = default!;
        public string ReferenciaComercio { get; set; } = default!;
        public string Orden { get; set; } = default!;
        public string Tipo { get; set; } = default!;
        public string MedioPago { get; set; } = default!;
        public string Titular { get; set; } = default!;
        public string Banco { get; set; } = default!;
        public string ReferenciaTarjeta { get; set; } = default!;
        public string TipoTarjeta { get; set; } = default!;
        public string Autorizacion { get; set; } = default!;
        public decimal IvaMensajeria { get; set; }
        public decimal Servicio { get; set; }
        public decimal IvaServicio { get; set; }
        public decimal ComisionComercio { get; set; }
        public decimal ComisionUsuario { get; set; }
        public decimal IvaComision { get; set; }
        public decimal TotalImporte { get; set; }
        public decimal TotalCobrado { get; set; }
        public string Promocion { get; set; } = default!;
        public string Estado { get; set; } = default!;
        public string Contrato { get; set; } = default!;
        public string MensajeMedioPago { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Telefono { get; set; } = default!;
        public string Plataforma { get; set; } = default!;
        public string EstatusReclamacion { get; set; } = default!;
        public int Localidad { get; set; }
        public int IdPadron { get; set; }
        public int Af { get; set; }
        public int Mf { get; set; }
        public int IdCuenta { get; set; }
        public string RazonSocial { get; set; } = default!;
        public int OficinaId { get; set; }
        public string Oficina { get; set; } = default!;

        public static PaymentDetails FromDataReader(IDataReader reader)
        {
            return new PaymentDetails
            {
                Id = reader.GetInt64(reader.GetOrdinal("id")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                FechaDispersion = reader.IsDBNull(reader.GetOrdinal("fecha_dispersion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fecha_dispersion")),
                Comercio = reader.GetString(reader.GetOrdinal("comercio")),
                Unidad = reader.GetString(reader.GetOrdinal("unidad")),
                Concepto = reader.GetString(reader.GetOrdinal("concepto")),
                ReferenciaComercio = reader.GetString(reader.GetOrdinal("referencia_comercio")),
                Orden = reader.GetString(reader.GetOrdinal("orden")),
                Tipo = reader.GetString(reader.GetOrdinal("tipo")),
                MedioPago = reader.GetString(reader.GetOrdinal("medio_pago")),
                Titular = reader.GetString(reader.GetOrdinal("titular")),
                Banco = reader.GetString(reader.GetOrdinal("banco")),
                ReferenciaTarjeta = reader.GetString(reader.GetOrdinal("referencia_tarjeta")),
                TipoTarjeta = reader.GetString(reader.GetOrdinal("tipo_tarjeta")),
                Autorizacion = reader.GetString(reader.GetOrdinal("autorizacion")),
                IvaMensajeria = reader.GetDecimal(reader.GetOrdinal("iva_mensajeria")),
                Servicio = reader.GetDecimal(reader.GetOrdinal("servicio")),
                IvaServicio = reader.GetDecimal(reader.GetOrdinal("iva_servicio")),
                ComisionComercio = reader.GetDecimal(reader.GetOrdinal("comision_comercio")),
                ComisionUsuario = reader.GetDecimal(reader.GetOrdinal("comision_usuario")),
                IvaComision = reader.GetDecimal(reader.GetOrdinal("iva_comision")),
                TotalImporte = reader.GetDecimal(reader.GetOrdinal("total_importe")),
                TotalCobrado = reader.GetDecimal(reader.GetOrdinal("total_cobrado")),
                Promocion = reader.GetString(reader.GetOrdinal("promocion")),
                Estado = reader.GetString(reader.GetOrdinal("estado")),
                Contrato = reader.GetString(reader.GetOrdinal("contrato")),
                MensajeMedioPago = reader.GetString(reader.GetOrdinal("mensaje_medio_pago")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                Telefono = reader.GetString(reader.GetOrdinal("telefono")),
                Plataforma = reader.GetString(reader.GetOrdinal("plataforma")),
                EstatusReclamacion = reader.GetString(reader.GetOrdinal("estatus_reclamacion")),
                Localidad = reader.GetInt32(reader.GetOrdinal("ref_localidad")),
                IdPadron = reader.GetInt32(reader.GetOrdinal("ref_idPadron")),
                Af = reader.GetInt32(reader.GetOrdinal("ref_af")),
                Mf = reader.GetInt32(reader.GetOrdinal("ref_mf")),
                IdCuenta = reader.GetInt32(reader.GetOrdinal("cpad_idCuenta")),
                RazonSocial = reader.GetString(reader.GetOrdinal("cpad_razonSocial")),
                OficinaId = reader.GetInt32(reader.GetOrdinal("cpad_oficinaId")),
                Oficina = reader.GetString(reader.GetOrdinal("oficina"))
            };
        }

    }
}
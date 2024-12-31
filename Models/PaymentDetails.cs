using System;
using System.Data;
using System.Globalization;

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


        public string IvaMensajeriaCurrency { get => IvaMensajeria.ToString("c2", new CultureInfo("es-MX")); }
        public string ServicioCurrency { get => Servicio.ToString("c2", new CultureInfo("es-MX")); }
        public string IvaServicioCurrency { get => IvaServicio.ToString("c2", new CultureInfo("es-MX")); }
        public string ComisionComercioCurrency { get => ComisionComercio.ToString("c2", new CultureInfo("es-MX")); }
        public string ComisionUsuarioCurrency { get => ComisionUsuario.ToString("c2", new CultureInfo("es-MX")); }
        public string IvaComisionCurrency { get => IvaComision.ToString("c2", new CultureInfo("es-MX")); }
        public string TotalImporteCurrency { get => TotalImporte.ToString("c2", new CultureInfo("es-MX")); }
        public string TotalCobradoCurrency { get => TotalCobrado.ToString("c2", new CultureInfo("es-MX")); }


        public static PaymentDetails FromDataReader(IDataReader reader)
        {
            return new PaymentDetails
            {
            Id = Convert.ToInt64(reader["id"]),
            Fecha = Convert.ToDateTime(reader["fecha"]),
            FechaDispersion = reader.IsDBNull(reader.GetOrdinal("fecha_dispersion")) ? (DateTime?)null : Convert.ToDateTime(reader["fecha_dispersion"]),
            Comercio = Convert.ToString(reader["comercio"]) ?? String.Empty,
            Unidad = Convert.ToString(reader["unidad"]) ?? String.Empty,
            Concepto = Convert.ToString(reader["concepto"]) ?? String.Empty,
            ReferenciaComercio = Convert.ToString(reader["referencia_comercio"]) ?? String.Empty,
            Orden = Convert.ToString(reader["orden"]) ?? String.Empty,
            Tipo = Convert.ToString(reader["tipo"]) ?? String.Empty,
            MedioPago = Convert.ToString(reader["medio_pago"]) ?? String.Empty,
            Titular = Convert.ToString(reader["titular"]) ?? String.Empty,
            Banco = Convert.ToString(reader["banco"]) ?? String.Empty,
            ReferenciaTarjeta = Convert.ToString(reader["referencia_tarjeta"]) ?? String.Empty,
            TipoTarjeta = Convert.ToString(reader["tipo_tarjeta"]) ?? String.Empty,
            Autorizacion = Convert.ToString(reader["autorizacion"]) ?? String.Empty,
            IvaMensajeria = Convert.ToDecimal(reader["iva_mensajeria"]),
            Servicio = Convert.ToDecimal(reader["servicio"]),
            IvaServicio = Convert.ToDecimal(reader["iva_servicio"]),
            ComisionComercio = Convert.ToDecimal(reader["comision_comercio"]),
            ComisionUsuario = Convert.ToDecimal(reader["comision_usuario"]),
            IvaComision = Convert.ToDecimal(reader["iva_comision"]),
            TotalImporte = Convert.ToDecimal(reader["total_importe"]),
            TotalCobrado = Convert.ToDecimal(reader["total_cobrado"]),
            Promocion = Convert.ToString(reader["promocion"]) ?? String.Empty,
            Estado = Convert.ToString(reader["estado"]) ?? String.Empty,
            Contrato = Convert.ToString(reader["contrato"]) ?? String.Empty,
            MensajeMedioPago = Convert.ToString(reader["mensaje_medio_pago"]) ?? String.Empty,
            Email = Convert.ToString(reader["email"]) ?? String.Empty,
            Telefono = Convert.ToString(reader["telefono"]) ?? String.Empty,
            Plataforma = Convert.ToString(reader["plataforma"]) ?? String.Empty,
            EstatusReclamacion = Convert.ToString(reader["estatus_reclamacion"]) ?? String.Empty,
            Localidad = Convert.ToInt32(reader["ref_localidad"]),
            IdPadron = Convert.ToInt32(reader["ref_idPadron"]),
            Af = Convert.ToInt32(reader["ref_af"]),
            Mf = Convert.ToInt32(reader["ref_mf"]),
            IdCuenta = Convert.ToInt32(reader["cpad_idCuenta"]),
            RazonSocial = Convert.ToString(reader["cpad_razonSocial"]) ?? String.Empty,
            OficinaId = Convert.ToInt32(reader["cpad_oficinaId"]),
            Oficina = Convert.ToString(reader["oficina"]) ?? String.Empty
            };
        }

    }
}
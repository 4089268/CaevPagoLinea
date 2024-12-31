using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CAEV.PagoLinea.Models;
using Microsoft.AspNetCore.Authentication;

namespace CAEV.PagoLinea.Services
{
    public class PaymentResumeService(IConfiguration c, ILogger<PaymentResumeService> l) : IPaymentResume
    {

        private readonly IConfiguration configuration = c;
        private readonly ILogger<PaymentResumeService> logger = l;
        private readonly int commandTimeout = (int) TimeSpan.FromMinutes(12).TotalSeconds;


        public ICollection<PaymentResume> GetPaymentResumes()
        {
            List<PaymentResume> paymentResumes = new();

            var query = @"
            SELECT  id,
                cpad_idCuenta,
                cpad_razonSocial,
                ref_af,
                ref_mf,
                fecha,
                total_importe,
                mensaje_medio_pago,
                cpad_oficinaId,
                oficina,
                concepto
            FROM [dbo].[vw_ConciliacionBancoDetails]
            ORDER BY fecha desc";

            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("PagoLinea"))) {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = commandTimeout;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            paymentResumes.Add(PaymentResume.FromDataReader(reader));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                this.logger.LogError(ex, "SQL Error at get the resume of the payments: {m}", ex.Message);
            }
            catch (Exception ex) {
                this.logger.LogError(ex, "Error at get the resume of the payments: {m}", ex.Message);
            }

            return paymentResumes;
        }

        public ICollection<PaymentResume> GetPaymentResumes(int officeID, int take, int skip)
        {
            List<PaymentResume> paymentResumes = new();

            var query = @"
            SELECT id,
                cpad_idCuenta,
                cpad_razonSocial,
                ref_af,
                ref_mf,
                fecha,
                total_importe,
                mensaje_medio_pago,
                cpad_oficinaId,
                oficina,
                concepto
            FROM [dbo].[vw_ConciliacionBancoDetails]
            WHERE cpad_oficinaId = @officeID
            ORDER BY fecha DESC
            OFFSET @skip ROWS
            FETCH NEXT @take ROWS ONLY";

            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("PagoLinea"))) {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = commandTimeout;
                    command.Parameters.AddWithValue("@officeID", officeID);
                    command.Parameters.AddWithValue("@take", take);
                    command.Parameters.AddWithValue("@skip", skip);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            paymentResumes.Add( PaymentResume.FromDataReader(reader) );
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                this.logger.LogError(ex, "SQL Error at get the resume of the payments: {m}", ex.Message);
            }
            catch (Exception ex) {
                this.logger.LogError(ex, "Error at get the resume of the payments: {m}", ex.Message);
            }

            return paymentResumes.Skip(skip).Take(take).ToList();
        }

        public PaymentDetails? GetPaymentDetails(int paymentId)
        {
            PaymentDetails? recordDetails = null;
            
            var query = @"
            SELECT  id,
                fecha,
                fecha_dispersion,
                comercio,
                unidad,
                concepto,
                referencia_comercio,
                orden,
                tipo,
                medio_pago,
                titular,
                banco,
                referencia_tarjeta,
                tipo_tarjeta,
                autorizacion,
                mensajeria,
                iva_mensajeria,
                servicio,
                iva_servicio,
                comision_comercio,
                comision_usuario,
                iva_comision,
                total_importe,
                total_cobrado,
                promocion,
                estado,
                contrato,
                mensaje_medio_pago,
                email,
                telefono,
                plataforma,
                estatus_reclamacion,
                ref_localidad,
                ref_idPadron,
                ref_af,
                ref_mf,
                cpad_idCuenta,
                cpad_razonSocial,
                cpad_oficinaId,
                oficina
            FROM [dbo].[vw_ConciliacionBancoDetails]
            WHER id = @id";

            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("PagoLinea"))) {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = commandTimeout;
                    command.Parameters.AddWithValue("@id", paymentId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            recordDetails = PaymentDetails.FromDataReader(reader);
                        }
                        else
                        {
                            throw new KeyNotFoundException("Payment id not found");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                this.logger.LogError(ex, "SQL Error at get the details of the payment '{id}': {m}", paymentId, ex.Message);
            }
            catch (Exception ex) {
                this.logger.LogError(ex, "Error at get the details of the payment '{id}': {m}", paymentId, ex.Message);
            }

            return recordDetails;
        }
    
    }

}
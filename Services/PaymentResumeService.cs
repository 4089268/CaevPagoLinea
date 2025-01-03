using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Services
{
    public class PaymentResumeService(IConfiguration c, ILogger<PaymentResumeService> l) : IPaymentResume
    {

        private readonly IConfiguration configuration = c;
        private readonly ILogger<PaymentResumeService> logger = l;
        private readonly int commandTimeout = (int) TimeSpan.FromMinutes(12).TotalSeconds;
        private readonly int storeRecordChunkSize = 500;


        public IEnumerable<PaymentResume> GetPaymentResumes()
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

        public IEnumerable<PaymentResume> GetPaymentResumes(int officeID, int take, int skip)
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
            SELECT id,
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
            WHERE id = @id";

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

        public IEnumerable<PaymentDetails> GetPaymentDetailsOffice(int officeId, DateTime from, DateTime to)
        {
            List<PaymentDetails> recordDetails = new();
            
            var query = @"
            SELECT id,
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
            WHERE cpad_oficinaId = @oficinaId and fecha >= @from and fecha <= @to"; ;

            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("PagoLinea"))) {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = commandTimeout;
                    command.Parameters.AddWithValue("@oficinaId", officeId);
                    command.Parameters.AddWithValue("@from", from.Date);
                    command.Parameters.AddWithValue("@to", to.Date);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recordDetails.Add( PaymentDetails.FromDataReader(reader));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                this.logger.LogError(ex, "SQL Error at get the details of the office '{id}': {m}", officeId, ex.Message);
            }
            catch (Exception ex) {
                this.logger.LogError(ex, "Error at get the details of the office '{id}': {m}", officeId, ex.Message);
            }

            return recordDetails;
        }

        public async Task<IEnumerable<long>> StorePaymentRecords(IEnumerable<PaymentFileRecord> records)
        {
            var storedIds = new List<long>();

            using (var connection = new SqlConnection(configuration.GetConnectionString("PagoLinea")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;

                        // * define the INSERT query with a conditional check
                        command.CommandText = @"
                            IF NOT EXISTS (
                                SELECT 1 
                                FROM [dbo].[Conciliacion_Banco] 
                                WHERE [id] = @Id
                            )
                            BEGIN
                                INSERT INTO [dbo].[Conciliacion_Banco] (
                                    [id], [fecha], [fecha_dispersion], [comercio], [unidad], 
                                    [concepto], [referencia_comercio], [orden], [tipo], [medio_pago], 
                                    [titular], [banco], [referencia_tarjeta], [tipo_tarjeta], [autorizacion], 
                                    [mensajeria], [iva_mensajeria], [servicio], [iva_servicio], 
                                    [comision_comercio], [comision_usuario], [iva_comision], 
                                    [total_importe], [total_cobrado], [promocion], [estado], 
                                    [contrato], [mensaje_medio_pago], [email], [telefono], 
                                    [plataforma], [estatus_reclamacion]
                                ) VALUES (
                                    @Id, @Fecha, @FechaDeDispersion, @Comercio, @Unidad, 
                                    @Concepto, @ReferenciaComercio, @Orden, @Tipo, @MedioDePago, 
                                    @Titular, @Banco, @ReferenciaTarjeta, @TipoTarjeta, @Autorizacion, 
                                    @Mensajeria, @IvaMensajeria, @Servicio, @IvaServicio, 
                                    @ComisionComercio, @ComisionUsuario, @IvaComision, 
                                    @TotalImporte, @TotalCobrado, @Promocion, @Estado, 
                                    @Contrato, @MensajeMedioDePago, @Email, @Telefono, 
                                    @Plataforma, @EstatusReclamacion
                                );

                                SELECT @Id;
                            END";

                        // * add parameters
                        command.Parameters.Add("@Id", SqlDbType.NChar, 15);
                        command.Parameters.Add("@Fecha", SqlDbType.DateTime);
                        command.Parameters.Add("@FechaDeDispersion", SqlDbType.DateTime);
                        command.Parameters.Add("@Comercio", SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Unidad", SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Concepto", SqlDbType.VarChar, 80);
                        command.Parameters.Add("@ReferenciaComercio", SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Orden", SqlDbType.VarChar, 60);
                        command.Parameters.Add("@Tipo", SqlDbType.VarChar, 25);
                        command.Parameters.Add("@MedioDePago", SqlDbType.VarChar, 40);
                        command.Parameters.Add("@Titular", SqlDbType.VarChar, 90);
                        command.Parameters.Add("@Banco", SqlDbType.VarChar, 60);
                        command.Parameters.Add("@ReferenciaTarjeta", SqlDbType.VarChar, 50);
                        command.Parameters.Add("@TipoTarjeta", SqlDbType.VarChar, 10);
                        command.Parameters.Add("@Autorizacion", SqlDbType.VarChar, 15);
                        command.Parameters.Add("@Mensajeria", SqlDbType.Money);
                        command.Parameters.Add("@IvaMensajeria", SqlDbType.Money);
                        command.Parameters.Add("@Servicio", SqlDbType.Money);
                        command.Parameters.Add("@IvaServicio", SqlDbType.Money);
                        command.Parameters.Add("@ComisionComercio", SqlDbType.Money);
                        command.Parameters.Add("@ComisionUsuario", SqlDbType.Money);
                        command.Parameters.Add("@IvaComision", SqlDbType.Money);
                        command.Parameters.Add("@TotalImporte", SqlDbType.Money);
                        command.Parameters.Add("@TotalCobrado", SqlDbType.Money);
                        command.Parameters.Add("@Promocion", SqlDbType.VarChar, 20);
                        command.Parameters.Add("@Estado", SqlDbType.VarChar, 30);
                        command.Parameters.Add("@Contrato", SqlDbType.VarChar, 20);
                        command.Parameters.Add("@MensajeMedioDePago", SqlDbType.VarChar, 30);
                        command.Parameters.Add("@Email", SqlDbType.VarChar, 150);
                        command.Parameters.Add("@Telefono", SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Plataforma", SqlDbType.VarChar, 30);
                        command.Parameters.Add("@EstatusReclamacion", SqlDbType.VarChar, 50);

                        try
                        {
                            var i = 1;
                            foreach (var record in records)
                            {
                                // * assign parameter values
                                command.Parameters["@Id"].Value = record.Id.ToString();
                                command.Parameters["@Fecha"].Value = record.Fecha;
                                command.Parameters["@FechaDeDispersion"].Value = (object)record.FechaDeDispersion ?? DBNull.Value;
                                command.Parameters["@Comercio"].Value = record.Comercio;
                                command.Parameters["@Unidad"].Value = record.Unidad;
                                command.Parameters["@Concepto"].Value = record.Concepto;
                                command.Parameters["@ReferenciaComercio"].Value = record.ReferenciaComercio;
                                command.Parameters["@Orden"].Value = record.Orden;
                                command.Parameters["@Tipo"].Value = record.Tipo;
                                command.Parameters["@MedioDePago"].Value = record.MedioDePago;
                                command.Parameters["@Titular"].Value = record.Titular;
                                command.Parameters["@Banco"].Value = record.Banco;
                                command.Parameters["@ReferenciaTarjeta"].Value = record.ReferenciaTarjeta;
                                command.Parameters["@TipoTarjeta"].Value = record.TipoTarjeta;
                                command.Parameters["@Autorizacion"].Value = record.Autorizacion;
                                command.Parameters["@Mensajeria"].Value = record.Mensajeria;
                                command.Parameters["@IvaMensajeria"].Value = record.IvaMensajeria;
                                command.Parameters["@Servicio"].Value = record.Servicio;
                                command.Parameters["@IvaServicio"].Value = record.IvaServicio;
                                command.Parameters["@ComisionComercio"].Value = record.ComisionComercio;
                                command.Parameters["@ComisionUsuario"].Value = record.ComisionUsuario;
                                command.Parameters["@IvaComision"].Value = record.IvaComision;
                                command.Parameters["@TotalImporte"].Value = record.TotalImporte;
                                command.Parameters["@TotalCobrado"].Value = record.TotalCobrado;
                                command.Parameters["@Promocion"].Value = record.Promocion;
                                command.Parameters["@Estado"].Value = record.Estado;
                                command.Parameters["@Contrato"].Value = record.Contrato;
                                command.Parameters["@MensajeMedioDePago"].Value = record.MensajeMedioDePago;
                                command.Parameters["@Email"].Value = record.Email;
                                command.Parameters["@Telefono"].Value = record.Telefono;
                                command.Parameters["@Plataforma"].Value = record.Plataforma;
                                command.Parameters["@EstatusReclamacion"].Value = record.EstatusReclamacion;

                                this.logger.LogInformation("Storing record: {record}; {i}-{j}", record, i, records.Count());

                                // * execute the command
                                var result = command.ExecuteScalar();
                                if(result != null)
                                {
                                    this.logger.LogDebug("Response: {result}", result.ToString()!);
                                    storedIds.Add(Convert.ToInt64(result.ToString()!));
                                }
                                i ++;
                            }

                            transaction.Commit();
                        }
                        catch(Exception err)
                        {
                            this.logger.LogError(err, "Error at store the payment records: {m}", err.Message);
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }

            await Task.CompletedTask;
            return storedIds;
        }
    
    }

}
using System;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CAEV.PagoLinea.Services {

    public class InvoiceService (ILogger<InvoiceService> logger, PagoLineaContext context, IOptions<MultipagoSettings> options) {
        private readonly ILogger<InvoiceService> logger = logger;
        private readonly PagoLineaContext pagoLineaContext = context;
        private readonly MultipagoSettings multipagoSettings = options.Value;

        /// <summary>
        /// </summary>
        /// <param name="padron"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Localidad not found</exception>
        public LayoutEnvio MakeInvoiceRequestPayload(CuentaPadron padron){

            // * get the conceptId base on the localidad
            var _localidad = this.pagoLineaContext.Localidades.FirstOrDefault(item => item.Id == padron.IdLocalidad);
            if( _localidad == null) {
                throw new KeyNotFoundException($"La localidad '{padron.IdLocalidad}' del padron '{padron.IdPadron}' no se encontro en el sistema" );
            }
            var conceptId = 1;
            switch(_localidad.OficinaId){
                case 2:
                    conceptId = LayoutEnvioConceptos.TUXPAN;
                    break;
                case 27:
                    conceptId = LayoutEnvioConceptos.MARTINEZ_DE_LA_TORRE;
                    break;
            }

            
            // * prepare the paylod for sending to the payment sevice
            var orderID = new string(Guid.NewGuid().ToString().Replace("-","").Take(30).ToArray());
            var layouRequest = new LayoutEnvio {
                Account = this.multipagoSettings.Account,
                Product = this.multipagoSettings.Product,
                Node = this.multipagoSettings.Node.ToString(),
                Concept = conceptId.ToString(),
                Ammount = padron.Total,
                Customername = padron.RazonSocial,
                Currency = LayoutEnvioCurrency.PesosMexicanos,
                Urlsuccess = this.multipagoSettings.UrlResponse + "?s=1",
                Urlfailure = this.multipagoSettings.UrlResponse + "?s=0",
                Order = orderID.ToString(),
                Reference = ReferenceMaker.GetReference(padron)
            };

            // * make the signature
            layouRequest.Signature = HashUtils.GetHash2(
                string.Format("{0}{1}{2}", layouRequest.Order, layouRequest.Reference, layouRequest.AmmountString),
                this.multipagoSettings.Key
            );

            // * save a record of the payload
            var orderPayment = new OrderPayment {
                Code = orderID,
                IdLocalidad = padron.IdLocalidad,
                IdPadron = padron.IdPadron,
                Ammount = layouRequest.Ammount,
                Reference = layouRequest.Reference,
                Concept = layouRequest.Concept,
                Node = layouRequest.Node,
                CreatedAt = DateTime.Now
            };
            this.pagoLineaContext.OrdersPayment.Add(orderPayment);
            this.pagoLineaContext.SaveChanges();
            this.logger.LogInformation("New order payment record saved with id {orderCode}", orderPayment.Code);

            return layouRequest;
        }

        public ValidatePaymentViewModel ProcessPayment(LayoutResponse response, int statusCode){
            // * validate the response
            var _validationSignaturePayload = string.Format("{0}{1}{2}{3}", response.MpOrder, response.MpReference, response.AmmountString, response.MpAuthorization);
            var _validationSignature = HashUtils.GetHash2( _validationSignaturePayload, this.multipagoSettings.Key );

            logger.LogDebug("Response validation signature payload: [{payload}]", _validationSignaturePayload);
            logger.LogDebug("Response validation signature:[{signature}]", _validationSignature);
            logger.LogDebug("Response signature:[{signature}]", response.MpSignature);

            if( response.MpSignature != _validationSignature){
                throw new SecurityTokenInvalidSignatureException("The response signature is invalid.");
            }

            // * retrive the orderId
            var orderPayment = this.pagoLineaContext.OrdersPayment.Find(response.MpOrder);
            if( orderPayment == null){
                throw new KeyNotFoundException($"A order payment with code '{response.MpOrder}' was not found on the system.");
            }

            // * update the date and the response code
            orderPayment.ResponseAt = DateTime.Now;
            orderPayment.ResponseCode = response.MpResponse;
            orderPayment.Authorization = response.MpAuthorization;
            this.pagoLineaContext.OrdersPayment.Update(orderPayment);
            this.pagoLineaContext.SaveChanges();

            logger.LogDebug("Response order: {orderId}", response.MpOrder);


            // * get the padron assosiated to the payment
            var padron = this.pagoLineaContext.CuentasPadron.First(item => item.IdPadron == orderPayment.IdPadron && item.IdLocalidad == orderPayment.IdLocalidad);

            // * prepare the response
            var validatePaymentViewModel = new ValidatePaymentViewModel {
                TransactionId = orderPayment.Code,
                Date = orderPayment.ResponseAt!.Value,
                UserName = padron.RazonSocial,
                UserAccount = padron.IdCuenta.ToString(),
                Ammount = orderPayment.Ammount,
            };

            if( statusCode == 0){
                validatePaymentViewModel.PaymentStatus = ValidatePaymentViewModel.PaymentStatuses.Fail;
                validatePaymentViewModel.Status = "PAGO RECHAZADO";
                return validatePaymentViewModel;
            }

            if( response.MpAuthorization == "000000"){
                validatePaymentViewModel.PaymentStatus = ValidatePaymentViewModel.PaymentStatuses.Pending;
                validatePaymentViewModel.Status = "EN PROCESO DE PAGO";
                return validatePaymentViewModel;
            }

            int authorizationId = int.TryParse(response.MpAuthorization, out int tmpAuthId)?tmpAuthId:0;
            if( authorizationId == 0){
                validatePaymentViewModel.PaymentStatus = ValidatePaymentViewModel.PaymentStatuses.Fail;
                validatePaymentViewModel.Status = "PAGO NO EXITOSO";
                return validatePaymentViewModel;
            }

            validatePaymentViewModel.PaymentStatus = ValidatePaymentViewModel.PaymentStatuses.Success;
            validatePaymentViewModel.Status = "PAGO EXITOSO";
            return validatePaymentViewModel;

        }
    }

}
using System;
using Microsoft.Extensions.Options;
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
                Urlsuccess = "https://caev.gob.mx/caev/confirmar-pago?status=1",
                Urlfailure = "https://caev.gob.mx/caev/confirmar-pago?status=0",
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

    }
    
}
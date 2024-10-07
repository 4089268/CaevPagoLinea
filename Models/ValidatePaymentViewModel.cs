using System;
using System.Globalization;

namespace CAEV.PagoLinea.Models
{

    public class ValidatePaymentViewModel {

        public enum PaymentStatuses
        {
            Success,
            Pending,
            Fail
        }

        public PaymentStatuses PaymentStatus {get;set;}
        public string TransactionId {get;set;} = string.Empty;
        public DateTime Date {get;set;}
        public string UserName {get;set;} = string.Empty;
        public string UserAccount {get;set;} = string.Empty;
        public Decimal Ammount {get;set;}
        public string Status {get;set;} = string.Empty;
        public string AmmountFormat {
            get {
                return Ammount.ToString("c2", new CultureInfo("es-MX"));
            }
        }
    }
    
}
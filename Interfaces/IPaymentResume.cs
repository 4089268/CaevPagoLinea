using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Services
{
    public interface IPaymentResume
    {

        public ICollection<PaymentResume> GetPaymentResumes();

        public ICollection<PaymentResume> GetPaymentResumes(int officeID, int take, int skip);

        public PaymentDetails? GetPaymentDetails(int paymentId);

    }
}
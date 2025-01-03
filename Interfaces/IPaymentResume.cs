using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Services
{
    public interface IPaymentResume
    {

        public IEnumerable<PaymentResume> GetPaymentResumes();

        public IEnumerable<PaymentResume> GetPaymentResumes(int officeID, int take, int skip);

        public PaymentDetails? GetPaymentDetails(int paymentId);

        public IEnumerable<PaymentDetails> GetPaymentDetailsOffice(int officeId, DateTime from, DateTime to);

        public Task<IEnumerable<long>> StorePaymentRecords(IEnumerable<PaymentFileRecord> records);

    }
}
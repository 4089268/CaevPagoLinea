using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;

namespace CAEV.PagoLinea.Models
{
    public class PaymentsViewModel
    {
        public IEnumerable<CatOficina> Oficinas { get; set; }
        public IEnumerable<PaymentResume> PaymentResumes { get; set; }

        public int OficinaId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public PaymentsViewModel()
        {
            OficinaId = 1;
            Oficinas = [];
            PaymentResumes = [];
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }
    }

}
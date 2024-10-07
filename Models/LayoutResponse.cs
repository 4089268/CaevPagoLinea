using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CAEV.PagoLinea.Models {
    public class LayoutResponse {
        
        public int MpAccount { get; set; }
        public string MpOrder { get; set; } = default!;
        public string MpReference { get; set; } = default!;
        public string MpConcept { get; set; } = default!;
        public string MpResponse { get; set; } = default!;
        public string MpSignature { get; set; } = default!;
        public decimal MpAmount { get; set; }
        public string MpPaymentMethod { get; set; } = default!;
        public string MpAuthorization { get; set; } = default!;

        public string? MpNode { get; set; }
        public string? MpCurrency { get; set; }
        public string? MpPaymentMethodCode { get; set; }
        public string? MpPaymentMethodComplete { get; set; }
        public string? MpResponseComplete { get; set; }
        public string? MpResponseMsg { get; set; }
        public string? MpResponseMsgComplete { get; set; }
        public string? MpAuthorizationComplete { get; set; }
        public string? MpPan { get; set; }
        public string? MpPanComplete { get; set; }
        public DateTime? MpDate { get; set; }
        public string? MpCustomerName { get; set; }
        public string? MpPromoMsi { get; set; }
        public string? MpBankCode { get; set; }
        public string? MpSaleId { get; set; }
        public string? MpSaleHistoryId { get; set; }
        public string? MpTrxHistoryId { get; set; }
        public string? MpTrxHistoryIdComplete { get; set; }
        public string? MpBankName { get; set; }
        public string? MpFolio { get; set; }
        public string? MpCardHolderName { get; set; }
        public string? MpCardHolderNameComplete { get; set; }
        public string? MpAuthorizationMp1 { get; set; }
        public string? MpPhone { get; set; }
        public string? MpEmail { get; set; }
        public string? MpPromo { get; set; }
        public string? MpPromoMsiBank { get; set; }
        public string? MpSecurePayment { get; set; }
        public string? MpCardType { get; set; }
        public string? MpPlatform { get; set; }
        public string? MpContract { get; set; }
        public string? MpCieInterClabe { get; set; }
        public string? MpCommerceName { get; set; }
        public string? MpCommerceNameLegal { get; set; }
        public string? MpCieInterReference { get; set; }
        public string? MpCieInterConcept { get; set; }
        public string? MpSbToken { get; set; }
        public bool? MpV2View { get; set; }
        public string? MpBrowser { get; set; }
        public string? MpSo { get; set; }
        public string? MpDevice { get; set; }

        public string AmmountString {
            get => MpAmount.ToString("F2", new CultureInfo("es-MX"));
        }

    }

}
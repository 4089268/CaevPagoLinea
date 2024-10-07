using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CAEV.PagoLinea.Models {
    public class LayoutResponse {
        
        [JsonPropertyName("mp_order")]
        public string Order { get; set; } = default!;

        [JsonPropertyName("mp_reference")]
        public string Reference { get; set; } = default!;

        [JsonPropertyName("mp_amount")]
        public decimal Ammount { get; set; }

        [JsonPropertyName("mp_response")]
        public string Response { get; set; } = default!;

        [JsonPropertyName("mp_authorization")]
        public string Authorization { get; set; } = default!;

        [JsonPropertyName("mp_signature")]
        public string Signature { get; set; } = default!;

        public string AmmountString {
            get => Ammount.ToString("F2", new CultureInfo("es-MX"));
        }

    }

}
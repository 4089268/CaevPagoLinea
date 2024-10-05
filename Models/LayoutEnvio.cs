using System;
using System.Collections.Generic;
using System.Globalization;

namespace CAEV.PagoLinea {
    public class LayoutEnvio {

        public int Account {get;set;}
        public int Product {get;set;}
        public string Order {get;set;} = default!;
        public string Reference {get;set;} = default!;
        public string Node {get;set;} = default!;
        public string Concept {get;set;} = default!;
        public decimal Ammount {get;set;}
        public string Customername {get;set;} = default!;
        public decimal Currency {get;set;}
        public string Signature {get;set;} = default!;
        public string Urlsuccess {get;set;} = default!;
        public string Urlfailure {get;set;} = default!;

        public string AmmountString {
            get => Ammount.ToString("F2", new CultureInfo("es-MX"));
        }

    }
    
    public class LayoutEnvioCurrency {
        public static readonly int PesosMexicanos = 1;
        public static readonly int DolaresAmericanos = 2;
    }
    
    public class LayoutEnvioConceptos {
        public static readonly int PANUCO = 1;
    }
}
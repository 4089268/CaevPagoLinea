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
        public static readonly int PAPANTLA = 2;
        public static readonly int TUXPAN = 3;
        public static readonly int MARTINEZ_DE_LA_TORRE = 4;
        public static readonly int VEGA_DE_ALATORRE = 5;
        public static readonly int GUTIERREZ_ZAMORA = 6;
        public static readonly int COATZINTLA = 7;
        public static readonly int CERRO_AZUL = 8;
        public static readonly int CHICONTEPEC = 9;
        public static readonly int ALAMO = 10;
        public static readonly int TANTOYUCA = 11;
        public static readonly int TEMPOAL = 12;
        public static readonly int OTATITLAN = 13;
        public static readonly int COSAMALOAPAN = 14;
        public static readonly int ANGEL_CABADA = 15;
        public static readonly int SANTIAGO_TUXTLA = 16;
        public static readonly int CATEMACO = 17;
        public static readonly int ACAYUCAN = 18;
        public static readonly int LAS_CHOAPAS = 19;
        public static readonly int AGUA_DULCE = 20;
        public static readonly int PLAYA_VICENTE = 21;
        public static readonly int NANCHITAL = 22;
        public static readonly int JESUS_CARRANZA = 23;
        public static readonly int CUITLAHUAC = 24;
        public static readonly int PENUELA = 25;
        public static readonly int YANGA = 26;
        public static readonly int TEZONAPA = 27;
        public static readonly int NOGALES = 28;
        public static readonly int RIO_BLANCO = 29;
        public static readonly int ALTOTONGA = 30;
        public static readonly int JALACINGO = 31;
        public static readonly int XICO = 32;
        public static readonly int TLALTETELA = 33;

    }
}
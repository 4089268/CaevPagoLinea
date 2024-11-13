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

        public static readonly int ALVARADO = 34;
        public static readonly int ANTON_LIZARDO = 35;
        public static readonly int CARLOS_A_CARRILLO = 36;
        public static readonly int CD_ALEMAN = 37;
        public static readonly int CD_CUAUHTEMOC = 38;
        public static readonly int CD_MENDOZA = 39;
        public static readonly int CHACALTIANGUIS = 40;
        public static readonly int CHICHICAXTLE = 41;
        public static readonly int CHINAMECA = 42;
        public static readonly int EL_HIGO = 43;
        public static readonly int EMILIO_CARRANZA = 44;
        public static readonly int IXHUATLAN_DEL_CAFE = 45;
        public static readonly int IXHUATLAN_DEL_SURESTE = 46;
        public static readonly int JUAN_DIAZ_COVARRUBIAS = 47;
        public static readonly int JUAN_RGUEZ_CLARA = 48;
        public static readonly int LAS_VIGAS = 49;
        public static readonly int MISANTLA = 50;
        public static readonly int PASO_DEL_MACHO = 51;
        public static readonly int PEROTE = 52;
        public static readonly int PIEDRAS_NEGRAS = 53;
        public static readonly int POZA_RICA = 54;
        public static readonly int PLATON_SANCHEZ = 55;
        public static readonly int SALTABARRANCA = 56;
        public static readonly int SAYULA_DE_ALEMAN = 57;
        public static readonly int SOLEDAD_DE_DOBLADO = 58;
        public static readonly int TEPETZINTLA = 59;
        public static readonly int TLACOTALPAN = 60;
        public static readonly int TLAPACOYAN = 61;
        public static readonly int TOMATLAN = 62;
        public static readonly int TRES_VALLES = 63;
        public static readonly int VILLA_AZUETA = 64;

    }
}
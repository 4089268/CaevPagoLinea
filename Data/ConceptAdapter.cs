using System;
using CAEV.PagoLinea;

namespace CAEV.PagoLinea.Data
{
    public class ConceptAdapter {

        private readonly IEnumerable<OfficeConcepto> ConceptosOficinas;

        public ConceptAdapter(){
            ConceptosOficinas = new List<OfficeConcepto> {
                new OfficeConcepto { OfficeName = "POZA RICA", OfficeId = 1, ConceptoId = 54 },
                new OfficeConcepto { OfficeName = "TUXPAN", OfficeId = 2, ConceptoId = 3 },
                new OfficeConcepto { OfficeName = "CERRO AZUL", OfficeId = 3, ConceptoId = 8 },
                new OfficeConcepto { OfficeName = "COATZINTLA", OfficeId = 4, ConceptoId = 7 },
                new OfficeConcepto { OfficeName = "CUITLAHUAC", OfficeId = 5, ConceptoId = 24 },
                new OfficeConcepto { OfficeName = "TANTOYUCA", OfficeId = 6, ConceptoId = 11 },
                new OfficeConcepto { OfficeName = "PANUCO", OfficeId = 7, ConceptoId = 1 },
                new OfficeConcepto { OfficeName = "MORALILLO", OfficeId = 8, ConceptoId = null },
                new OfficeConcepto { OfficeName = "ISLA", OfficeId = 9, ConceptoId = null },
                new OfficeConcepto { OfficeName = "ALTOTONGA", OfficeId = 10, ConceptoId = 30 },
                new OfficeConcepto { OfficeName = "COSAMALOAPAN", OfficeId = 11, ConceptoId = 14 },
                new OfficeConcepto { OfficeName = "RURALES COSAMALOAPAN", OfficeId = 12, ConceptoId = 14 },
                new OfficeConcepto { OfficeName = "NANCHITAL", OfficeId = 13, ConceptoId = 22 },
                new OfficeConcepto { OfficeName = "COSOLEACAQUE", OfficeId = 14, ConceptoId = null },
                new OfficeConcepto { OfficeName = "ACAYUCAN", OfficeId = 15, ConceptoId = 18 },
                new OfficeConcepto { OfficeName = "MINATITLAN", OfficeId = 16, ConceptoId = null },
                new OfficeConcepto { OfficeName = "PAPANTLA", OfficeId = 17, ConceptoId = 2 },
                new OfficeConcepto { OfficeName = "ALAMO", OfficeId = 18, ConceptoId = 10 },
                new OfficeConcepto { OfficeName = "SOLEDAD DE DOBLADO", OfficeId = 19, ConceptoId = 58 },
                new OfficeConcepto { OfficeName = "RIO BLANCO", OfficeId = 20, ConceptoId = 29 },
                new OfficeConcepto { OfficeName = "YANGA", OfficeId = 21, ConceptoId = 26 },
                new OfficeConcepto { OfficeName = "ALVARADO", OfficeId = 22, ConceptoId = 34 },
                new OfficeConcepto { OfficeName = "RURALES ALVARADO", OfficeId = 23, ConceptoId = 34 },
                new OfficeConcepto { OfficeName = "LAS CHOAPAS", OfficeId = 24, ConceptoId = 19 },
                new OfficeConcepto { OfficeName = "AGUA DULCE", OfficeId = 25, ConceptoId = 20 },
                new OfficeConcepto { OfficeName = "NARANJOS", OfficeId = 26, ConceptoId = null },
                new OfficeConcepto { OfficeName = "MARTINEZ DE LA TORRE", OfficeId = 27, ConceptoId = 4 },
                new OfficeConcepto { OfficeName = "TLACOTALPAN", OfficeId = 28, ConceptoId = 60 },
                new OfficeConcepto { OfficeName = "ANGEL R. CABADA", OfficeId = 29, ConceptoId = 15 },
                new OfficeConcepto { OfficeName = "CATEMACO", OfficeId = 30, ConceptoId = 17 },
                new OfficeConcepto { OfficeName = "CD. CUAUHTEMOC", OfficeId = 31, ConceptoId = 38 },
                new OfficeConcepto { OfficeName = "CD. MENDOZA", OfficeId = 32, ConceptoId = 39 },
                new OfficeConcepto { OfficeName = "PEROTE", OfficeId = 33, ConceptoId = 52 },
                new OfficeConcepto { OfficeName = "PIEDRAS NEGRAS", OfficeId = 34, ConceptoId = 53 },
                new OfficeConcepto { OfficeName = "TEZONAPA", OfficeId = 35, ConceptoId = 27 },
                new OfficeConcepto { OfficeName = "PLAYA VICENTE", OfficeId = 36, ConceptoId = 21 },
                new OfficeConcepto { OfficeName = "SANTIAGO TUXTLA", OfficeId = 37, ConceptoId = 16 },
                new OfficeConcepto { OfficeName = "XICO", OfficeId = 38, ConceptoId = 32 },
                new OfficeConcepto { OfficeName = "PASO DEL MACHO", OfficeId = 39, ConceptoId = 51 },
                new OfficeConcepto { OfficeName = "NOGALES", OfficeId = 40, ConceptoId = 28 },
                new OfficeConcepto { OfficeName = "TEMPOAL", OfficeId = 41, ConceptoId = 12 },
                new OfficeConcepto { OfficeName = "PLATON SANCHEZ", OfficeId = 42, ConceptoId = 55 },
                new OfficeConcepto { OfficeName = "EL HIGO", OfficeId = 43, ConceptoId = 43 },
                new OfficeConcepto { OfficeName = "MISANTLA", OfficeId = 44, ConceptoId = 50 },
                new OfficeConcepto { OfficeName = "GUTIERREZ ZAMORA", OfficeId = 45, ConceptoId = 6 },
                new OfficeConcepto { OfficeName = "TRES VALLES", OfficeId = 46, ConceptoId = 63 },
                new OfficeConcepto { OfficeName = "VILLA AZUETA", OfficeId = 47, ConceptoId = 64 },
                new OfficeConcepto { OfficeName = "JUAN RODRIGUEZ CLARA", OfficeId = 48, ConceptoId = 48 },
                new OfficeConcepto { OfficeName = "JUAN DIAZ COVARRUBIAS", OfficeId = 49, ConceptoId = 47 },
                new OfficeConcepto { OfficeName = "LAS VIGAS", OfficeId = 50, ConceptoId = 49 },
                new OfficeConcepto { OfficeName = "VEGA DE ALATORRE", OfficeId = 51, ConceptoId = 5 },
                new OfficeConcepto { OfficeName = "EMILIO CARRANZA", OfficeId = 52, ConceptoId = 44 },
                new OfficeConcepto { OfficeName = "JALACINGO", OfficeId = 53, ConceptoId = 31 },
                new OfficeConcepto { OfficeName = "TLAPACOYAN", OfficeId = 54, ConceptoId = 61 },
                new OfficeConcepto { OfficeName = "TOMATLAN", OfficeId = 55, ConceptoId = 62 },
                new OfficeConcepto { OfficeName = "IXHUATLAN DEL CAFE", OfficeId = 56, ConceptoId = 45 },
                new OfficeConcepto { OfficeName = "CHICHICAXTLE", OfficeId = 57, ConceptoId = 41 },
                new OfficeConcepto { OfficeName = "CD. ALEMAN", OfficeId = 58, ConceptoId = 37 },
                new OfficeConcepto { OfficeName = "OTATITLAN", OfficeId = 59, ConceptoId = 13 },
                new OfficeConcepto { OfficeName = "CARLOS A. CARRILLO", OfficeId = 60, ConceptoId = 16 },
                new OfficeConcepto { OfficeName = "SAYULA DE ALEMAN", OfficeId = 61, ConceptoId = 57 },
                new OfficeConcepto { OfficeName = "COATZACOALCOS", OfficeId = 62, ConceptoId = null },
                new OfficeConcepto { OfficeName = "TLALTETELA", OfficeId = 63, ConceptoId = 33 },
                new OfficeConcepto { OfficeName = "CHACALTIANGUIS", OfficeId = 64, ConceptoId = 40 },
                new OfficeConcepto { OfficeName = "PEÑUELA", OfficeId = 65, ConceptoId = 25 },
                new OfficeConcepto { OfficeName = "JESUS CARRANZA", OfficeId = 66, ConceptoId = 23 },
                new OfficeConcepto { OfficeName = "IXHUATLAN DEL SURESTE", OfficeId = 67, ConceptoId = 46 },
                new OfficeConcepto { OfficeName = "CHINAMECA", OfficeId = 68, ConceptoId = 42 },
                new OfficeConcepto { OfficeName = "SALTABARRANCA", OfficeId = 69, ConceptoId = 56 },
                new OfficeConcepto { OfficeName = "CAZONES/TUXPAN", OfficeId = 70, ConceptoId = null },
                new OfficeConcepto { OfficeName = "TEPETZINTLA", OfficeId = 71, ConceptoId = 59 },
                new OfficeConcepto { OfficeName = "CHICONTEPEC", OfficeId = 72, ConceptoId = 9 },
                new OfficeConcepto { OfficeName = "EMILIO CARRANZA/PALMA SOLA", OfficeId = 73, ConceptoId = null }
            };
        }

        public int GetConcept(int officeId, int defaultConcept = 1){

            var concept = this.ConceptosOficinas.FirstOrDefault(item => item.OfficeId == officeId);
            if(concept == null){
                return defaultConcept;
            }

            return concept.ConceptoId ?? defaultConcept;

        }

    }

    public class OfficeConcepto {
        public string OfficeName { get; set; }
        public int OfficeId { get; set; }
        public int? ConceptoId { get; set; }
    }
}

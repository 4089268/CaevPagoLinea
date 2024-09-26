using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Services
{
    public class ArquosService {

        private readonly string _connectionString;

        public ArquosService(IConfiguration configuration){
            this._connectionString = configuration.GetConnectionString("Arquos")!;
        }


        public ICollection<PadronRecord> GetPadron(){

            List<PadronRecord> records = new();

            string query = @"
                SELECT vp.id_padron,
                    vp.id_cuenta,
                    vp.razon_social,
                    vp._localizacion,
                    vp.subtotal,
                    vp.iva,
                    vp.total,
                    vp._mesFacturado,
                    vp.direccion,
                    p.id_localidad,
                    _poblacion
                FROM Padron.vw_Cat_Padron vp
                inner join padron.Cat_Padron p on p.id_padron=vp.id_padron";

            try {
                using (SqlConnection connection = new SqlConnection(_connectionString)) {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = (int)TimeSpan.FromMinutes(12).TotalSeconds;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            records.Add( PadronRecord.FromDataReader(reader) );
                        }
                    }
                }
            }
            catch (SqlException ex) {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
            }

            return records;
        }

    }

}
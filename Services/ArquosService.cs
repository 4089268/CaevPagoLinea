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
                isnull(vp.subtotal,0) as subtotal,
                isnull(vp.iva,0) as iva,
                isnull(vp.total,0) as total,
                vp._mesFacturado,
                vp.direccion,
                p.id_localidad,
                isnull(p.sector,0) as sector,
                vp._poblacion
            FROM Padron.vw_Cat_Padron vp
             Inner join padron.Cat_Padron p on p.id_padron=vp.id_padron
             Where isnull(vp.id_padron,0)>0 and isnull(vp.id_cuenta,0)>0";

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
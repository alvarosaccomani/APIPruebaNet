using APIPruebaNet.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIPruebaNet.Services
{
    public class EquipoService
    {
        private readonly string _connectionString;

        public EquipoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Equipo>> GetEquiposAsync()
        {
            var equipos = new List<Equipo>();

            using (var connection = new NpgsqlConnection(_connectionString)) {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("SELECT eq_id, eq_nombre, eq_precio FROM eq_equipos", connection))
                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        equipos.Add(new Equipo {
                            eq_Id = reader.GetInt32(0),
                            eq_Nombre = reader.GetString(1),
                            eq_Precio = reader.GetDouble(2)
                        });
                    }
                }
            }
            return equipos;
        }

        public async Task<Equipo> GetEquipoAsync(int eq_Id)
        {
            Equipo equipo = null;

            using (var connection = new NpgsqlConnection(_connectionString)) 
            {
                await connection.OpenAsync();
                using(var command = new NpgsqlCommand("SELECT eq_id, eq_nombre, eq_precio FROM eq_equipos WHERE eq_id = @eq_id", connection))
                {
                    command.Parameters.AddWithValue("eq_Id", eq_Id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            equipo = new Equipo
                            {
                                eq_Id = reader.GetInt32(0),
                                eq_Nombre = reader.GetString(1),
                                eq_Precio = reader.GetDouble(2)
                            };
                        }
                    }
                }
            }
            return equipo;
        }
    }
}

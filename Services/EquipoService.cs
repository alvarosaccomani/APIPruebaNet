using APIPruebaNet.Models;
using Npgsql;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
                            eq_Precio = reader.GetDouble(2),
                            eq_Documentacion = reader.GetString(3)
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
                using(var command = new NpgsqlCommand("SELECT eq_id, eq_nombre, eq_precio, eq_Documentacion, eq_Contenido FROM eq_equipos WHERE eq_id = @eq_id", connection))
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
                                eq_Precio = reader.GetDouble(2),
                                eq_Documentacion = reader.GetString(3),
                                eq_Contenido = reader["eq_Contenido"] as byte[]
                            };
                        }
                    }
                }
            }
            return equipo;
        }

        public async Task AddEquipoAsync(Equipo equipo)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("INSERT INTO eq_equipos (eq_nombre, eq_precio, eq_documentacion, eq_contenido) VALUES (@eq_nombre, @eq_precio, @eq_documentacion, @eq_contenido)", connection))
                {
                    command.Parameters.AddWithValue("eq_nombre", equipo.eq_Nombre);
                    command.Parameters.AddWithValue("eq_precio", equipo.eq_Precio);
                    command.Parameters.AddWithValue("eq_documentacion", equipo.eq_Documentacion);
                    command.Parameters.AddWithValue("eq_contenido", equipo.eq_Contenido);
                    await command.ExecuteNonQueryAsync();
                }

            }
        }

        public async Task ActualizarEquipoAsync(int eq_Id, Equipo equipo)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("UPDATE eq_equipos SET eq_nombre = @eq_nombre, eq_precio = @eq_precio, eq_documentacion = @eq_documentacion, eq_contenido = @eq_contenido WHERE eq_id = @eq_id", connection))
                {
                    command.Parameters.AddWithValue("eq_Id", eq_Id);
                    command.Parameters.AddWithValue("eq_Nombre", equipo.eq_Nombre);
                    command.Parameters.AddWithValue("eq_Precio", equipo.eq_Precio);
                    command.Parameters.AddWithValue("eq_documentacion", equipo.eq_Documentacion);
                    command.Parameters.AddWithValue("eq_contenido", equipo.eq_Contenido);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task EliminaEquipoAsync(int eq_Id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("DELETE FROM eq_equipos WHERE eq_id = @eq_id", connection))
                {
                    command.Parameters.AddWithValue("eq_id", eq_Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}

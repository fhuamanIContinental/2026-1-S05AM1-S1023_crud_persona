using CrudPersona.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace CrudPersona.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaAdoController : ControllerBase
    {
        private readonly MySqlConnection _connection;

        /// <summary>
        /// al siguiente metodo se le denomina constructor
        /// </summary>
        /// <param name="connection"></param>
        public PersonaAdoController(MySqlConnection connection)
        {
            _connection = connection;
        }

        // GET: api/personas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var personas = new List<PersonaModel>();

            var query = """
            SELECT 
                id,
                tipo_documento,
                fecha_nacimiento,
                numero_documento,
                nombres,
                apellido_paterno,
                apellido_materno,
                direcion,
                celular
            FROM persona
        """;

            await _connection.OpenAsync();
            using var cmd = new MySqlCommand(query, _connection);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                personas.Add(new PersonaModel
                {
                    Id = reader.GetInt32("id"),
                    TipoDocumento = reader.GetString("tipo_documento"),
                    FechaNacimiento = reader.GetDateTime("fecha_nacimiento"),
                    NumeroDocumento = reader.GetString("numero_documento"),
                    Nombres = reader.GetString("nombres"),
                    ApellidoPaterno = reader.GetString("apellido_paterno"),
                    ApellidoMaterno = reader.GetString("apellido_materno"),
                    Direccion = reader.GetString("direcion"),
                    Celular = reader.GetString("celular")
                });
            }

            await _connection.CloseAsync();
            return Ok(personas);
        }

        // GET: api/personas/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = "SELECT * FROM persona WHERE id = @id";

            await _connection.OpenAsync();
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                await _connection.CloseAsync();
                return NotFound();
            }

            var persona = new PersonaModel
            {
                Id = reader.GetInt32("id"),
                TipoDocumento = reader.GetString("tipo_documento"),
                FechaNacimiento = reader.GetDateTime("fecha_nacimiento"),
                NumeroDocumento = reader.GetString("numero_documento"),
                Nombres = reader.GetString("nombres"),
                ApellidoPaterno = reader.GetString("apellido_paterno"),
                ApellidoMaterno = reader.GetString("apellido_materno"),
                Direccion = reader.GetString("direcion"),
                Celular = reader.GetString("celular")
            };

            await _connection.CloseAsync();
            return Ok(persona);
        }

        // POST: api/personas
        [HttpPost]
        public async Task<IActionResult> Create(PersonaModel persona)
        {
            var query = """
            INSERT INTO persona
            (tipo_documento, fecha_nacimiento, numero_documento, nombres,
             apellido_paterno, apellido_materno, direcion, celular)
            VALUES
            (@tipo, @fecha, @numero, @nombres, @apPat, @apMat, @dir, @cel)
        """;

            await _connection.OpenAsync();
            using var cmd = new MySqlCommand(query, _connection);

            cmd.Parameters.AddWithValue("@tipo", persona.TipoDocumento);
            cmd.Parameters.AddWithValue("@fecha", persona.FechaNacimiento);
            cmd.Parameters.AddWithValue("@numero", persona.NumeroDocumento);
            cmd.Parameters.AddWithValue("@nombres", persona.Nombres);
            cmd.Parameters.AddWithValue("@apPat", persona.ApellidoPaterno);
            cmd.Parameters.AddWithValue("@apMat", persona.ApellidoMaterno);
            cmd.Parameters.AddWithValue("@dir", persona.Direccion);
            cmd.Parameters.AddWithValue("@cel", persona.Celular);

            await cmd.ExecuteNonQueryAsync();
            await _connection.CloseAsync();

            return Ok("Persona creada");
        }

        // PUT: api/personas/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PersonaModel persona)
        {
            var query = """
            UPDATE persona SET
                tipo_documento = @tipo,
                fecha_nacimiento = @fecha,
                numero_documento = @numero,
                nombres = @nombres,
                apellido_paterno = @apPat,
                apellido_materno = @apMat,
                direcion = @dir,
                celular = @cel
            WHERE id = @id
        """;

            await _connection.OpenAsync();
            using var cmd = new MySqlCommand(query, _connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@tipo", persona.TipoDocumento);
            cmd.Parameters.AddWithValue("@fecha", persona.FechaNacimiento);
            cmd.Parameters.AddWithValue("@numero", persona.NumeroDocumento);
            cmd.Parameters.AddWithValue("@nombres", persona.Nombres);
            cmd.Parameters.AddWithValue("@apPat", persona.ApellidoPaterno);
            cmd.Parameters.AddWithValue("@apMat", persona.ApellidoMaterno);
            cmd.Parameters.AddWithValue("@dir", persona.Direccion);
            cmd.Parameters.AddWithValue("@cel", persona.Celular);

            var rows = await cmd.ExecuteNonQueryAsync();
            await _connection.CloseAsync();

            if (rows == 0)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/personas/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var query = "DELETE FROM persona WHERE id = @id";

            await _connection.OpenAsync();
            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            await _connection.CloseAsync();

            if (rows == 0)
                return NotFound();

            return NoContent();
        }



    }
}

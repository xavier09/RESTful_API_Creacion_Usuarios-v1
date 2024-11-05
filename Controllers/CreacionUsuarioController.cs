using RESTful_API_Creacion_Usuarios.Models;
using RESTful_API_Creacion_Usuarios.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.ComponentModel.Design;
using System.Text.Json;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;


namespace RESTful_API_Creacion_Usuarios.Controllers
{

    [ApiController]
    [Route("usuario")]
    public class CreacionUsuarioController : ControllerBase
    {

        public IConfiguration _configuration;
        string connectionString = "Data Source=AsusRogAlly\\SQLEXPRESS;Initial Catalog=usuarios_api; Integrated Security = True;";

        public CreacionUsuarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public dynamic Login([FromBody] Usuario usurio)
        {

            // var data = JsonConvert.DeserializeObject<dynamic>(obj.ToString());
            string email = usurio.email;
            string pass = usurio.pass;
            string UUID, userEmail;
            List<System.String> user = new List<System.String>();
            bool existe = false;
            string query = @"SELECT UUID, EMAIL FROM dbo.USUARIOS WHERE EMAIL = @Email and PASSWORD_FIELD = @pass";

            try
            {

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@Email", SqlDbType.VarChar);
                    command.Parameters["@Email"].Value = email;
                    command.Parameters.Add("@pass", SqlDbType.VarChar);
                    command.Parameters["@pass"].Value = pass;
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        UUID = reader[0].ToString();
                        userEmail = reader[1].ToString();

                    }
                    else
                    {
                        return new
                        {
                            mensaje = "email o clave incorrecta."
                        };
                    }
                    //strValue=myreader["email"].ToString();
                    //strValue=myreader.GetString(0);

                    connection.Close();
                }


                var generateToken = generaJwtToken(userEmail);

                string query_update = @"UPDATE dbo.USUARIOS SET LAST_LOGIN = @date, JWT_TOKEN = @token, MODIFIED = @date  WHERE EMAIL = @Id";

                using (var connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query_update, connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@Id", SqlDbType.VarChar);
                        command.Parameters["@Id"].Value = userEmail;
                        command.Parameters.Add("@date", SqlDbType.DateTime);
                        command.Parameters["@date"].Value = DateTime.Now;
                        command.Parameters.Add("@token", SqlDbType.VarChar);
                        command.Parameters["@token"].Value = generateToken;

                        int rowsAffected = command.ExecuteNonQuery();

                    }

                    connection.Close();
                }

                return generateToken;


            }
            catch (Exception ex)
            {

                return new
                {
                    mensaje = ex.Message
                };
            }
        }

        [HttpPost]
        [Route("Creacion")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UsuarioDto> crearUsuario([FromBody] UsuarioDto usuario)

        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var tieneNumeros = new Regex(@"[0-9]+");
            //var tieneMayuscula = new Regex(@"[A-Z]+");
            var tiene7caracteres = new Regex(@".{7,}");



            try
            {

                bool email_isValid = Regex.IsMatch(usuario.email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                bool password_isValid = tieneNumeros.IsMatch(usuario.password) && tiene7caracteres.IsMatch(usuario.password);

         


                if (!email_isValid)
                {
                    ModelState.AddModelError("mensaje", "Formato de correo invalido.");

                    return BadRequest(ModelState);
                }



                if (!password_isValid)
                {
                    ModelState.AddModelError("mensaje", "Formato de clave invalido. Debe tener por lo menos un numero y un minimo de 7 caracteres");

                    return BadRequest(ModelState);
                }

                if (EmailExiste(usuario.email))
                {
                    ModelState.AddModelError("mensaje", "El correo ya esta registrado");

                    return BadRequest(ModelState);
                }


                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO dbo.USUARIOS ([NAME_FIELD],[EMAIL],[PASSWORD_FIELD] ,[NUMBER] ,[CITY_CODE],[COUNTRY_CODE],[JWT_TOKEN]) " +
                        "values (@name,@email,@password,@number,@cityCode,@countryCode,@JwtToken)";


                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.Add("@name", SqlDbType.VarChar);
                    cmd.Parameters["@name"].Value = usuario.name;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar);
                    cmd.Parameters["@email"].Value = usuario.email;
                    cmd.Parameters.Add("@password", SqlDbType.VarChar);
                    cmd.Parameters["@password"].Value = usuario.password;
                    cmd.Parameters.Add("@number", SqlDbType.VarChar);
                    cmd.Parameters["@number"].Value = usuario.Phones[0].mumber;
                    cmd.Parameters.Add("@cityCode", SqlDbType.VarChar);
                    cmd.Parameters["@cityCode"].Value = usuario.Phones[0].cityCode;
                    cmd.Parameters.Add("@countryCode", SqlDbType.VarChar);
                    cmd.Parameters["@countryCode"].Value = usuario.Phones[0].countryCode;
                    cmd.Parameters.Add("@JwtToken", SqlDbType.VarChar);
                    cmd.Parameters["@JwtToken"].Value = generaJwtToken(usuario.email);

                    cmd.ExecuteNonQuery();
                    connection.Close();

                }

                return Ok(usuarioRegistrado(usuario.email));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("mensaje", ex.Message);

                return BadRequest(ModelState);
            }


        }

        string HashString(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash);
        }

        bool validarToken(string token, string email)
        {

            bool existe = false;

            string query = @"SELECT Count(*) FROM dbo.USUARIOS WHERE EMAIL = @Email and JWT_TOKEN = @token";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@Email", SqlDbType.VarChar);
                command.Parameters["@Email"].Value = email;
                command.Parameters.Add("@token", SqlDbType.VarChar);
                command.Parameters["@token"].Value = token;
                int count = (Int32)command.ExecuteScalar();


                if (count > 0)
                    existe = true;

            }

            return existe;
        }

        bool EmailExiste(string email)
        {

            bool existe = false;

            string query = @"SELECT Count(*) FROM dbo.USUARIOS WHERE EMAIL = @Email";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@Email", SqlDbType.VarChar);
                command.Parameters["@Email"].Value = email;
                int count = (Int32)command.ExecuteScalar();


                if (count > 0)
                    existe = true;
                connection.Close();

            }

            return existe;
        }

        string generaJwtToken(string userEmail)
        {

            //crear la informacion del usuario para token
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Email,userEmail)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //crear detalle del token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

        string usuarioRegistrado(string email)
        {

            bool existe = false;
            var jsonResult = new StringBuilder();

            string query = @"SELECT [UUID],[CREATED],[MODIFIED],[LAST_LOGIN],[ISACTIVE],[JWT_TOKEN] FROM dbo.USUARIOS WHERE EMAIL = @Email FOR JSON PATH";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@Email", SqlDbType.VarChar);
                command.Parameters["@Email"].Value = email;
                var reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    jsonResult.Append("[]");
                }
                else
                {
                    while (reader.Read())
                    {
                        jsonResult.Append(reader.GetValue(0).ToString());
                    }
                }
                connection.Close();

            }

            return jsonResult.ToString();
        }





    }

}


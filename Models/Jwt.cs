using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RESTful_API_Creacion_Usuarios.Models
{
    public class Jwt
    {

        public string key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }



    }

}

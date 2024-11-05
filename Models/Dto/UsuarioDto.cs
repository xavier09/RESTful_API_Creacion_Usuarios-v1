

using System.ComponentModel;
using System.Xml.Linq;

namespace RESTful_API_Creacion_Usuarios.Models.Dto
{
    public class UsuarioDto
    {

        [DefaultValue("Juan Rodriguez")]

        public string name { get; set; }

        [DefaultValue("juan@rodriguez.org")]
        public string email { get; set; }

        [DefaultValue("hunter2")]
        public string password { get; set; } = "hunter2";
  
        public List<Phone> Phones { get; set; }
    }

    public class Phone
    {
        [DefaultValue(1234567)]
        public int mumber { get; set; } = 1234567;

        [DefaultValue(1)]
        public int cityCode { get; set; } = 1;

        [DefaultValue(57)]
        public int countryCode { get; set; } = 57;
    }



}

using System.ComponentModel;

namespace RESTful_API_Creacion_Usuarios.Models
{

    public class Usuario
    {
        [DefaultValue("admin@admin.org")]
        public string email { get; set; }

        [DefaultValue("123")]
        public string pass { get; set; }

        //  public List<Phone> Phones { get; set; }
    }

    //public class Phone
    //{
    //    public int Number { get; set; } = 1234567;
    //    public int CityCode { get; set; } = 1;
    //    public int CountryCode { get; set; } = 57;
    //}


}

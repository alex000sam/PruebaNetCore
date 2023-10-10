using System.ComponentModel.DataAnnotations;

namespace PruebaNetCore.Modelos.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(maximumLength: 150)]
        public string Nombre { get; set; } = null!;
        [StringLength(maximumLength: 1)]
        public string Genero { get; set; } = null!;
        public int Edad { get; set; }
        [StringLength(maximumLength: 300)]
        public string Direccion { get; set; } = null!;
        [StringLength(maximumLength: 10)]
        public string Telefono { get; set; } = null!;
        //[StringLength(maximumLength: 16)]
        //public string Contraseña { get; set; } = null!;
        public bool Estado { get; set; }
    }
}

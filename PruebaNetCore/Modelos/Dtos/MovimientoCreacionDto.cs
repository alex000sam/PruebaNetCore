using System.ComponentModel.DataAnnotations;

namespace PruebaNetCore.Modelos.Dtos
{
    public class MovimientoCreacionDto
    {
        //public DateTime Fecha { get; set; }
        [StringLength(maximumLength: 20)]
        public string TipoMovimiento { get; set; } = null!;
        public decimal Valor { get; set; }
    }
}

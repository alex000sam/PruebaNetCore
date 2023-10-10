using System.ComponentModel.DataAnnotations;

namespace PruebaNetCore.Modelos.Dtos
{
    public class MovimientoDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        [StringLength(maximumLength: 20)]
        public string TipoMovimiento { get; set; } = null!;
        public decimal Valor { get; set; }
    }
}

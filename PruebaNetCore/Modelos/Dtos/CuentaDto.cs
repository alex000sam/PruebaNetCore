using System.ComponentModel.DataAnnotations;

namespace PruebaNetCore.Modelos.Dtos
{
    public class CuentaDto
    {
        public int NumeroCuenta { get; set; }
        [StringLength(maximumLength: 20)]
        public string TipoCuenta { get; set; } = null!;
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
    }
}

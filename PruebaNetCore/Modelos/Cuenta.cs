namespace PruebaNetCore.Modelos
{
    public class Cuenta
    {
        public int NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; } = null!;
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;
        public List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
    }
}

namespace PruebaNetCore.Modelos
{
    public class Cliente : Persona
    {
        public string Contraseña { get; set; } = null!;
        public bool Estado { get; set; }
        public List<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
    }
}

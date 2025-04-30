namespace CuentasBancariasLafise.Entidades
{
    public class TransaccionEN
    {
        public string Codigo { get; set; }
        public string Fecha { get; set; }
        public string Tipo { get; set; }
        public string Cuenta { get; set; }
        public decimal Monto { get; set; }
        public decimal SaldoAntes { get; set; }
        public decimal SaldoNuevo { get; set; }
    }
}

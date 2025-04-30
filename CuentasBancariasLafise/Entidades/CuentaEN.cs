namespace CuentasBancariasLafise.Entidades
{
    public class CuentaEN
    {
        public int IdCuenta { get; set; }
        public string Numero { get; set; }
        public decimal Saldo { get; set; }
        public int Estado { get; set; }
        public int Cliente { get; set; }

    }
}

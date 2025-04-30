namespace CuentasBancariasLafise.Utils
{
    public class Response
    {
        public int codigo {  get; set; }
        public object data { get; set; }
        public string mensaje { get; set; }

        public void SetAdvertencia(string mensaje)
        {
            this.codigo = 0;
            this.mensaje = mensaje;
        }

        public void SetExito(object datos, string mensaje = "")
        {
            this.codigo = 1;
            this.data = datos;
            this.mensaje = mensaje;
        }

        public void SetError(string error, string mensaje = "")
        {
            mensaje = string.IsNullOrEmpty(mensaje) ? "Error al realizar operación, contacte a soporte" : mensaje;
            this.codigo = 2;
        }
    }
}

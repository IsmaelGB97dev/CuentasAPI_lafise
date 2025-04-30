using CuentasBancariasLafise.DataAccess;
using CuentasBancariasLafise.Entidades;
using CuentasBancariasLafise.Entidades.DTOS;
using CuentasBancariasLafise.Utils;

namespace CuentasBancariasLafise.LogicaNegocio
{
    public static class ClienteLN
    {
        public static Response CrearPerfilCliente(ClienteDTO cliente)
        {
			Response res = new Response();
			try
			{
				if (cliente == null)
					res.SetAdvertencia("Campos vacios");
				else if (string.IsNullOrEmpty(cliente.Nombre))
					res.SetAdvertencia("Nombre del cliente es requerido");
				else if (string.IsNullOrEmpty(cliente.FechaNacimiento))
					res.SetAdvertencia("Fecha de nacimiento es requerida");
				else if (string.IsNullOrEmpty(cliente.Sexo))
					res.SetAdvertencia("Campo Sexo es requerido");
				else if (string.IsNullOrEmpty(cliente.Ingresos.ToString()))
					res.SetAdvertencia("Ingresos son requeridos");
				else if (cliente.Ingresos <= 0)
					res.SetAdvertencia("Ingresos no pueden ser menores o iguales a cero");
				else
				{
					res = ClienteDA.CrearPerfilCliente(cliente);
				}
			}
			catch (Exception ex)
			{
				res.SetError(ex.ToString());
			}
			return res;
        }
    }
}

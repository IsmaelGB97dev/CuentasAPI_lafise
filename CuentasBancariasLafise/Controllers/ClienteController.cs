using CuentasBancariasLafise.Entidades.DTOS;
using CuentasBancariasLafise.LogicaNegocio;
using CuentasBancariasLafise.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuentasBancariasLafise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpPost]
        [Route("CrearPerfil")] 
        public IActionResult CrearPerfil([FromBody] ClienteDTO cliente)
        {
            Response res = new Response();
            try
            {
                res = ClienteLN.CrearPerfilCliente(cliente);
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
        }
    }
}

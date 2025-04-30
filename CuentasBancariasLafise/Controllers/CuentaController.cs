using CuentasBancariasLafise.Entidades.DTOS;
using CuentasBancariasLafise.LogicaNegocio;
using CuentasBancariasLafise.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuentasBancariasLafise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        [HttpGet]
        [Route("HistorialTransacciones")]
        public IActionResult HistorialTransacciones([FromQuery] string NumeroCuenta)
        {
            Response res = new Response();
            try
            {
                res = CuentaLN.HistorialTransacciones(NumeroCuenta);
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
        }

        [HttpPost]
        [Route("Crear")]
        public IActionResult CrearCuenta([FromBody] CuentaDTO cuenta)
        {
            Response res = new Response();
            try
            {
                res = CuentaLN.CrearCuenta(cuenta);
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
        }

        [HttpPost]
        [Route("AplicarTransaccion")]
        public IActionResult RealizarTransaccion([FromBody] TransaccionDTO transaccion)
        {
            Response res = new Response();
            try
            {
                res = CuentaLN.RealizarTransaccion(transaccion);
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
        }

        [HttpGet]
        [Route("ConsultarSaldo")]
        public IActionResult ConsultarSaldo([FromQuery] string NumeroCuenta)
        {
            Response res = new Response();
            try
            {
                res = CuentaLN.ConsultarSaldo(NumeroCuenta);
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

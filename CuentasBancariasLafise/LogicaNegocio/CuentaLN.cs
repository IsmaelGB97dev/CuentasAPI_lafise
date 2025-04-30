using CuentasBancariasLafise.DataAccess;
using CuentasBancariasLafise.Entidades;
using CuentasBancariasLafise.Entidades.DTOS;
using CuentasBancariasLafise.Utils;

namespace CuentasBancariasLafise.LogicaNegocio
{
    public static class CuentaLN
    {
        public static Response RealizarTransaccion(TransaccionDTO trans)
        {
            Response res = new Response();
            try
            {
                // Validaciones
                if (trans.Tipo != "Deposito" && trans.Tipo != "Retiro")
                    res.SetAdvertencia("Tipo de transaccion no valida, puede ser: Retiro o Deposito");
                else if (trans.Monto <= 0)
                    res.SetAdvertencia("Monto debe ser mayor a cero");
                else if (CuentaDA.ObtenerCuenta(0, trans.NumeroCuenta).data == null)
                    res.SetAdvertencia("La cuenta indicada no existe");
                else
                {
                    // Obtener saldos
                    decimal saldo = CuentaDA.ObtenerSaldo(trans.NumeroCuenta);
                    if (saldo == -1)
                        res.SetAdvertencia("Error al obtener saldos");
                    else
                    {
                        decimal saldoNuevo = 0;
                        if (trans.Tipo == "Deposito")
                        {
                            saldoNuevo = saldo + trans.Monto;
                            if (CuentaDA.ActualizarSaldo(trans.NumeroCuenta, saldoNuevo).codigo == 1)
                                res = CuentaDA.RealizarTransaccion(trans, saldo, saldoNuevo);
                            else
                                res.SetAdvertencia("Error al actualizar saldos");
                        }
                        else
                        {
                            if (saldo < trans.Monto)
                                res.SetAdvertencia("Fondos insuficientes");
                            else
                            {
                                saldoNuevo = saldo - trans.Monto;
                                if (CuentaDA.ActualizarSaldo(trans.NumeroCuenta, saldoNuevo).codigo == 1)
                                    res = CuentaDA.RealizarTransaccion(trans, saldo, saldoNuevo);
                                else
                                    res.SetAdvertencia("Error al actualizar saldos");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            return res;
        }

        public static Response HistorialTransacciones(string cuenta)
        {
            Response res = new Response();
            try
            {
                Response cuentaRes = CuentaDA.ObtenerCuenta(0, cuenta);
                if (cuentaRes.data == null)
                    res.SetAdvertencia("Cuenta indicada no existe");
                else
                {
                    res = CuentaDA.HistorialTransacciones(cuenta);
                }
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            return res;
        }

        public static Response ConsultarSaldo(string cuenta)
        {
            Response res = new Response();
            try
            {
                Response cuentaRes = CuentaDA.ObtenerCuenta(0, cuenta);
                if (cuentaRes.data == null)
                    res.SetAdvertencia("Cuenta indicada no existe");
                else
                {
                    decimal saldo = CuentaDA.ObtenerSaldo(cuenta);
                    if (saldo != -1)
                        res.data = new { Saldo = saldo };
                    else
                        res.SetAdvertencia("Error al obtener saldo");
                }
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            return res;
        }

        public static Response CrearCuenta(CuentaDTO cuenta)
        {
            Response res = new Response();
            try
            {
                if (cuenta == null)
                    res.SetAdvertencia("Campos vacios");
                else if (string.IsNullOrEmpty(cuenta.Cliente.ToString()))
                {
                    res.SetAdvertencia("Cliente es requerido");
                }
                else if (ClienteDA.ObtenerCliente(cuenta.Cliente).data == null)
                    res.SetAdvertencia("Cliente indicado no existe");
                else if (string.IsNullOrEmpty(cuenta.SaldoInicial.ToString()))
                {
                    res.SetAdvertencia("Saldo inicial es requerido");
                    if (cuenta.SaldoInicial <= 0)
                        res.SetAdvertencia("Saldo inicial no puede ser menor igual a cero");
                }
                else
                    res = CuentaDA.CrearCuenta(cuenta);
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            return res;
        }
    }
}

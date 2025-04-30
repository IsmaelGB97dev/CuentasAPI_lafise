using CuentasBancariasLafise.Entidades;
using CuentasBancariasLafise.Entidades.DTOS;
using CuentasBancariasLafise.LogicaNegocio;
using CuentasBancariasLafise.Utils;
using System.Data.SQLite;

namespace CuentasBancariasLafise.DataAccess
{
    public static class CuentaDA
    {
        private static readonly string _cadena = DA.GetCadenaConn();

        private static string GenerarNumeroCuenta()
        {
            string timestamp = DateTime.UtcNow.Ticks.ToString();
            string unique10 = timestamp.Substring(timestamp.Length - 10);
            return long.Parse(unique10).ToString();
        }

        public static decimal ObtenerSaldo(string numeroCuenta)
        {
            decimal response = -1;
            Response cuentaRes = CuentaDA.ObtenerCuenta(0, numeroCuenta);
            if (cuentaRes.codigo == 1)
            {
                if (cuentaRes.data != null)
                    response = ((CuentaEN)cuentaRes.data).Saldo;
            }
            return response;
        }

        public static Response ActualizarSaldo(string cuenta, decimal nuevoSaldo)
        {
            Response res = new Response();
            using var conn = new SQLiteConnection(_cadena);

            try
            {
                conn.Open();
                using var query = new SQLiteCommand("UPDATE Cuenta SET Saldo = @saldo WHERE Numero = @numero", conn);
                query.Parameters.AddWithValue("@saldo", nuevoSaldo);
                query.Parameters.AddWithValue("@numero", cuenta);

                long newID = query.ExecuteNonQuery();

                if (newID > 0)
                    res = ObtenerCuenta(0, cuenta);
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static Response HistorialTransacciones(string numeroCuenta)
        {
            Response res = new Response();
            List<TransaccionEN> listaTrn = new List<TransaccionEN>();
            using var conn = new SQLiteConnection(_cadena);
            try
            {
                conn.Open();
                using var query = new SQLiteCommand("SELECT * FROM Transaccion WHERE Cuenta = @cuenta", conn);
                query.Parameters.AddWithValue("@cuenta", numeroCuenta);

                using var reader = query.ExecuteReader();

                while (reader.Read())
                {
                    TransaccionEN trn = new TransaccionEN();
                    trn.Codigo = reader.GetString(1);
                    trn.Fecha = reader.GetString(2);
                    trn.Tipo = reader.GetString(3);
                    trn.Cuenta = reader.GetString(4);
                    trn.Monto = reader.GetDecimal(5);
                    trn.SaldoAntes = reader.GetDecimal(6);
                    trn.SaldoNuevo = reader.GetDecimal(7);

                    listaTrn.Add(trn);
                }
                if (listaTrn.Count == 0)
                    res.SetAdvertencia("No hay movimientos");
                else
                    res.SetExito(listaTrn);
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static Response ObtenerTransaccion(long IdTransaccion)
        {
            Response res = new Response();
            TransaccionEN trn = new TransaccionEN();
            using var conn = new SQLiteConnection(_cadena);
            try
            {
                conn.Open();
                using var query = new SQLiteCommand("SELECT * FROM Transaccion WHERE Id = @id", conn);
                query.Parameters.AddWithValue("@id", IdTransaccion);

                using var reader = query.ExecuteReader();

                while (reader.Read())
                {
                    trn.Codigo = reader.GetString(1);
                    trn.Fecha = reader.GetString(2);
                    trn.Tipo = reader.GetString(3);
                    trn.Cuenta = reader.GetString(4);
                    trn.Monto = reader.GetDecimal(5);
                    trn.SaldoAntes = reader.GetDecimal(6);
                    trn.SaldoNuevo = reader.GetDecimal(7);
                }
                if (trn.Codigo == "")
                    res.SetAdvertencia("Transaccion no existe");
                else
                    res.SetExito(trn);
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static Response RealizarTransaccion(TransaccionDTO trans, decimal saldoAntes, decimal saldoNuevo)
        {
            Response res = new Response();
            TransaccionEN cl = new TransaccionEN();
            using var conn = new SQLiteConnection(_cadena);
            try
            {
                conn.Open();
                using var query = new SQLiteCommand("INSERT INTO Transaccion(Codigo, Fecha, Tipo, Cuenta, Monto, SaldoAntes, SaldoNuevo)" +
                    "Values(@codigo, @fecha, @tipo, @cuenta, @monto, @saldoantes, @saldonuevo)", conn);
                query.Parameters.AddWithValue("@codigo", Guid.NewGuid().ToString());
                query.Parameters.AddWithValue("@fecha", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                query.Parameters.AddWithValue("@tipo", trans.Tipo);
                query.Parameters.AddWithValue("@cuenta", trans.NumeroCuenta);
                query.Parameters.AddWithValue("@monto", trans.Monto);
                query.Parameters.AddWithValue("@saldoantes", saldoAntes);
                query.Parameters.AddWithValue("@saldonuevo", saldoNuevo);
                query.ExecuteNonQuery();

                long newID = conn.LastInsertRowId;

                if (newID > 0)
                    res = ObtenerTransaccion(newID);
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return res;
        }


        public static Response ObtenerCuenta(long IdCuenta = 0, string numero = "")
        {
            Response res = new Response();
            CuentaEN cl = new CuentaEN();
            using var conn = new SQLiteConnection(_cadena);
            try
            {
                conn.Open();
                using var query = new SQLiteCommand("SELECT * FROM Cuenta WHERE " + (numero == "" ? "Id": "Numero") + "= @id", conn);
                query.Parameters.AddWithValue("@id", (numero == "" ? IdCuenta : numero));

                using var reader = query.ExecuteReader();

                while (reader.Read())
                {
                    cl.IdCuenta = reader.GetInt32(0);
                    cl.Numero = reader.GetString(1);
                    cl.Saldo = reader.GetDecimal(2);
                    cl.Estado = reader.GetInt32(3);
                    cl.Cliente = reader.GetInt32(4);
                }
                if (cl.IdCuenta == 0)
                    res.SetAdvertencia("Cuenta no existe");
                else
                    res.SetExito(cl);
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static Response CrearCuenta(CuentaDTO cuenta)
        {
            Response res = new Response();
            using var conn = new SQLiteConnection(_cadena);

            try
            {
                conn.Open();
                using var query = new SQLiteCommand("INSERT INTO Cuenta(Numero, Saldo, Estado, Cliente) VALUES(@numero, @saldo, @estado, @cliente)", conn);
                query.Parameters.AddWithValue("@numero", GenerarNumeroCuenta());
                query.Parameters.AddWithValue("@saldo", cuenta.SaldoInicial);
                query.Parameters.AddWithValue("@estado", 1);
                query.Parameters.AddWithValue("@cliente", cuenta.Cliente);
                query.ExecuteNonQuery();

                long newID = conn.LastInsertRowId;

                if (newID > 0)
                    res = ObtenerCuenta(newID);
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return res;
        }
    }
}

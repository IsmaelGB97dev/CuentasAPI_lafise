using CuentasBancariasLafise.Entidades;
using CuentasBancariasLafise.Entidades.DTOS;
using CuentasBancariasLafise.Utils;
using System.Data.SQLite;

namespace CuentasBancariasLafise.DataAccess
{
    public static class ClienteDA
    {
        private static readonly string _cadena = DA.GetCadenaConn();


        public static Response ObtenerCliente(long IdCliente)
        {
            Response res = new Response();
            ClienteEn cl = new ClienteEn(); 
            using var conn = new SQLiteConnection(_cadena);

            try
            {
                conn.Open();
                using var query = new SQLiteCommand("SELECT * FROM Cliente WHERE Id = @idCliente", conn);
                query.Parameters.AddWithValue("@idCliente", IdCliente);

                using var reader = query.ExecuteReader();

                while(reader.Read())
                {
                    cl.IdCliente = reader.GetInt32(0);
                    cl.Nombre = reader.GetString(1);
                    cl.FechaNacimiento = reader.GetString(2);
                    cl.Sexo = reader.GetString(3);
                    cl.Ingresos = reader.GetDecimal(4);
                }
                if (cl.IdCliente == 0)
                    res.SetAdvertencia("Cliente no existe");
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



        public static Response CrearPerfilCliente(ClienteDTO cliente)
        {
            Response res = new Response();
            using var conn = new SQLiteConnection(_cadena);

            try
            {
                conn.Open();
                using var query = new SQLiteCommand("INSERT INTO Cliente(Nombre, FechaNacimiento, Sexo, Ingresos) VALUES(@nombre, @fechaNac, @sexo, @ingresos)", conn);
                query.Parameters.AddWithValue("@nombre", cliente.Nombre);
                query.Parameters.AddWithValue("@fechaNac", cliente.FechaNacimiento);
                query.Parameters.AddWithValue("@sexo", cliente.Sexo);
                query.Parameters.AddWithValue("@ingresos", cliente.Ingresos);
                query.ExecuteNonQuery();

                long newID = conn.LastInsertRowId;

                if (newID > 0)
                    res = ObtenerCliente(newID);
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

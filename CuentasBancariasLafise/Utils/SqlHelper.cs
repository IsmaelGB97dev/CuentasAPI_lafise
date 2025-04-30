using CuentasBancariasLafise.DataAccess;
using System.Data.SQLite;

namespace CuentasBancariasLafise.Utils
{
    public static class SqlHelper
    {
        private static readonly string _cadena = DA.GetCadenaConn();

        public static void InicializarBD()
        {
            var projectRoot = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
            string sqlPath = Path.Combine(projectRoot, "CuentasLafise.db");

            if (!File.Exists(sqlPath))
            {
                SQLiteConnection.CreateFile(sqlPath);

                using (var conn = new SQLiteConnection(_cadena))
                {
                    conn.Open();

                    string createCliente = @"
                        CREATE TABLE [Cliente] (
                          [Id] INTEGER PRIMARY KEY AUTOINCREMENT
                        , [Nombre] text NOT NULL
                        , [FechaNacimiento] text NOT NULL
                        , [Sexo] text NOT NULL
                        , [Ingresos] numeric(53,0) NOT NULL
                        );
                    ";

                    string createCuenta = @"
                        CREATE TABLE [Cuenta] (
                          [Id] INTEGER PRIMARY KEY AUTOINCREMENT
                        , [Numero] text NOT NULL
                        , [Saldo] numeric(53,0) DEFAULT (0) NOT NULL
                        , [Estado] bigint DEFAULT (1) NOT NULL
                        , [Cliente] bigint NOT NULL
                        );
                    ";

                    string createTrans = @"
                       CREATE TABLE [Transaccion] (
                          [Id] INTEGER PRIMARY KEY AUTOINCREMENT
                        , [Codigo] text NOT NULL
                        , [Fecha] text NOT NULL
                        , [Tipo] text NOT NULL
                        , [Cuenta] text NOT NULL
                        , [Monto] numeric(53,0) NOT NULL
                        , [SaldoAntes] numeric(53,0) NOT NULL
                        , [SaldoNuevo] numeric(53,0) NOT NULL
                        );  
                    ";

                    using (var command = new SQLiteCommand(conn))
                    {
                        command.CommandText = createCuenta;
                        command.ExecuteNonQuery();

                        command.CommandText = createCliente;
                        command.ExecuteNonQuery();


                        command.CommandText = createTrans;
                        command.ExecuteNonQuery();
                    }
                }
            }

        }
    }
}

namespace CuentasBancariasLafise.DataAccess
{
    public static class DA
    {
        public static string GetCadenaConn(string name = "DefaultConnection")
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // importante si estás en consola o pruebas
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration.GetConnectionString(name);
        }
    }
}

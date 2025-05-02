using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTestAPI
{
    public class Tests
    {
        private WebApplicationFactory<Program> _factory = null!;
        private HttpClient _cliente = null!;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _cliente = _factory.CreateClient();
        }

        [TearDown]
        public void Cleanup()
        {
            _cliente.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task RegistrarCliente()
        {
            var nuevoCliente = new
            {
                Nombre = "Maria Lopez Jarquin",
                FechaNacimiento = "20/03/1998",
                Sexo = "Femenino",
                Ingresos = 3000
            };
            var response = await _cliente.PostAsJsonAsync("/api/Cliente/CrearPerfil", nuevoCliente);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var json = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine("Respuesta de la api: " + json);

            Assert.IsTrue(json.Contains(nuevoCliente.Nombre));
        }

        [Test]
        public async Task CreacionCuenta()
        {
            var request = new
            {
                Cliente = 1,
                SaldoInicial = 3000
            };
            var response = await _cliente.PostAsJsonAsync("/api/Cuenta/Crear", request);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var json = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine("Respuesta de la api: " + json);

            Assert.IsTrue(json.Contains(request.SaldoInicial.ToString()));
        } 
        
        [Test]
        public async Task RealizarDeposito()
        {
            var transaccion = new
            {
                Tipo = "Deposito",
                NumeroCuenta = "6720780607",
                Monto = 200
            };
            var response = await _cliente.PostAsJsonAsync("/api/Cuenta/AplicarTransaccion", transaccion);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var json = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine("Respuesta de la api: " + json);

            Assert.IsTrue(json.Contains(transaccion.NumeroCuenta));
        }

        [Test]
        public async Task RealizarRetiro()
        {
            var transaccion = new
            {
                Tipo = "Retiro",
                NumeroCuenta = "6720780607",
                Monto = 100
            };
            var response = await _cliente.PostAsJsonAsync("/api/Cuenta/AplicarTransaccion", transaccion);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var json = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine("Respuesta de la api: " + json);

            Assert.IsTrue(json.Contains(transaccion.NumeroCuenta));
        }

        [Test]
        public async Task VerHistorial()
        {
            var request = new
            {
                NumeroCuenta = "6720780607"
            };
            var response = await _cliente.GetAsync("/api/Cuenta/HistorialTransacciones?NumeroCuenta=" + request.NumeroCuenta);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var json = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine("Respuesta de la api: " + json);

            Assert.IsTrue(json.Contains("saldoAntes"));
        }
    }
}
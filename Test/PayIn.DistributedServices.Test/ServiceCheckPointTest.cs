using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PayIn.DistributedServices.Test.Helpers;
using System;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test
{
	[TestClass]
	public class ServiceCheckPointTest : BaseTest
	{
		[TestMethod]
		public void Crud()
		{
			Server.LoginWebAsync("operator@pay-in.es", "Pa$$w0rd")
				.WaitWithResult<string>();

			// Consultar listado
			{
				var result = Server.GetAsync("Api/ServiceCheckPoint", new { })
					.WaitWithResult<JObject>();
				Assert.IsNotNull(result["data"]);
			}

			// Crear
			// Consultar

			// Modificar
			// Consultar

			// Eliminar
			// Consultar
		}
	}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PayIn.DistributedServices.Test.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test
{
	[TestClass]
	public class ControlItemTest : BaseTest
	{
		[TestMethod]
		public void Crud()
		{
			Server.LoginWebAsync("operator@pay-in.es", "Pa$$w0rd")
				.WaitWithResult<string>();

			int ServiceConcessionId = 0;
			int ItemId = 0;

			#region Consultar listado
			{
				var result = Server.GetAsync("Api/ControlItem")
					.WaitWithResult<JObject>();
				Assert.IsNotNull(result["data"]);
			}
			#endregion Consultar listado

			#region Consultar Concession
			{
				var result = Server.GetAsync("Api/ServiceConcession", new {
					filter = "Control Logistica"
				})
					.WaitWithResult<JObject>();
				var array = result["data"] as JArray;
				Assert.IsNotNull(array);
				Assert.AreEqual(array.Count(), 1);

				var instance = array.FirstOrDefault() as JObject;
				ServiceConcessionId = Convert.ToInt32(instance["id"]);
				Assert.AreNotEqual(ServiceConcessionId, 0);
			}
			#endregion Consultar Concession

			#region Crear
			if (ItemId == 0)
			{
				var result = Server.PostAsync("Api/ControlItem", new
				{
					name = "Item de pruebas",
					observations = "Observaciones de pruebas",
					concessionId = ServiceConcessionId,
					saveTrack = true,
					saveFacialRecognition = true,
					checkTimetable = true
				})
					.WaitWithResult<JObject>();
				ItemId = Convert.ToInt32(result["id"]);
				Assert.AreNotEqual(ItemId, 0);
			}
			#endregion Crear

			#region Comprobar Crear
			{
				var result = Server.GetAsync("Api/ControlItem", ItemId)
					.WaitWithResult<JObject>();

				var array = result["data"] as JArray;
				Assert.IsNotNull(array);
				Assert.AreEqual(array.Count(), 1);

				var instance = array.FirstOrDefault() as JObject;
				Assert.AreEqual(instance["id"], ItemId);
				Assert.AreEqual(instance["name"], "Item de pruebas");
				Assert.AreEqual(instance["observations"], "Observaciones de pruebas");
				Assert.AreEqual(instance["saveTrack"], true);
				Assert.AreEqual(instance["saveFacialRecognition"], true);
				Assert.AreEqual(instance["checkTimetable"], true);
			}
			#endregion Comprobar Crear

			#region Modificar
			{
				var result = Server.PutAsync("Api/ControlItem", ItemId, new
				{
					name = "Item de pruebas2",
					observations = "Observaciones de pruebas2",
					saveTrack = false,
					saveFacialRecognition = false,
					checkTimetable = false
				})
					.WaitWithResult<JObject>();
				ItemId = Convert.ToInt32(result["id"]);
				Assert.AreNotEqual(ItemId, 0);
			}
			#endregion Crear

			#region Comprobar Modificar
			{
				var result = Server.GetAsync("Api/ControlItem", ItemId)
					.WaitWithResult<JObject>();

				var array = result["data"] as JArray;
				Assert.IsNotNull(array);
				Assert.AreEqual(array.Count(), 1);

				var instance = array.FirstOrDefault() as JObject;
				Assert.AreEqual(instance["id"], ItemId);
				Assert.AreEqual(instance["name"], "Item de pruebas2");
				Assert.AreEqual(instance["observations"], "Observaciones de pruebas2");
				Assert.AreEqual(instance["saveTrack"], false);
				Assert.AreEqual(instance["saveFacialRecognition"], false);
				Assert.AreEqual(instance["checkTimetable"], false);
			}
			#endregion Comprobar Modificar

			#region Eliminar
			{
				Server.DeleteAsync("Api/ControlItem", ItemId)
					.Wait();
			}
			#endregion Eliminar

			#region Comprobar Eliminar
			{
				var result = Server.GetAsync("Api/ControlItem", ItemId)
					.WaitWithResult<JObject>();

				var array = result["data"] as JArray;
				Assert.IsNotNull(array);
				Assert.AreEqual(array.Count(), 0);
			}
			#endregion Comprobar Eliminar
		}
	}
}

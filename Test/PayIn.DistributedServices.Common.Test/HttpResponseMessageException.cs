using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test.Helpers
{
	public static class HttpResponseMessageException
	{
		#region ThrowException
		public static async Task<HttpResponseMessage> ThrowExceptionAsync(this HttpResponseMessage that)
		{
			if (!that.IsSuccessStatusCode)
			{
				var json = await that.Content.ReadAsStringAsync();
				try
				{
					var result = json.FromJson<JObject>();
					throw new ApplicationException(result["message"].Value<string>());
				}
				catch (ApplicationException) {
					throw;
				}
				catch { }
				throw new Exception(json);
			}
			return that;
		}
		#endregion ThrowException
	}
}

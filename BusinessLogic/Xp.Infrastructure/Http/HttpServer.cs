using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Infrastructure.Http
{
	public class HttpServer
	{
		public enum HttpServerEncoding
		{
			UrlEncoding,
			Json
		}

		#region BaseAddress
		public string BaseAddress { get; set; }
		#endregion BaseAddress

		#region Token
		public string Token { get; set; }
		#endregion Token

		#region Constructors
		public HttpServer(string baseAddress, string token)
		{
			BaseAddress = baseAddress;
			Token = token;
		}
		#endregion Constructors

		#region LoginWebAsync
		public async Task<string> LoginWebAsync(string user, string password)
		{
			var uri = new Uri(new Uri(BaseAddress), "token");
			var request = string.Format("grant_type=password&username={0}&password={1}&client_id=PayInWebApp", user, password);
			using (var client = new HttpClient())
			{
				var content = new StringContent(request, Encoding.UTF8);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

				var response = (await client.PostAsync(uri, content))
					.ThrowException();

				var json = (await response.Content.ReadAsStringAsync());
				var result = json
					.FromJson<JObject>();

				Token = result["access_token"].Value<string>();
				return Token;
			}
		}
		#endregion LoginWebAsync

		#region LoginTpvAsync
		public async Task<string> LoginTpvAsync(string user, string password)
		{
			var uri = new Uri(new Uri(BaseAddress), "token");
			var request = string.Format("grant_type=password&username={0}&password={1}&client_id=PayInTpv&client_secret=PayInTpv@1238", user, password);
			using (var client = new HttpClient())
			{
				var content = new StringContent(request, Encoding.UTF8);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

				var response = (await client.PostAsync(uri, content))
					.ThrowException();

				var json = (await response.Content.ReadAsStringAsync());
				var result = json
					.FromJson<JObject>();

				Token = result["access_token"].Value<string>();
				return Token;
			}
		}
		#endregion LoginTpvAsync

		#region ConnectSignalR
		public async Task ConnectSignalR<Type>(string name, Action<Type> action)
		{
			var uri = new Uri(new Uri(BaseAddress), "signalr");
			using (var connection = new HubConnection(uri.AbsoluteUri))
			{
				connection.Headers.Add("Authorization", "bearer " + Token);

				var proxy = connection.CreateHubProxy("PushSignalRHub");
				proxy.On<Type>(name, item => action(item));
				await connection.Start();
			}
		}
		#endregion ConnectSignalR

		#region GetAsync
		public async Task<TResult> GetAsync<TResult>(string url, int id, object arguments = null)
		{
			using (var client = new HttpClient())
			{
				var argumentUris = arguments.ToUrlEncoding();
				var uri = new Uri(BaseAddress + url + "/" + id +
					"?t=" + DateTime.Now.Ticks +
					(!argumentUris.IsNullOrEmpty() ? "&" + argumentUris : ""));

                if (!Token.IsNullOrEmpty())
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

				var response = (await client.GetAsync(uri))
					.ThrowException();
				var json = await response.Content.ReadAsStringAsync();

				var result = json
					.FromJson<TResult>();
				return result;
			}
		}
		public async Task<TResult> GetAsync<TResult>(string url, object arguments = null)
        {
            var argumentUris = arguments.ToUrlEncoding();
            string url2 = BaseAddress + url +
                    "?t=" + DateTime.Now.Ticks +
                    (!argumentUris.IsNullOrEmpty() ? "&" + argumentUris : "");

            var responseText = "";
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(url2);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                    var response = (await client.GetAsync(uri));
                    responseText = await response.Content.ReadAsStringAsync();

                    response.ThrowException();

                    var result = responseText
                        .FromJson<TResult>();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception("GET " + url2 + "\nToken: " + Token + "\nError: " + e.Message + "\nResponse: " + responseText + "\nStack: " + e.StackTrace);
            }
		}
        public async Task<JObject> GetAsync(string url, object arguments = null)
        {
            return await GetAsync<JObject>(url, arguments);
        }
        public async Task<JObject> GetAsync(string url, int id, object arguments = null)
        {
            return await GetAsync<JObject>(url, id,arguments);
        }
		#endregion GetAsync

		#region PostAsync
		public async Task<TResult> PostAsync<TResult>(string url, int? id = null, object arguments = null, HttpServerEncoding encoding = HttpServerEncoding.Json)
			where TResult : new()
		{
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			//ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => { return true; });

			using (var client = new HttpClient())
			{
				StringContent content;
				if (encoding == HttpServerEncoding.Json)
				{
					content = new StringContent(arguments.ToJson(), Encoding.UTF8);
					content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				}
				else
				{
					content = new StringContent(arguments.ToUrlEncoding(), Encoding.UTF8);
					content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
				}

				if (!Token.IsNullOrEmpty())
				    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

				var uri = id != null ?
					new Uri(BaseAddress + url + "/" + id) :
					new Uri(BaseAddress + url);
				var response = (await client.PostAsync(uri, content))
					.ThrowException();

                var resultString = await response.Content.ReadAsStringAsync();
				var result = resultString.FromJson<TResult>();
				return result;
			}
		}
		public async Task<JObject> PostAsync(string url, int? id = null, object arguments = null)
		{
			return await PostAsync<JObject>(url, id: id, arguments: arguments);
		}
		#endregion PostAsync

		#region PutAsync
		public async Task<JObject> PutAsync(string url, int? id = null, object arguments = null, HttpServerEncoding encoding = HttpServerEncoding.Json)
		{
			using (var client = new HttpClient())
			{
				StringContent content;
				if (encoding == HttpServerEncoding.Json)
				{
					content = new StringContent(arguments.ToJson(), Encoding.UTF8);
					content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				}
				else
				{
					content = new StringContent(arguments.ToUrlEncoding(), Encoding.UTF8);
					content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
				}

                if (!Token.IsNullOrEmpty())
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

				var uri = id != null ? 
					new Uri(BaseAddress + url + "/" + id) :
					new Uri(BaseAddress + url);
				var response = (await client.PutAsync(uri, content))
					.ThrowException();

				var result = (await response.Content.ReadAsStringAsync())
					.FromJson<JObject>();
				return result;
			}
		}
		#endregion PutAsync

		#region DeleteAsync
		public async Task DeleteAsync(string url, int id)
		{
			using (var client = new HttpClient())
			{
				var uri = new Uri(BaseAddress + url + "/" + id);

                if (!Token.IsNullOrEmpty())
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

				(await client.DeleteAsync(uri))
					.ThrowException();
			}
		}
		#endregion DeleteAsync
	}
}

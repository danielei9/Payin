using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test.Helpers
{
	[Synchronization]
	public class HttpServer : IDisposable
	{
#if HOMO
		protected static string BaseAddress = "http://payin-homo.cloudapp.net";
#elif TEST
		protected static string BaseAddress = "http://payin-test.cloudapp.net";
#elif DEBUG
		protected static string BaseAddress = "http://localhost:8080";
#else
		protected static string BaseAddress = "http://payin.cloudapp.net";
#endif

		private static string AccessToken = null;
		private static string RefreshToken = null;

		#region User
		private string _User;
		public string User
		{
			get
			{
				return _User;
			}
			set
			{
				_User = value;
				AccessToken = null;
				RefreshToken = null;
			}
		}
		#endregion User

		#region Password
		private string _Password;
		public string Password
		{
			get
			{
				return _Password;
			}
			set
			{
				_Password = value;
				AccessToken = null;
				RefreshToken = null;
			}
		}
		#endregion Password

		#region ClientId
		private string _ClientId;
		public string ClientId
		{
			get
			{
				return _ClientId;
			}
			set
			{
				_ClientId = value;
				AccessToken = null;
				RefreshToken = null;
			}
		}
		#endregion ClientId

		#region ClientSecret
		private string _ClientSecret;
		public string ClientSecret
		{
			get
			{
				return _ClientSecret;
			}
			set
			{
				_ClientSecret = value;
				AccessToken = null;
				RefreshToken = null;
			}
		}
		#endregion ClientSecret

		#region Constructors
		public HttpServer()
		{
		}
		public HttpServer(string user, string password, string clientId, string clientSecret)
		{
			User = user;
			Password = password;
			ClientId = clientId;
			ClientSecret = clientSecret;
		}
		#endregion Constructors

		#region Dispose
		public void Dispose()
		{
		}
		#endregion Dispose

		#region ArgumentsToUrl
		private string ArgumentsToUrl(object query)
		{
			if (query == null)
				return "";

			var arguments = new StringBuilder();
			foreach (var property in query.GetType().GetProperties())
			{
				var value = property.GetValue(query);
				if (value != null)
				{
					var text = "";
					var isEnum = property.PropertyType.IsEnum;

					// Convert to communicate
					if (isEnum)
						text = ((int)value).ToString();
					else
						text = value.ToString();

					if (text != "")
					{
						if (arguments.Length > 0)
							arguments.Append("&" + property.Name + "=" + text);
						else
							arguments.Append(property.Name + "=" + text);
					}
				}
			}

			return arguments.ToString();
		}
		#endregion ArgumentsToUrl

		#region GetTokenAsync
		public async Task<string> GetTokenAsync()
		{
			if (AccessToken.IsNullOrEmpty())
			{
				using (var client = new HttpClient())
				{
					// Uri
					client.BaseAddress = new Uri(BaseAddress + "/token");

					// Headers
					var headers = new List<KeyValuePair<string, string>>()
					{
						new KeyValuePair<string, string>("grant_type", "password"),
						new KeyValuePair<string, string>("username", User),
						new KeyValuePair<string, string>("password",  Password),
						new KeyValuePair<string, string>("client_id", ClientId)
					};
					if (!ClientSecret.IsNullOrEmpty())
						headers.Add(new KeyValuePair<string, string>("client_secret", ClientSecret));

					// Body
					var content = new FormUrlEncodedContent(headers);
					var message = await client.PostAsync("token", content);

					// Execution
					var response = await message.ThrowExceptionAsync();

					// Response
					var json = await response.Content.ReadAsStringAsync();
					var result = json
						.FromJson<JObject>();
					AccessToken = result["access_token"].Value<string>();
					RefreshToken = result["refresh_token"].Value<string>();
				};
			}

			return AccessToken;
		}
		#endregion GetTokenAsync

		#region RefreshTokenAsync
		private async Task<string> RefreshTokenAsync()
		{
			using (var client = new HttpClient())
			{
				// Uri
				client.BaseAddress = new Uri(BaseAddress + "/token");

				// Headers
				var content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("grant_type", "refresh_token"),
					new KeyValuePair<string, string>("refresh_token", RefreshToken),
					new KeyValuePair<string, string>("client_id", ClientId),
					new KeyValuePair<string, string>("client_secret", ClientSecret)
				});
				
				// Body
				var message = await client.PostAsync("token", content);

				// Response
				var response = await message.ThrowExceptionAsync();
				var json = await response.Content.ReadAsStringAsync();

				// Response
				var result = json
					.FromJson<JObject>();
				AccessToken = result["access_token"].Value<string>();
				RefreshToken = result["refresh_token"].Value<string>();
			};

			return AccessToken;
		}
		#endregion RefreshTokenAsync

		#region ExecuteAsync
		private async Task<T> ExecuteAsync<T>(Func<Task<HttpResponseMessage>> call, bool repited = false)
		{
			// Ejecutar
			var message = await call();
			var json = await message.Content.ReadAsStringAsync();

			if (!message.IsSuccessStatusCode)
			{
				// Llamada recursiva
				if (!repited && (message.StatusCode == HttpStatusCode.Unauthorized))
				{
					await RefreshTokenAsync();
					return await ExecuteAsync<T>(call, true);
				}

				// Devolver error
				try
				{
					var result = json.FromJson<JObject>();

					var response = result["message"].Value<string>();
					var values = result["modelState"]
						?.Cast<JProperty>()
						?.SelectMany(x =>
							x.Value
								.Select(y =>
									x.Name + ": " + y.ToString()
								)
						)
						?.JoinString("\n");
					throw new ApplicationException(
						response +
						(
							values.IsNullOrEmpty() ?
							"" :
							"\n" + values
						)
					);
				}
				catch (ApplicationException)
				{
					throw;
				}
				catch { }

				throw new Exception(json);
			}
			else
			{
				// Devolver resultado
				var result = json
					.FromJson<T>();
				return result;
			}
		}
		#endregion ExecuteAsync

		#region GetAsync
		public async Task<T> GetAsync<T>(string url, object arguments = null)
		{
			var argumentUris = ArgumentsToUrl(arguments);
			var uri = new Uri(BaseAddress + url +
				"?t=" + DateTime.Now.Ticks + // esto se pone para evitar que los get se cachen
				(!argumentUris.IsNullOrEmpty() ? "&" + argumentUris : ""));
			
			using (var client = new HttpClient())
			{
				return await ExecuteAsync<T>(async() => {
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync());
					return await client.GetAsync(uri);
				});
			}
		}
		public async Task<T> GetAsync<T>(string url, int id, object arguments = null)
		{
			return await GetAsync<T>(url + "/" + id, arguments);
		}
		#endregion GetAsync

		#region PostAsync
		public async Task<JObject> PostAsync(string url, object arguments = null)
		{
			return await PostAsync<JObject>(url, arguments);
		}
		public async Task<JObject> PostAsync(string url, long id, object arguments = null)
		{
			return await PostAsync<JObject>(url + "/" + id, arguments);
		}
		public async Task<T> PostAsync<T>(string url, object arguments = null)
		{
			var uri = new Uri(BaseAddress + url);

			using (var client = new HttpClient())
			{
				var content = new StringContent(arguments.ToJson(), Encoding.UTF8);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				return await ExecuteAsync<T>(async () => {
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync());
					return await client.PostAsync(uri, content);
				});
			}
		}
		public async Task<T> PostAsync<T>(string url, long id, object arguments = null)
		{
			return await PostAsync<T>(url + "/" + id, arguments);
		}
		#endregion PostAsync

		#region PutAsync
		public async Task<JObject> PutAsync(string url, long id, object arguments = null)
		{
			return await PutAsync<JObject>(url, id, arguments);
		}
		public async Task<T> PutAsync<T>(string url, long id, object arguments = null)
		{
			var uri = new Uri(BaseAddress + url + "/" + id);

			using (var client = new HttpClient())
			{
				var content = new StringContent(arguments.ToJson(), Encoding.UTF8);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				return await ExecuteAsync<T>(async () => {
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync());
					return await client.PutAsync(uri, content);
				});
			}
		}
		#endregion PutAsync

		#region DeleteAsync
		public async Task<JObject> DeleteAsync(string url, int id)
		{
			return await DeleteAsync<JObject>(url, id);
		}
		public async Task<T> DeleteAsync<T>(string url, int id)
		{
			var uri = new Uri(BaseAddress + url + "/" + id);

			using (var client = new HttpClient())
			{
				return await ExecuteAsync<T>(async () => {
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync());
					return await client.DeleteAsync(uri);
				});
			}
		}
		#endregion DeleteAsync
	}
}

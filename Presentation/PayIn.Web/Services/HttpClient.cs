using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using PayIn.Common;
using PayIn.Domain.Security;

namespace PayIn.Web.Services
{
    public class HttpClientNetwork
    {
        private static HttpClientNetwork instance = null;

        protected HttpClientNetwork() { }

        public static HttpClientNetwork Instance
        {
            get
            {
                if (instance == null)
                    instance = new HttpClientNetwork();

                return instance;
            }
        }

        public void Login(string user, string pass, out HttpResponseMessage resultCode,out JToken resultJSON)
        {
            resultCode = null;
            resultJSON = "";
            string sBaseUrl = String.Format("{0}://{1}:{2}",
                                    HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Host,
                                    HttpContext.Current.Request.Url.Port);

			using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(sBaseUrl);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", user),
                    new KeyValuePair<string, string>("password",  pass),
                    new KeyValuePair<string, string>("client_id", "PayInWebApp")
                });
                resultCode = client.PostAsync("/token", content).Result;
				var task = resultCode.Content.ReadAsStringAsync();
				task.Wait();
				resultJSON = JObject.Parse(task.Result);
            };
        }
        public void ForgotPassword(string user, out HttpResponseMessage resultCode)
        {
            resultCode = null;
            string sBaseUrl = String.Format("{0}://{1}:{2}",
                                    HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Host,
                                    HttpContext.Current.Request.Url.Port);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(sBaseUrl);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", user)
                });
                resultCode = client.PostAsync("/api/account/ForgotPassword", content).Result;
            };
        }
        public void ConfirmForgotPassword(string user,string code, string pass, string confirmPass,out HttpResponseMessage resultCode)
        {
            resultCode = null;
            string sBaseUrl = String.Format("{0}://{1}:{2}",
                                    HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Host,
                                    HttpContext.Current.Request.Url.Port);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(sBaseUrl);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("userid", user),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("password", pass),
                    new KeyValuePair<string, string>("confirmPassword", confirmPass),
                    new KeyValuePair<string, string>("client_id", "PayInWebApp")
                });
                resultCode = client.PostAsync("/api/account/ConfirmForgotPassword", content).Result;
            };
        }
        public void Register(string email,string user, string birthday,string mobile, string pass, string confirmPass,bool checkTerms,UserType isBussiness, out HttpResponseMessage resultCode,out JToken resultJSON)
        {
            resultCode = null;			
            resultJSON = "";			

            string sBaseUrl = String.Format("{0}://{1}:{2}",
                                    HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Host,
                                    HttpContext.Current.Request.Url.Port);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(sBaseUrl);

			
				var content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("userName", email),
                    new KeyValuePair<string, string>("name", user),
					new KeyValuePair<string, string>("birthday", birthday),
					new KeyValuePair<string, string>("isBussiness",isBussiness.ToString()),
					new KeyValuePair<string, string>("mobile", mobile),
					new KeyValuePair<string, string>("password", pass),
					new KeyValuePair<string, string>("confirmPassword", confirmPass),
					new KeyValuePair<string, string>("acceptTerms", checkTerms.ToString())
				});				  

				
                resultCode = client.PostAsync("/api/account", content).Result;
            };
        }

		public void ConfirmEmail(string user, string code, out HttpResponseMessage resultCode)
		{
			resultCode = null;
			string sBaseUrl = String.Format("{0}://{1}:{2}",
									HttpContext.Current.Request.Url.Scheme,
									HttpContext.Current.Request.Url.Host,
									HttpContext.Current.Request.Url.Port);
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(sBaseUrl);
				var content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("userid", user),
					new KeyValuePair<string, string>("code", code),
					new KeyValuePair<string, string>("client_id", AccountClientId.Web)
				});
				resultCode = client.PutAsync("/api/account/ConfirmEmail", content).Result;
			};
		}

		public void ConfirmEmailAndData(string user, string code, string mobile, string password, out HttpResponseMessage resultCode)
		{
			resultCode = null;
			string sBaseUrl = String.Format("{0}://{1}:{2}",
									HttpContext.Current.Request.Url.Scheme,
									HttpContext.Current.Request.Url.Host,
									HttpContext.Current.Request.Url.Port);
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(sBaseUrl);
				var content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("userid", user),
					new KeyValuePair<string, string>("code", code),
					new KeyValuePair<string, string>("client_id", AccountClientId.Web),
					new KeyValuePair<string, string>("mobile", mobile),
					new KeyValuePair<string, string>("password", password)
				});
				resultCode = client.PutAsync("/api/account/ConfirmEmailAndData", content).Result;
			};
		}

		// , model.name,model.taxName, model.taxNumber , model.taxAddress, model.bankAccountNumber, model.address, model.observations
		public void ConfirmCompanyEmailAndData(string user, string code, string mobile, string login, string password, string name, string taxName, string taxNumber, string taxAddress, string bankAccountNumber, string address, string observations, out HttpResponseMessage resultCode)
		{
			resultCode = null;
			string sBaseUrl = String.Format("{0}://{1}:{2}",
									HttpContext.Current.Request.Url.Scheme,
									HttpContext.Current.Request.Url.Host,
									HttpContext.Current.Request.Url.Port);
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(sBaseUrl);
				var content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("userId", user),
					new KeyValuePair<string, string>("code", code),
					new KeyValuePair<string, string>("client_id", AccountClientId.Web),
					new KeyValuePair<string, string>("mobile", mobile),
					new KeyValuePair<string, string>("password", password),
					new KeyValuePair<string, string>("name", name),
					new KeyValuePair<string, string>("userName", login),
					new KeyValuePair<string, string>("taxNumber", taxNumber),
					new KeyValuePair<string, string>("taxName", taxName),
					new KeyValuePair<string, string>("taxAddress", taxAddress),
					new KeyValuePair<string, string>("bankAccountNumber", bankAccountNumber),
					new KeyValuePair<string, string>("address", address),
					new KeyValuePair<string, string>("observations", observations)
				});
				resultCode = client.PutAsync("/api/account/ConfirmCompanyEmailAndData", content).Result;
			};
		}

	}
}
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xp.Infrastructure.Http;

namespace Xp.Infrastructure.Services
{
    public class MixPanelService : IAnalyticsService
	{
#if PRODUCTION || VILAMARXANT
		private readonly string Token = "873691d1e07a2d6e4dd6cd61ac4bc3d7";
        private readonly string TokenVilamarxant = "b5fba3d2fea0ba84e0b2da147e643c7c";
#elif TEST
		private readonly string Token = "ff13c0850f717ffacab012452e00489f";
        private readonly string TokenVilamarxant = "33525865c43cd6642fc94f6176c3b57a";
#else
        private readonly string TokenVilamarxant = "33525865c43cd6642fc94f6176c3b57a";
#endif // PRODUCTION

        private SessionData SessionData { get; set; }

		#region Constructors
		public MixPanelService(
			SessionData sessionData
		)
		{
			SessionData = sessionData;
		}
		#endregion Constructors

		#region TrackEventAsync
		public async Task TrackEventAsync(string name, Dictionary<string, object> parameters = null)
		{
#if PRODUCTION || TEST
            // Payin
            await TrackEventInternalAsync(name, Token, parameters);
#endif // PRODUCTION || TEST

            // Vilamarxant
            if (
                (
                    (name == "NoticeMobileGet") ||
                    (name == "EventMobileGet")
                ) &&
                (SessionData.ClientId == AccountClientId.AndroidVilamarxantNative)
            )
                await TrackEventInternalAsync(name, TokenVilamarxant, parameters);
        }
        #endregion TrackEventAsync

        #region TrackEventInternalAsync
        public async Task TrackEventInternalAsync(string name, string token, Dictionary<string, object> parameters = null)
        {
			try
			{
				var _parameters = parameters;
				if ((SessionData != null) && (!_parameters.ContainsKey("distinct_id")))
					_parameters.Add("distinct_id", SessionData.Login);
				if (_parameters.ContainsKey("distinct_id"))
					_parameters["distinct_id"] = (_parameters["distinct_id"] as string).ToLower();

				var payload = string.Format(
					"{{"  +
						"\"event\":\"{1}\"," +
						"\"properties\":{{" +
							"\"token\": \"{0}\"" +
							"{2}" +
						"}}" +
					"}}",
					token,
					name,
					_parameters
						.Select(x => ",\"" + x.Key + "\":" + (
							x.Value.GetType() == typeof(string) ? "\"" + x.Value + "\"" :
							x.Value
						))
						.JoinString("")
				);
				payload = payload.ToBase64();

				using (var client = new HttpClient())
				{
					var response = (await client.GetAsync("http://api.mixpanel.com/track?data=" + payload))
						.ThrowException();
					var result = await response.Content.ReadAsStringAsync();
					if (result == "0")
						throw new ApplicationException("Mixpanel server error");
				}
			}
			catch
			{
			}
        }
        #endregion TrackEventInternalAsync

        #region TrackUserAsync
        public async Task TrackUserAsync(string login, string name, string email)
		{
#if PRODUCTION || TEST
			try
			{
				var payload = string.Format(
					"{{" +
						"\"$token\": \"{0}\"," +
						"\"$distinct_id\":\"{1}\"," +
						"\"$set\":{{" +
							"\"$name\":\"{2}\"," +
							"\"$email\":\"{3}\"" +
						"}}" +
					"}}",
					Token,
					login.ToLower(),
					name,
					email.ToLower()
				);
				payload = payload.ToBase64();

				using (var client = new HttpClient())
				{
					var response = (await client.GetAsync("http://api.mixpanel.com/engage?data=" + payload))
						.ThrowException();
					var result = await response.Content.ReadAsStringAsync();
					if (result == "0")
						throw new ApplicationException("Mixpanel server error");
				}
			}
			catch
			{
			}
#else
			await Task.Run(() => { });
#endif // PRODUCTION
		}
		#endregion TrackUserAsync
	}
}

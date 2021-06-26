using System;
using System.Net.Http;

namespace Xp.Infrastructure.Http
{
	public static class HttpResponseMessageException
	{
		#region ThrowException
		public static HttpResponseMessage ThrowException(this HttpResponseMessage that)
		{
			if (!that.IsSuccessStatusCode)
			{
				if (that.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
#if (DEBUG || RELEASE)
					throw new Exception(that.ToString());
#else
				throw new Exception(that.StatusCode + " (" + ((int)that.StatusCode) + ")\n" + that.ReasonPhrase);
#endif
				}
				else
				{
					var task = that.Content.ReadAsStringAsync();
					task.Wait();
					var json = task.Result;
					var obj = json.FromJson();
					var content = obj["message"].Value as string;
					throw new ApplicationException(content);
				}
			}
			return that;
		}
		#endregion ThrowException
	}
}

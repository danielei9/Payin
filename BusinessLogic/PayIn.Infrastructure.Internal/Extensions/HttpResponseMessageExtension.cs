
namespace System.Net.Http
{
	public static class HttpResponseMessageExtension
	{
		#region ThrowException
		public static HttpResponseMessage ThrowException(this HttpResponseMessage that)
		{
			if (!that.IsSuccessStatusCode)
			{
#if (DEBUG || TEST || HOMO)
				throw new Exception(that.ToString());
#else
				throw new Exception(that.StatusCode + " (" + ((int)that.StatusCode) + ")\n" + that.ReasonPhrase);
#endif
			}
			return that;
		}
		#endregion ThrowException
	}
}

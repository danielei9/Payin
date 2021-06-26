
namespace System.Net.Http
{
	public static class HttpResponseMessageExtension
	{
		public static HttpResponseMessage ThrowException(this HttpResponseMessage that)
		{
			if (!that.IsSuccessStatusCode)
			{
#if (DEBUG)
				throw new Exception(that.ToString());
#else
				throw new Exception(that.StatusCode + " (" + ((int)that.StatusCode) + ")\n" + that.ReasonPhrase);
#endif
			}
			return that;
		}
	}
}

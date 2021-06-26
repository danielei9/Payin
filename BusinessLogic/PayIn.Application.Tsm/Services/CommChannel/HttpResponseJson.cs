using System.Text;

namespace PayIn.Application.Tsm.Services.CommChannel
{
	public class HttpResponseJson
	{
		public string Type { get; set; }
		public string TransactionId { get; set; }
		public string Data { get; set; }

		public HttpResponseJson(string type, string transactionId, string data)
		{
			Type = type;
			TransactionId = transactionId;
			Data = data;
		}

		public override string ToString()
		{
			var retVal = new StringBuilder("{\n\t");
			retVal.Append("\"type\": \"" + Type + "\",");
			retVal.Append("\"transactionId\": \"" + TransactionId + "\",");
			retVal.Append("\"card\": \"" + Data + "\"\n}");

			return retVal.ToString();
		}
	}
}

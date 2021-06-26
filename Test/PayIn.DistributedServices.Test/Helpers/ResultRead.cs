using Newtonsoft.Json.Linq;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System.Collections.Generic;

namespace PayIn.DistributedServices.Test.Helpers
{
	public class ResultRead
	{
		public int OperationId { get; set; }
		public IEnumerable<MifareOperationResultArguments> Card { get; set; }
		public IEnumerable<JToken> Charges { get; set; }
		public IEnumerable<JToken> Recharges { get; set; }
		public JObject Fields { get; set; }
	}
}

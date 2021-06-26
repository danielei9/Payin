using Newtonsoft.Json.Linq;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System.Collections.Generic;
using Xp.Application.Results;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.DistributedServices.Test
{
	public interface ITestCard
	{
		byte[] Uid { get; }
		IEnumerable<MifareOperationResultArguments> Execute(JToken scripts);
		IEnumerable<MifareOperationResultArguments> Execute(ScriptResult script);
	}
}

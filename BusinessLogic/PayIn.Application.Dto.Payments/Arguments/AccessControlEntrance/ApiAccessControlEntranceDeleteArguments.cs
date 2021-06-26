using Newtonsoft.Json;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiAccessControlEntranceDeleteArguments : IArgumentsBase
	{
		[JsonIgnore]
		public int Id { set; get; }
	}
}

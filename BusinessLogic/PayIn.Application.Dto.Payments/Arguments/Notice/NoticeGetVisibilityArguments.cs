using Newtonsoft.Json;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class NoticeGetVisibilityArguments : IArgumentsBase
	{
		[JsonIgnore]
		public int Id { get; set; }
	}
}

using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusMobileRequestGetByUserArguments : IArgumentsBase
	{
		public int LineId { get; set; }
	}
}

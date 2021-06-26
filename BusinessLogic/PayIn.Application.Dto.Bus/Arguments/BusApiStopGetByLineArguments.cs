using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusApiStopGetByLineArguments : IArgumentsBase
	{
		public int LineId { get; set; }

		#region Constructors
		public BusApiStopGetByLineArguments(int lineId)
		{
			LineId = lineId;
		}
		#endregion Constructors
	}
}

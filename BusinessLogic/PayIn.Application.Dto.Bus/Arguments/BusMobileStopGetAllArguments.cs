using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusMobileStopGetAllArguments : IArgumentsBase
	{
		public string LineId { get; set; }

		#region Constructors
		public BusMobileStopGetAllArguments(string lineId)
		{
			LineId = lineId;
		}
		#endregion Constructors
	}
}

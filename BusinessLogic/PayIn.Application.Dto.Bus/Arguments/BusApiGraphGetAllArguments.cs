using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusApiGraphGetAllArguments : IArgumentsBase
	{
		public int LineId { get; set; }

		#region Constructors
		public BusApiGraphGetAllArguments(int lineId)
		{
			LineId = lineId;
		}
		#endregion Constructors
	}
}

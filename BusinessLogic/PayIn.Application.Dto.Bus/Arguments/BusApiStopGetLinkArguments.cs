using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusApiStopGetLinkArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public BusApiStopGetLinkArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}

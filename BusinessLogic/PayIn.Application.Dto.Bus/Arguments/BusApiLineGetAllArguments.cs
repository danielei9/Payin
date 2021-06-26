using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusApiLineGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public BusApiLineGetAllArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}

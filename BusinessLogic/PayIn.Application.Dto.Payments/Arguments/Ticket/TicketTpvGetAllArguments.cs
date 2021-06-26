using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class TicketTpvGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public TicketTpvGetAllArguments(string filter) 
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
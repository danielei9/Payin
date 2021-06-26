using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.TicketTemplate
{
	public partial class TicketTemplateGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		

		#region Constructors
		public TicketTemplateGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
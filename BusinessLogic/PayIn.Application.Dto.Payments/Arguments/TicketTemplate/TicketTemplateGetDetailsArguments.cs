using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.TicketTemplate
{
	public class TicketTemplateGetDetailsArguments : IArgumentsBase
	{
		public int Id { get; set; }	

		#region Constructors
		public TicketTemplateGetDetailsArguments(int id)
		{
			Id = id;		
		}
		#endregion Constructors
	}
}

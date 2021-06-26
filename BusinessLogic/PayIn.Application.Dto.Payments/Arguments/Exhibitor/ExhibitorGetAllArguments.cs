using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ExhibitorGetAllArguments : IArgumentsBase
	{
		public int EventId { get; set; }

		#region Constructors
		public ExhibitorGetAllArguments(int eventId, int formId)
		{
            EventId = eventId;
		}
		#endregion Constructors
	}
}

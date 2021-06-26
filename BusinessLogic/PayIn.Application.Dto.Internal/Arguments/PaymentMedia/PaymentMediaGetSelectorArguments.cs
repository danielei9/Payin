using Xp.Application.Dto;

namespace PayIn.Application.Dto.Arguments.PaymentMedia
{
	public partial class PaymentMediaGetSelectorArguments : ArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public PaymentMediaGetSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}

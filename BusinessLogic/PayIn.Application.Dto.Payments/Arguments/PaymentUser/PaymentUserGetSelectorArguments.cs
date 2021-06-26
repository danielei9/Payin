using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.PaymentUser
{
	public partial class PaymentUserGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public PaymentUserGetSelectorArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
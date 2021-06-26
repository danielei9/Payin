using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments

{
	public partial class PaymentConcessionGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public PaymentConcessionGetSelectorArguments(string param)
		{
			Filter = param;
		}
		#endregion Constructors
	}
}

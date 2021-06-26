using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public class PaymentMediaRefundArguments : IArgumentsBase
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings=false)] public string  Pin                  { get; set; }
		                                    public int?    PublicPaymentMediaId { get; set; }
		                                    public int     PublicTicketId       { get; set; }
		                                    public int     PublicPaymentId      { get; set; }
		[Range(0, double.MaxValue)]         public decimal Amount               { get; set; }
		                                    public int     Order                { get; set; }
		                                    public string  CommerceCode         { get; set; }

		#region Constructors
		public PaymentMediaRefundArguments(string commerceCode, string pin, int? publicPaymentMediaId, int publicTicketId, int publicPaymentId, int order, decimal amount)
		{
			CommerceCode = commerceCode;
			Pin = pin;
			PublicPaymentMediaId = publicPaymentMediaId;
			PublicTicketId = publicTicketId;
			PublicPaymentId = publicPaymentId;
			Order = order;
			Amount = amount;
		}
		#endregion Constructors
	}
}

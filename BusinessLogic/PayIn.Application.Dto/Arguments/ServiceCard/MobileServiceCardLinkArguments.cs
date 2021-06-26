using Xp.Common.Dto.Arguments;
using System.ComponentModel.DataAnnotations;

namespace PayIn.Application.Dto.Arguments
{
    public partial class MobileServiceCardLinkArguments : IArgumentsBase
	{
		public string UidText { get; set; }
		public int PaymentConcessionId { get; set; }

		#region Constructors
		public MobileServiceCardLinkArguments(string uidText, int paymentConcessionId)
		{
            UidText = uidText;
			PaymentConcessionId = paymentConcessionId;
		}
		#endregion Constructors
	}
}

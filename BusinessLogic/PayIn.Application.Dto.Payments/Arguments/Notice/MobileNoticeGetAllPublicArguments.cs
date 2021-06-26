using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileNoticeGetAllPublicArguments : IArgumentsBase
	{
		public int? SystemCardId { get; set; }
		public int? PaymentConcessionId { get; set; }
		public LanguageEnum? Language { get; set; } = LanguageEnum.Spanish;

		#region Constructors
		public MobileNoticeGetAllPublicArguments(int? systemCardId, int? paymentConcessionId, LanguageEnum? language)
		{
			SystemCardId = systemCardId;
			PaymentConcessionId = paymentConcessionId;
			Language = language ?? LanguageEnum.Spanish;
		}
		#endregion Constructors
	}
}

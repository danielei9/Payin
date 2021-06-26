using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class MobileNoticeGetEdictsArguments : IArgumentsBase
	{
		public int? SystemCardId { get; set; }
		public int PaymentConcessionId { get; set; }
		public LanguageEnum? Language { get; set; } = LanguageEnum.Spanish;

		#region Constructors
		public MobileNoticeGetEdictsArguments(int? systemCardId, int paymentConcessionId, LanguageEnum? language)
		{
			SystemCardId = systemCardId;
			PaymentConcessionId = paymentConcessionId;
			Language = language ?? LanguageEnum.Spanish;
		}
		#endregion Constructors
	}
}

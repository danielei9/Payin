using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class MobileMainGetAllV4Arguments : IArgumentsBase
    {
        public int? SystemCardId { get; set; }
        public int? PaymentConcessionId { get; set; }
		public LanguageEnum? Language { get; set; } = LanguageEnum.Spanish;

		#region Constructors
		public MobileMainGetAllV4Arguments(int? systemCardId, int? paymentConcessionId, LanguageEnum? language)
        {
            SystemCardId = systemCardId == 0 ? null : systemCardId;
            PaymentConcessionId = paymentConcessionId == 0 ? null : paymentConcessionId;
			Language = language;
		}
        #endregion Constructors
    }
}

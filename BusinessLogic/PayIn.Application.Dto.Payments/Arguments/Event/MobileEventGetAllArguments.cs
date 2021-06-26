using System.ComponentModel.DataAnnotations;
using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileEventGetAllArguments: IArgumentsBase
	{
		public string Filter { get; set; }
		public int? SystemCardId { get; set; }
		public int? PaymentConcessionId { get; set; }
		public LanguageEnum Language { get; set; } = LanguageEnum.Spanish;

		#region Constructors
		public MobileEventGetAllArguments(string filter, int? systemCardId, int? paymentConcessionId, LanguageEnum? language)
		{
			SystemCardId = systemCardId;
			PaymentConcessionId = paymentConcessionId;
			Filter = filter ?? "";
			Language = language ?? LanguageEnum.Spanish;
        }
        #endregion Constructors
    }
}

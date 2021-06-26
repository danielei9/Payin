using Newtonsoft.Json;
using PayIn.Common.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicTicketPayUserArguments : IArgumentsBase
	{
        [JsonIgnore]
		[Required(AllowEmptyStrings = false)] public int Id { get; set; }
		[Required(AllowEmptyStrings = false)] public string Login { get; set; }
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }
		           public string TaxNumber { get; set; }
		[Required(AllowEmptyStrings = false)] public string TaxName { get; set; }
		           public string TaxAddress { get; set; }
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings = false)] public string Pin { get; set; }
		public IEnumerable<PublicTicketPayUserArguments_PaymentMedia> PaymentMedias { get; set; }

		#region Constructors
		public PublicTicketPayUserArguments(string login, string name, string taxNumber, string taxName, string taxAddress, string pin, IEnumerable<PublicTicketPayUserArguments_PaymentMedia> paymentMedias
		)
		{
			Login = login;
			Name = name;
			TaxNumber = taxNumber ?? "";
			TaxName = taxName;
			TaxAddress = taxAddress ?? "";
			PaymentMedias = paymentMedias;
			Pin = pin;
		}
		#endregion Constructors
	}
}

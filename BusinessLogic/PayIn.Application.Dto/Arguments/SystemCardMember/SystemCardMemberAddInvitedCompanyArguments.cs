using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.SystemCardMember
{
	public partial class SystemCardMemberAddInvitedCompanyArguments : IArgumentsBase
	{
		[Display(Name = "Login")]			[Required]							public string Login { get; set; }
		[Display(Name = "Name")]			[Required]							public string Name { get; set; }
		[Display(Name = "resources.security.bankAccountNumber")]				public string BankAccountNumber { get; set; }
		[Display(Name = "resources.security.address")]							public string Address { get; set; }
		[Display(Name = "resources.security.observations")]						public string Observations { get; set; }

		[Display(Name = "Mobile")]			[Required]							public string Mobile { get; set; }
		
		[Display(Name = "resources.security.taxNumber")]						public string TaxNumber { get; set; }
		[Display(Name = "resources.security.taxName")]							public string TaxName { get; set; }
		[Display(Name = "resources.security.taxAddress")]						public string TaxAddress { get; set; }


		#region Constructors
		public SystemCardMemberAddInvitedCompanyArguments(string login, string name, string bankAccountNumber, string address, string observations, string mobile, string taxNumber, string taxName, string taxAddress)
		{
			Login = login;
			Name = name;
			BankAccountNumber = bankAccountNumber;
			Address = address;
			Observations = observations;
			Mobile = mobile;
			TaxNumber = taxNumber;
			TaxName = taxName;
			TaxAddress = taxAddress;
		}
		#endregion Constructors

	}
}

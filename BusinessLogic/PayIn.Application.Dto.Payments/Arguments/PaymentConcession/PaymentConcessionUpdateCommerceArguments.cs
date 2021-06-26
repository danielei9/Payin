using PayIn.Common;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionUpdateCommerceArguments : IArgumentsBase
	{
		                                                                                    public int Id { get; set; }
		// Información fiscal
		[Display(Name = "resources.paymentConcession.taxNumber")]	            [Required]	public string TaxNumber { get; set; }
		[Display(Name = "resources.paymentConcession.taxName")]	                [Required]	public string TaxName { get; set; }
		[Display(Name = "resources.paymentConcession.taxAddress")]	            [Required]	public string TaxAddress { get; set; }
		[RegularExpression(@"^(\s*\w){2}(\s*\d){22}\s*$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "AccountErrorMessage")]
		[Display(Name = "resources.paymentConcession.bankAccountNumber")]	    [Required]	public string BankAccountNumber { get; set; }
		[DataSubType(DataSubType.Image, DataSubType.Pdf)]
		[Display(Name = "resources.paymentConcession.formA")]                               public byte[] FormA { get; set; }
																							public string FormUrl { get; set; }
		// Información comercial
		[Display(Name = "resources.paymentConcession.name")]			         			public string Name { get; set; }
		[Display(Name = "resources.paymentConcession.phone")]				    [Required]	public string Phone { get; set; }
		[Display(Name = "resources.paymentConcession.address")]                 [Required]  public string Address { get; set; }
		[Display(Name = "resources.paymentConcession.observations")]						public string Observations { get; set; }
		// Otros
		[Display(Name = "resources.paymentConcession.payinCommission")]		    [Required]	public decimal PayinCommission { get; set; }
		[Display(Name = "resources.paymentConcession.liquidationAmountMin")]	[Required]	public decimal LiquidationAmountMin { get; set; }
		[Display(Name = "resources.paymentConcession.state")]                               public ConcessionState State { get; set; }

		#region Constructors
		public PaymentConcessionUpdateCommerceArguments(int id, string bankAccountNumber, decimal payinCommission, decimal liquidationAmountMin, string formUrl, string name, string observations, string phone, string address, ConcessionState state, byte[] formA)
		{
			Id = id;
			// Información fiscal
			BankAccountNumber = bankAccountNumber;
			FormA = formA;
			FormUrl = FormUrl;
			// Información comercial
			Name = name;
			Phone = phone;
			Address = address;
			Observations = observations;
			// Otros
			PayinCommission = payinCommission;
			LiquidationAmountMin = liquidationAmountMin;
			State = state;
		}

		#endregion Constructors
	}
}

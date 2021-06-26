using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionCreateArguments : IArgumentsBase
	{
		// Información fiscal
		[Display(Name = "resources.paymentConcession.taxNumber")]         [Required] public string TaxNumber { get; set; }
		[Display(Name = "resources.paymentConcession.taxName")]           [Required] public string TaxName { get; set; }
		[Display(Name = "resources.paymentConcession.taxAddress")]        [Required] public string TaxAddress { get; set; }
		[RegularExpression(@"^(\s*\w){2}(\s*\d){22}\s*$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "AccountErrorMessage")]
		[Display(Name = "resources.paymentConcession.bankAccountNumber")] [Required] public string BankAccountNumber { get; set; }
		[DataSubType(DataSubType.Image, DataSubType.Pdf)]
		[Display(Name = "resources.paymentConcession.formA")]             [Required] public byte[] FormA { get; set; }
		// Información comercial
		[Display(Name = "resources.paymentConcession.name")]              [Required] public string Name { get; set; }
		[Display(Name = "resources.paymentConcession.phone")]             [Required] public string Phone { get; set; }
		[Display(Name = "resources.paymentConcession.address")]           [Required] public string Address { get; set; }
		[Display(Name = "resources.paymentConcession.observations")]                 public string Observations { get; set; }
		
		[DataType(DataType.Password)] [RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Display(Name = "resources.paymentConcession.pin")]               [Required] public string Pin { get; set; }
		[DataType(DataType.Password)] [Compare("Pin", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "ConfirmPin")]
		[Display(Name = "resources.paymentConcession.pinConfirmation")]   [Required] public string PinConfirmation { get; set; }
		
		[Display(Name = "resources.security.acceptTerms")]                [Required] public bool AcceptTerms { get; set; }
        [Display(Name = "resources.paymentConcession.cart")]                           public bool OnlineCartActivated { get; set; }

        #region Constructors
        public PaymentConcessionCreateArguments(string name, string taxName, string taxNumber, 
            string taxAddress, string pin, string pinConfirmation, string observations, string address, 
            string phone, string bankAccountNumber, byte[] formA, bool acceptTerms, bool onlinecartActivated)
        {
			//ServiceSupplier
			TaxNumber              = taxNumber;
			TaxName                = taxName;
			TaxAddress             = taxAddress;
			BankAccountNumber      = bankAccountNumber;
			FormA                  = formA;
			//PaymentConcession
			Name                   = name;
			Phone                  = phone;
			Address                = address;
			Observations           = observations;
            OnlineCartActivated    = onlinecartActivated;
            // Pin
            Pin                    = pin;
			PinConfirmation        = pinConfirmation;
			// Terms               
			AcceptTerms            = acceptTerms;
		}
		#endregion Constructors
	}
}

using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class UserMobileForgotPinArguments : IArgumentsBase
    {
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
        [Required(AllowEmptyStrings = false)]
        public string Pin { get; set; }

        #region Constructors
        public UserMobileForgotPinArguments(string password, string pin)
        {
            Password = password;
            Pin = pin;
        }
        #endregion Constructors
    }
}

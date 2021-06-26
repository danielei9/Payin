using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicEntranceCheckCreateUserQrArguments : IArgumentsBase
	{
		[Required(AllowEmptyStrings=false)] public string Code { get; set; }
		[Required]                          public CheckInType Type { get; set; }
		                                    public string VatNumber { get; set; }
		                                    public string Name { get; set; }
		                                    public string LastName { get; set; }
		                                    public string Email { get; set; }
		                                    public string InvitationCode { get; set; }
		                                    public bool   Force { get; set; }

		#region Constructors
		public PublicEntranceCheckCreateUserQrArguments(string code, CheckInType type, string vatNumber, string name, string lastName, string email, string invitationCode, bool force)
		{
			Code = code;
			Type = type;
			VatNumber = VatNumber ?? "";
			Name = name ?? "";
			LastName = lastName ?? "";
			Email = email ?? "";
			InvitationCode = invitationCode ?? "";
			Force = force;
		}
		#endregion Constructors
	}
}

using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiEntranceInviteArguments : IArgumentsBase
	{
		[Required] public int EntranceTypeId				{ get; set; }

		[Display(Name = "resources.entrance.quantity")]
		[Required] public int Quantity						{ get; set; }

		[Display(Name = "resources.entrance.email")]
		[Required] public string Email						{ get; set; }

		[Display(Name = "resources.entrance.vatNumber")]
		[Required] public string VatNumber					{ get; set; }

		[Display(Name = "resources.entrance.userName")]
		[Required] public string UserName					{ get; set; }

		[Display(Name = "resources.entrance.lastName")]
		[Required] public string LastName					{ get; set; }

		public long Code									{ get; set; }

		#region Constructor
		public ApiEntranceInviteArguments(int quantity, int entranceTypeId, string userName, string email, string vatNumber, long code, string lastName)
		{
			Quantity = quantity;
			EntranceTypeId = entranceTypeId;
			UserName = userName;
			Email = email;
			VatNumber = vatNumber;
			Code = code;
			LastName = lastName;
		}
		#endregion Constructor
	}
}

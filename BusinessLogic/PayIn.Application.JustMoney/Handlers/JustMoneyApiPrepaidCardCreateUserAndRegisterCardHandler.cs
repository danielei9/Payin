using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardCreateUserAndRegisterCardHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardCreateUserAndRegisterCardArguments>
	{
		[Dependency] public JustMoneyApiPrepaidCardCreateUserAndRequestCardHandler JustMoneyApiPrepaidCardCreateHandler { get; set; }
		[Dependency] public JustMoneyApiPrepaidCardRegisterCardCreateHandler JustMoneyApiPrepaidCardRegiserCardCreateHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardCreateUserAndRegisterCardArguments arguments)
		{
			await JustMoneyApiPrepaidCardCreateHandler.RegisterUserAsync(
				arguments.FirstName, arguments.LastName,
				arguments.Alias,
				arguments.BirthDay,
				arguments.Phone, arguments.Mobile,
				arguments.Email, arguments.ConfirmEmail,
				arguments.Password,
				arguments.Address1, arguments.Address2, arguments.City, arguments.ZipCode, arguments.Province, arguments.Country
			);

			await JustMoneyApiPrepaidCardRegiserCardCreateHandler.RegisterCardAsync(
				arguments.CardHolderId,
				arguments.FirstName, arguments.LastName,
				arguments.Alias,
				arguments.BirthDay,
				arguments.Phone, arguments.Mobile,
				arguments.Email,
				arguments.Address1, arguments.Address2, arguments.City, arguments.ZipCode, arguments.Province, arguments.Country
			);

			return null;
		}
		#endregion ExecuteAsync
	}
}

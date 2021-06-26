using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.Domain.JustMoney;
using PayIn.Domain.JustMoney.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	[XpLog("Pfs", "Confirm")]
	public class JustMoneyApiPfsConfirmHandler : IServiceBaseHandler<JustMoneyApiPfsConfirmArguments>
	{
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public IEntityRepository<BankCardTransaction> BankCardRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPfsConfirmArguments arguments)
		{
			var response = arguments.Data.FromXml<R>();
			if (response.R4 != "Successful")
				throw new ArgumentException("Transacción terminada sin éxito", "R4");
			if (response.R10 != "Confirm")
				throw new ArgumentException("Transacción no confirmada", "R10");

			if (response.TKN.IsNullOrEmpty())
				throw new ArgumentException("Transacción sin token", "TKN");

			var item = (await BankCardRepository.GetAsync())
				.Where(x => x.RegistrationMessageId == response.R3)
				.FirstOrDefault();
			if (item == null)
				throw new ApplicationException("MessageId not exists");

			item.State = PrepaidCardState.Active;
			item.Pan = response.TKNCN;
			item.Type = response.TKNCardType;
			item.Token = response.TKN;
			item.ExpirationMM = response.TKNCardExpMM;
			item.ExpirationYYYY = response.TKNCardExpYYYY;

			var isDebit =
				(item.Type == "VSE") || //Visa Electron
				(item.Type == "VSD") || //Visa Debit
				(item.Type == "MSCD") || //MasterCard Debit
				(item.Type == "MAE") || //Maestro
				(item.Type == "MSCB") || //MasterCard Business
				(item.Type == "VSBD") || //Visa Business Debit
				(item.Type == "MCBD") || //MasterCard Business Debit
				(item.Type == "VSPD") || //VISA Prepaid Debit
				(item.Type == "MCPD") || //MasterCard Prepaid Debit
				(item.Type == "MAEP") || //Maestro Prepaid Debit
				(item.Type == "MAEB") || //Maestro Business
				(item.Type == "MSCS") || //MasterCard Signia
				(item.Type == "MCWD") || //MasterCard World Debit
				(item.Type == "MSCP") || //MasterCard Purchasing/ Fleet
				(item.Type == "MSCC") || //MasterCard Corporate / Commercial
				(item.Type == "VPRD") || //Visa Purchasing Debit
				(item.Type == "VSCD") || //Visa Corporate/ Commercial Debit
				(item.Type == "VSID") || //Visa Infinite Debit
				(item.Type == "AXCN") || //American Express Consumer
				(item.Type == "AXD") || //American Express Debit
				(item.Type == "AXCR") || //American Express Corporate
				(item.Type == "AXSB"); //American Express Small Business

			var isCredit =
				(item.Type == "VSA") || //Visa Credit
				(item.Type == "MSC") || //MasterCard Credit
				(item.Type == "VSBC") || //Visa Business Credit
				(item.Type == "MCBC") || //MasterCard Business Credit
				(item.Type == "VSPC") || //VISA Prepaid Credit
				(item.Type == "MCPC") || //MasterCard Prepaid Credit
				(item.Type == "MCWC") || //MasterCard World Credit
				(item.Type == "VPRC") || //Visa Purchasing Credit
				(item.Type == "VSCC") || //Visa Corporate / Commercial Credit
				(item.Type == "VSIC"); //Visa Infinite Credit
			
			var transactionDescription = isDebit ?
				"Web Load by Debit Card" :
				"Web Load by Credit Card";
			var directFee = isDebit ?
				"** WDB" :
				"** WCR";

			// Get state
			var depositToCard = await PfsService.DepositToCard(item.CardHolderId,
				item.Amount,
				transactionDescription,
				"1",
				directFee
			);

			item.DepositReferenceId = depositToCard.ReferenceId;

			return null;
        }
		#endregion ExecuteAsync
	}
}

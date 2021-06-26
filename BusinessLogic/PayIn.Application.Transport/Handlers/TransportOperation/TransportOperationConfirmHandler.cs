using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Promotions;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Infrastructure.Transport.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Handlers
{
	[XpLog("TransportOperation", "Confirm", RelatedId = "Id")]
	[XpAnalytics("TransportOperation", "Confirm")]
	public class TransportOperationConfirmHandler :
		IServiceBaseHandler<TransportOperationConfirmArguments>
	{
		[Dependency] public IEntityRepository<TransportOperation> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public ServerService ServerService { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportOperationConfirmArguments arguments)
		{
			var operation = (await Repository.GetAsync(arguments.Id, "Price.Title", "Ticket.Payments", "BlackList", "GreyList"));
			if (operation == null)
				throw new ArgumentNullException(nameof(TransportOperationConfirmArguments.Id));
            if (operation.OperationType == OperationType.Refund)
                throw new ApplicationException("No se puede confirmar una operación devuelta");

            #region Confirmar GreyList
            // Desmarcar
            if (operation.GreyListUnmarked)
				operation.GreyListUnmarkedResult = await ServerService.GreyListUnmark(operation, arguments.MobileSerial, arguments.Now);

            // Marcar
            foreach (var item in operation.GreyList)
                item.CodeReturned = await ServerService.GreyListMark(item, operation, arguments.MobileSerial, arguments.Now);
			#endregion Confirmar GreyList

			#region Confirmar BlackList
			// Desmarcar
			if (operation.BlackListUnmarked)
				operation.BlackListUnmarkedResult = await ServerService.BlackListUnmark(operation, arguments.MobileSerial, arguments.Now);

			// Marcar
			foreach (var item in operation.BlackList)
				item.CodeReturned = await ServerService.BlackListMark(item, operation, arguments.MobileSerial, arguments.Now);
			#endregion Confirmar BlackList

			operation.ConfirmationDate = arguments.Now;
			operation.Login = SessionData.Login;
            
            try
            {
                #region Confirmar Recarga
                if (operation.OperationType == OperationType.Recharge)
                {
                    // Get Pago
                    var rechargePayment = operation.Ticket?.Payments?.FirstOrDefault();
                    operation.ScriptResponse = (await ServerService.Recharged(operation, rechargePayment, arguments.MobileSerial, arguments.Now))
                        .ToJson();
                }
                #endregion Confirmar Recarga		

                #region Confirmar Devolución
                if (operation.OperationType == OperationType.Revoke)
                {
                    operation.ScriptResponse = (await ServerService.Revoked(operation, arguments.MobileSerial, arguments.Now))
                        .ToJson();
                }
                #endregion Confirmar Devolución	
            }
            catch (Exception e)
            {
                operation.Error = e.Message;
            }

            return operation;
		}
		#endregion ConfirmAsync
	}
}
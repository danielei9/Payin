using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    [XpLog("Ticket", "CreatePayedAndGet", RelatedId = "TicketId")]
    [XpAnalytics("Ticket", "CreatePayedAndGet")]
    public class PublicTicketCreatePayedAndGetHandler : //PublicTicketCreateAndGetHandler,
        IServiceBaseHandler<PublicTicketCreatePayedAndGetArguments>
    {
        [Dependency] public MobileTicketPayV3Handler MobileTicketPayV3Handler { get; set; }
        [Dependency] public IEntityRepository<Ticket> Repository { get; set; }
        [Dependency] public MobileTicketCreateAndGetHandler MobileTicketCreateAndGetHandler { get; set; }
        [Dependency] public PublicPaymentMediaGetByUserHandler PublicPaymentMediaGetByUserHandler { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(PublicTicketCreatePayedAndGetArguments arguments)
		{
			var now = DateTime.Now;

            arguments.Type = Common.TicketType.NotPayable;

            // Create ticket
            var ticket = await MobileTicketCreateAndGetHandler.CreateTicketAsync(arguments.Reference, null, arguments.Date, arguments.ConcessionId, arguments.EventId, arguments.Lines, null, new TicketAnswerFormsArguments_Form[0], null, null, arguments.Type, arguments.Email, arguments.Login, arguments.ExternalLogin, null, now, true, true);

            // Crear pago
            var payment = new Payment(
                ticket,
                ticket.Amount,
                0,
                now,
                userName: SessionData.Name,
                userLogin: SessionData.Login,
                externalLogin: arguments.ExternalLogin,
                taxNumber: SessionData.TaxNumber,
                taxName: SessionData.TaxName,
                taxAddress: SessionData.TaxAddress,
                authorizationCode: arguments.AuthorizationCode ?? arguments.Reference,
				commerceCode: arguments.CommerceCode
            );
            ticket.Payments.Add(payment);

            // Get ticket
            var result = await MobileTicketCreateAndGetHandler.GetTicketAsync(ticket);

            // Para guardar en el log
            arguments.TicketId = ticket.Id;

            return new ResultBase<dynamic>
            {
                Data = new[] { result }
            };
        }
		#endregion ExecuteAsync
	}
}

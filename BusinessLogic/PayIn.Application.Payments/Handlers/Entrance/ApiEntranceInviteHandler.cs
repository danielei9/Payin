using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Payments.Handlers
{
	public class ApiEntranceInviteHandler : IServiceBaseHandler<ApiEntranceInviteArguments>
	{
		[Dependency] public SessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
		[Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
		[Dependency] public MobileTicketCreateAndGetHandler TicketMobileCreateAndGetHandler { get; set; }
		[Dependency] public MobileTicketPayV3Handler TicketMobilePayV3Handler { get; set; }

		[Dependency] public EmailService EmailService { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ApiEntranceInviteArguments arguments)
		{
			var now = DateTime.UtcNow;

			var concession = (await PaymentConcessionRepository.GetAsync())
				.Where(x => x.Concession.Supplier.Login == SessionData.Login)
				.FirstOrDefault();

			var entranceType = await EntranceTypeRepository.GetAsync(arguments.EntranceTypeId);

            var countEntrances = (await EntranceRepository.GetAsync("EntranceType"))
                .Where(x => x.EntranceTypeId == entranceType.Id)
                .Count();

			// Crear ticket y entradas
			var resultCreate = (await TicketMobileCreateAndGetHandler.ExecuteAsync(new MobileTicketCreateAndGetArguments(
				"",
				null,
				now,
				concession.Id,
				entranceType.EventId,
				null,
				null,
				null,
				TicketType.Ticket,
				new List<MobileTicketCreateAndGetArguments_TicketLine>
				{
					new MobileTicketCreateAndGetArguments_TicketLine
					{
						Amount = 0,
						EntranceTypeId = arguments.EntranceTypeId,
						Quantity = arguments.Quantity,
						Title = entranceType.Name,
						Type = TicketLineType.Entrance
					}
				},
				null,
				null
			))) as MobileTicketCreateAndGetResultBase;

			var ticketId = resultCreate.Data.FirstOrDefault().Id;

			// Asignar usuarios
			var ticket = (await TicketRepository.GetAsync(ticketId, "Lines.Entrances"));
				
				
			foreach (var line in ticket.Lines)
				foreach (var entrance in line.Entrances)
				{
					entrance.UserName = arguments.UserName;
					entrance.LastName = arguments.LastName;
					entrance.Login = arguments.Email;
					entrance.VatNumber = arguments.VatNumber;
                    entrance.SendingCount = 0;
				}
			
			// Pagar ticket
			var resulPay = (await TicketMobilePayV3Handler.ExecuteAsync(new MobileTicketPayV3Arguments(
				ticketId,
				new List<MobileTicketPayV3Arguments_PaymentMedia>(),
				new List<MobileTicketPayV3Arguments_Promotion>(),
				"",
				"",
				"",
				"Web",
				"",
				"",
				"",
				"",
				"",
				"",
				""
			)));

			// Enviar mail con entrada
			var subject = "Invitación enviada";
			var message = string.Format("Ha recibido {0} entrada/s a un evento", arguments.Quantity);

			//payment.ErrorText = message;
			await EmailService.SendAsync(
				arguments.Email,
				subject,
				message
			);

			return resulPay;
		}
		#endregion ExecuteAsync
	}
}

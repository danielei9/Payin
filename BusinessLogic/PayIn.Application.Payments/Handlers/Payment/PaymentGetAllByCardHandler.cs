using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentGetAllByCardHandler : 
		IQueryBaseHandler<PaymentGetAllByCardArguments, PaymentGetAllByCardResult>
	{
		[Dependency] public IEntityRepository<Payment> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<PaymentGetAllByCardResult>> ExecuteAsync(PaymentGetAllByCardArguments arguments)
		{
			var serviceCard = (await ServiceCardRepository.GetAsync("OwnerUser"))
				.Where(x=>x.Id == arguments.Id)
				.First();
			if (serviceCard == null)
				throw new ArgumentException("ServiceCardId");

			long uid = serviceCard.Uid;
			string userLogin = serviceCard.OwnerUser.Login;

			var payments = (await Repository.GetAsync("Ticket", "Ticket.Concession.Concession"))
				.Where(x => 
					x.Uid == uid &&
					x.UserLogin == userLogin
				)
				.OrderBy(x => x.Seq)
				.ToList();


			var result = payments
				.Select(x => new PaymentGetAllByCardResult
				{
					Id = x.Id,
					TicketId = x.TicketId,
					Date = x.Date,
					ConcessionName = x.Ticket.Concession.Concession.Name,
					TicketAmount = x.Ticket.Amount,
					PaymentAmount = x.Amount
				});
					
			return new ResultBase<PaymentGetAllByCardResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

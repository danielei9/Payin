using PayIn.Application.Dto.Arguments.Ticket;
using PayIn.Domain.Internal;
using PayIn.Domain.Internal.Infrastructure;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	public class TicketPayHandler :
		IServiceBaseHandler<TicketPayArguments>
	{
		public readonly IEntityRepository<User> UserRepository;
		public readonly IPaymentGatewayAdapter PaymentGatewayAdapter;

		#region Contructors
		public TicketPayHandler(
			IEntityRepository<User> userRepository,
			IPaymentGatewayAdapter paymentGatewayAdapter
		)
		{
			if (userRepository == null)
				throw new ArgumentNullException("userRepository");
			UserRepository = userRepository;

			if (paymentGatewayAdapter == null)
				throw new ArgumentNullException("paymentGatewayAdapter");
			PaymentGatewayAdapter = paymentGatewayAdapter;
		}
		#endregion Contructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TicketPayArguments>.ExecuteAsync(TicketPayArguments command)
		{
			//var user = await UserRepository.GetAsync(SessionData.Id.Value);
			//if (command.Pin != user.Pin)
			//	throw new XpArgumentException("Pin", TicketResources.PinException);

			//await PaymentGatewayAdapter.PayAsync(command);

			return null;
		}
		#endregion ExecuteAsync
	}
}

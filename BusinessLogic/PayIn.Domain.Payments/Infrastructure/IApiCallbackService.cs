using System;
using System.Threading.Tasks;

namespace PayIn.Domain.Payments.Infrastructure
{
	public interface IApiCallbackService
	{
		Task<string> OnPayedAsync(string url, string login, DateTime date, int ticketId, int paymentId, decimal quantity);
		Task<string> OnPaymentMediaCreatedAsync(string url, string login, DateTime date, int ticketId, int paymentId);
		Task<string> OnPaymentMediaCreationErrorAsync(string url, string login, DateTime date, int ticketId, int paymentId, string errorCode, string errorMessage);
	}
}

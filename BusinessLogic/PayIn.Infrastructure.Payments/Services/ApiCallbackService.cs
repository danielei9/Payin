using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments.Infrastructure;
using System;
using System.Threading.Tasks;
using Xp.Infrastructure.Http;

namespace PayIn.Infrastructure.Payments.Services
{
	public class ApiCallbackService : BaseService, IApiCallbackService
	{
		public override string BaseAddress { get { return ""; } }

		#region Constructors
		public ApiCallbackService(ISessionData sessionData)
		{
			SessionData = sessionData;
		}
		#endregion Constructors

		#region OnPayedAsync
		private class OnPayedArguments
		{
			public string Login { get; set; }
			public int PaymentId { get; set; }
			public DateTime Date { get; set; }
			public decimal Quantity { get; set; }
		}
		public async Task<string> OnPayedAsync(string url, string login, DateTime date, int ticketId, int paymentId, decimal quantity)
		{
			var result = await Server.PostAsync(url, id: ticketId, arguments: new OnPayedArguments
			{
				Login = login,
				PaymentId = paymentId,
				Date = date,
				Quantity = quantity
			});

			return result.ToString();
		}
		#endregion OnPayedAsync

		#region OnPaymentMediaCreatedAsync
		private class OnPaymentMediaCreatedArguments
		{
			public string Login { get; set; }
			public int PaymentId { get; set; }
			public DateTime Date { get; set; }
		}
		public async Task<string> OnPaymentMediaCreatedAsync(string url, string login, DateTime date, int ticketId, int paymentId)
		{
			var result = await Server.PostAsync(url, id: ticketId, arguments: new OnPaymentMediaCreatedArguments
			{
				Login = login,
				PaymentId = paymentId,
				Date = date
			});

			return result.ToString();
		}
		#endregion OnPaymentMediaCreatedAsync

		#region OnPaymentMediaCreationErrorAsync
		private class OnPaymentMediaCreationErrorArguments
		{
			public string Login { get; set; }
			public int PaymentId { get; set; }
			public DateTime Date { get; set; }
			public string ErrorCode { get; set; }
			public string ErrorMessage { get; set; }
		}
		public async Task<string> OnPaymentMediaCreationErrorAsync(string url, string login, DateTime date, int ticketId, int paymentId, string errorCode, string errorMessage)
		{
			var result = await Server.PostAsync(url, id: ticketId, arguments: new OnPaymentMediaCreationErrorArguments
			{
				Login = login,
				PaymentId = paymentId,
				Date = date,
				ErrorCode = errorCode,
				ErrorMessage = errorMessage
			});

			return result.ToString();
		}
		#endregion OnPaymentMediaCreationErrorAsync
	}
}
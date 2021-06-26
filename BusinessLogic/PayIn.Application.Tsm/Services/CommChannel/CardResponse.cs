using Xp.Application.Tsm.GlobalPlatform.SmartCardIo;

namespace PayIn.Application.Tsm.Services.CommChannel
{
	public class CardResponse
	{
		public string Card { get; set; }
		public string TransactionId { get; set; }
		public PrintWriter ServlertResponseWriter { get; set; }
		public CountDownLatch ResponseSentSignal { get; set; }

		public CardResponse(PrintWriter servlertResponseWriter, CountDownLatch responseSentSignal, string transactionId, string card)
		{
			ServlertResponseWriter = servlertResponseWriter;
			ResponseSentSignal = responseSentSignal;
			TransactionId = transactionId;
			Card = card;
		}
	}
}

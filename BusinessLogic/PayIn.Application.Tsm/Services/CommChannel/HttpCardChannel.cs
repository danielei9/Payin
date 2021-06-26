using System.Collections.Generic;
using System.Linq;
using Xp.Application.Tsm.GlobalPlatform;
using Xp.Application.Tsm.GlobalPlatform.SmartCardIo;

namespace PayIn.Application.Tsm.Services.CommChannel
{
	public class HttpCardChannel : CardChannel
	{
		private Queue<CardResponse> CardResponseQueue;
		private PrintWriter result;

		public HttpCardChannel(Queue<CardResponse> cardResponseQueue)
		{
			CardResponseQueue = cardResponseQueue;
		}
		public void Close(string data)
		{
			var cardResponse = (CardResponse)null;
			try {
				cardResponse = CardResponseQueue.FirstOrDefault();
			} catch {
				return;
			}

			if (data == null)
				data = "no data";

			var command = new HttpResponseJson("EndTransaction", cardResponse.TransactionId, data);
			result = cardResponse.ServlertResponseWriter;
			result.Println(command.ToString());
			cardResponse.ResponseSentSignal.CountDown();
		}
		/*
		@Override
		public Card getCard()
		{
			// TODO Auto-generated method stub
			return null;
		}

		@Override
		public int getChannelNumber()
		{
			// TODO Auto-generated method stub
			return 0;
		}

		@Override
		public ResponseAPDU transmit(CommandAPDU arg0) throws CardException
		{
			String apduHexString = Conversion.arrayToHex(arg0.getBytes());
			CardResponse cardResponse =null;
			try {
				cardResponse = cardResponseQueue.take();
				HttpResponseJson command = new HttpResponseJson("CommandApdu", cardResponse.getTransferId(), apduHexString);
				out = cardResponse.getServlertResponseWriter();
				out.println(command.toJsonString());
				cardResponse.getResponseSentSignal().countDown();
			} catch (Exception e) {
				throw new CardException("http error");
			}
		
			try {
				cardResponse = cardResponseQueue.take();
				String commandResponseString = cardResponse.getResponseString();
				ResponseAPDU commandResponse = new ResponseAPDU(Conversion.hexToArray(commandResponseString));
				cardResponseQueue.put(cardResponse); //put back cardResponse for next command
				return commandResponse;
			} catch (InterruptedException e) {
				out.println("ResponseADPU not valid");
				return null;
			}
		}

		@Override
		public int transmit(ByteBuffer arg0, ByteBuffer arg1) throws CardException
		{
			ResponseAPDU responseApdu = transmit(new CommandAPDU(arg0));
			if(responseApdu == null) {
				throw new CardException("");
			}
			arg1.put(responseApdu.getBytes());
			return responseApdu.getBytes().length;
		}

		@Override
		public void close() throws CardException
		{
			close(null);
		}*/
	}
}

using System.Collections.Generic;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Domain.Transport;

namespace Xp.Application.Dto.Tsm.Arguments
{
	public class TsmExecuteArguments : GPArguments
	{
		public enum OperationType
		{
			Personalize = 0,
			Read = 1,
			Execute = 2
		}

		public CardSystem System { get; set; }
		public OperationType Type { get; set; }
		public IList<string> Data { get; set; } = new List<string>();
		public string TransactionId { get; set; }
		public int? OperationId { get; set; }
		public string State { get; set; }

		public NextStepEnum? NextStep { get; set; }
		public string User { get; set; }
		public string DeviceAddress { get; set; }
        public int CardEntry { get; set; }
		public uint CardUid { get; set; }
		public string CardName { get; set; }
		public string CardRandomId { get; set; }
		public int? PriceId { get; set; }
		public int? TicketId { get; set; }

		public string Script { get; set; }
    }
}
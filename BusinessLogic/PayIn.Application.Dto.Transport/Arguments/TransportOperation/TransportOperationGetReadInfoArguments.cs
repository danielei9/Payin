using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationGetReadInfoArguments : IArgumentsBase
	{
		public class CardId
		{
			public long Uid { get; set; }
			public CardType Type { get; set; }
		}
		public bool NeedKeys { get; set; } = true;

		[Required] public IEnumerable<CardId> Cards { get; set; }

		public dynamic Device { get; set; }	
		public long? Uids
		{
			get
			{
				return Cards
					.Select(x => x.Uid)
					.FirstOrDefault();
			}
		}
		public int? OperationId { get; set; }

		#region Constructors
		public TransportOperationGetReadInfoArguments(string mifareClassicCards, dynamic device)
		{
			Cards = mifareClassicCards.SplitString(",")
				.Select(x => new CardId
				{
					Uid = x.FromHexadecimal().ToInt32().Value,
					Type = CardType.MIFAREClassic
				})
				.ToList();
			Device = device;
		}
		#endregion Constructors
	}
}

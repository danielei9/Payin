using System.Collections.Generic;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;

namespace Xp.Application.Results
{
	public class ScriptResult
	{
		public class CardId
		{
			public long Uid { get; set; }
			public CardType Type { get; set; }
			public CardSystem System { get; set; }
		}

		public CardId Card { get; set; }
		public string Keys { get; set; }
		public IEnumerable<IMifareOperation> Script { get; set; }
	}
}

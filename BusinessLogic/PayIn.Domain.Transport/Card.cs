using Xp.Domain.Transport;

namespace PayIn.Domain.Transport
{
	public class Card
	{
		public CardSystem CardSystem { get; set; }
		public CardType CardType { get; set; }
		public long? CardId { get; set; }
	}
}

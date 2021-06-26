using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport
{
    public class CardInfo
    {
        public long Uid { get; set; }
        public CardType Type { get; set; }
        public CardSystem? CardSystem { get; set; }
    }
}

using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport
{
    public class OperationInfo
    {
        public int Id { get; set; }
        public OperationType Type { get; set; }
        public CardInfo Card { get; set; }
        public bool IsRead { get; set; }
    }
}

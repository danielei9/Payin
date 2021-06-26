using Xp.Application.Dto;

namespace PayIn.Application.Dto.Transport.Results
{
    public class Script2ResultBase<TResult> : ResultBase<TResult>
    {
        public CardData CardData { get; set; }
        public OperationInfo Operation { get; set; }
    }
}

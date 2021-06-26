using PayIn.Application.Dto.Transport.Results.TransportOperation;

namespace Xp.Application.Dto.Tsm.Arguments
{
    public class TsmExecuteArgumentsBase : ResultBase<TsmExecuteArguments>
    {
        public string Card { get; set; }
    }
}

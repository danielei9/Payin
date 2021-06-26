using PayIn.Application.Dto.Transport.Arguments.TransportOperation.Internal;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationClassicNoCompatible2Arguments : TransportOperationSearchInternalArguments
	{
		#region Constructors
		public TransportOperationClassicNoCompatible2Arguments(string cardId)
			:base(cardId)
		{
		}
		#endregion Constructors
	}
}

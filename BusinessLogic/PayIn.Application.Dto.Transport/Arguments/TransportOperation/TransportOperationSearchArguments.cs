using PayIn.Application.Dto.Transport.Arguments.TransportOperation.Internal;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationSearchArguments : TransportOperationSearchInternalArguments
	{
		#region Constructors
		public TransportOperationSearchArguments(string cardId)
			: base(cardId)
		{
		}
		#endregion Constructors
	}
}
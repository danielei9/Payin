using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using Xp.Application.Handlers;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeIsVisibleHandler :
		UpdateHandler<EntranceTypeIsVisibleArguments, EntranceType>
	{
		#region Constructors
		public EntranceTypeIsVisibleHandler(IEntityRepository<EntranceType> repository)
			: base(repository)
		{
		}
		#endregion Constructors
	}
}
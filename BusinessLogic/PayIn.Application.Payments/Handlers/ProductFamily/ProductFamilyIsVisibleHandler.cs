using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using Xp.Application.Handlers;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductFamilyIsVisibleHandler : 
		UpdateHandler<ProductFamilyIsVisibleArguments, ProductFamily>
	{
		#region Constructors
		public ProductFamilyIsVisibleHandler(IEntityRepository<ProductFamily> repository)
			: base(repository)
		{
		}
		#endregion Constructors
	}
}

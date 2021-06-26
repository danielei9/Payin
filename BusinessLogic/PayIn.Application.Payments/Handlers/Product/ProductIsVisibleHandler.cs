using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using Xp.Application.Handlers;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductIsVisibleHandler : 
		UpdateHandler<ProductIsVisibleArguments, Product>
	{
		#region Constructors
		public ProductIsVisibleHandler(IEntityRepository<Product> repository)
			: base(repository)
		{
		}
		#endregion Constructors
	}
}

using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Public;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductAddGroupHandler :
		IServiceBaseHandler<ProductAddGroupArguments>
	{
		[Dependency] public IEntityRepository<ServiceGroupProduct> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ProductAddGroupArguments arguments)
		{
			var item = new ServiceGroupProduct
			{
				ProductId = arguments.Id,
				GroupId = arguments.GroupId
			};
			await Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}

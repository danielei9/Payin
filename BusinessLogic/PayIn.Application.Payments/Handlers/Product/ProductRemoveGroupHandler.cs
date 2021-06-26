using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Public;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductRemoveGroupHandler :
		IServiceBaseHandler<ProductRemoveGroupArguments>
	{
		[Dependency] public IEntityRepository<ServiceGroupProduct> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ProductRemoveGroupArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					x.ProductId == arguments.Id &&
					x.GroupId == arguments.GroupId
				);

			foreach (var item in items)
				await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}

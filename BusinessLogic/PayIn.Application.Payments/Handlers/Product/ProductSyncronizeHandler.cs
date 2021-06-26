using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductSyncronizeHandler :
		IServiceBaseHandler<ProductSyncronizeArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Product> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> RepositoryPaymentConcession { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ProductSyncronizeArguments arguments)
		{
			var concession = (await RepositoryPaymentConcession.GetAsync("Products"))
				.Where(x =>
					(x.Id == arguments.PaymentConcessionId) &&
                    (x.Concession.State == ConcessionState.Active) &&
                    (
                        (
                            (x.Concession.Supplier.Login == SessionData.Login) &&
                            (x.Concession.State == ConcessionState.Active)
                        ) || (
                            (x.PaymentWorkers
                                .Any(y =>
                                    (y.State == WorkerState.Active) &&
                                    (y.Login == SessionData.Login)
                                )
                            )
                        )
					)
				)
				.FirstOrDefault();
            var productsBD = concession.Products
                .Where(x => x.State == ProductState.Active)
                .ToList();
            if (concession == null)
				throw new ApplicationException("No existe identificador de Concesion");

			// Recorrer los Productos de BD
			// - Si no está en arguments.products lo elimino
			foreach (var productBD in productsBD)
				if (!arguments.Products.Any(x => x.Code == productBD.Code))
					productBD.State = ProductState.Deleted;

			// Recorrer arguments.products:
			// - Si está en BD lo modifico
			// - Si no en BD está lo creo
			foreach(var productArgument in arguments.Products)
			{
				var productBD = productsBD
                    .Where(x => x.Code == productArgument.Code)
					.FirstOrDefault();

				if (productBD != null)
					productBD.Name = productArgument.Name;
				else
				{
					productBD = new Product
					{
						Code = productArgument.Code,
						Name = productArgument.Name,
						PhotoUrl = "",
						Description = "",
						State = ProductState.Active,
						IsVisible = false,
						Visibility = ProductVisibility.Internal
					};
					concession.Products.Add(productBD);
				}
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}

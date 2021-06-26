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
	public class ProductFamilyCreateHandler : 	IServiceBaseHandler<ProductFamilyCreateArguments>
	{
		private readonly IUnitOfWork								UnitOfWork;
		private readonly ISessionData								SessionData;
		private readonly IEntityRepository<ProductFamily>		    Repository;
		private readonly IEntityRepository<PaymentConcession>		RepositoryPaymentConcession;
		private readonly IEntityRepository<Product>				    RepositoryProduct;		

		#region Constructors
		public ProductFamilyCreateHandler(
			IUnitOfWork unitOfWork,
			ISessionData sessionData,
			IEntityRepository<ProductFamily> repository,
			IEntityRepository<PaymentConcession> repositoryPaymentConcession,
			IEntityRepository<Product> repositoryProduct
		)
		{
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryPaymentConcession == null) throw new ArgumentNullException("repositoryPaymentConcession");
			if (repositoryProduct == null) throw new ArgumentNullException("repositoryProduct");

			UnitOfWork = unitOfWork;
			SessionData = sessionData;
			Repository = repository;
            RepositoryPaymentConcession = repositoryPaymentConcession;
            RepositoryProduct = repositoryProduct;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ProductFamilyCreateArguments arguments)
		{
			var concession = (await RepositoryPaymentConcession.GetAsync())
				.Where(x =>
                         x.Concession.Supplier.Login == SessionData.Login)
                .FirstOrDefault();
			
            if (concession == null)
                throw new ApplicationException("No existe identificador de Concesion");

            var productFamily = new ProductFamily
            {
                Name                = arguments.Name,
                Description         = arguments.Description,
                PhotoUrl            = "",
                State               = ProductFamilyState.Active,
                PaymentConcessionId = concession.Id,
                SuperFamilyId       = arguments.Id,
				IsVisible			= true,
                Code                = arguments.Code
            };
            await Repository.AddAsync(productFamily);

            return productFamily;
    	}
		#endregion ExecuteAsync
	}
}

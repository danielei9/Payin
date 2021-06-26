using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class CampaignLineAddProductFamilyHandler : 
		IServiceBaseHandler<CampaignLineAddProductFamilyArguments>
	{
		private readonly IEntityRepository<CampaignLine> Repository;
		private readonly IEntityRepository<ProductFamily> ProductFamilyRepository;

		#region Constructors
		public CampaignLineAddProductFamilyHandler(
			IEntityRepository<CampaignLine> repository,
			IEntityRepository<ProductFamily> productFamilyRepository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (productFamilyRepository == null) throw new ArgumentNullException("productFamilyRepository");

			Repository = repository;
			ProductFamilyRepository = productFamilyRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(CampaignLineAddProductFamilyArguments arguments)
		{
			var productFamilies = (await ProductFamilyRepository.GetAsync(arguments.ProductFamilyId));
			if (productFamilies == null)
				throw new ArgumentNullException("ProductFamilyId");

			var item = (await Repository.GetAsync(arguments.Id, "ProductFamilies"));
			if (item == null)
				throw new ArgumentNullException("Id");

			item.ProductFamilies.Add(productFamilies);

			return item;
		}
		#endregion ExecuteAsync
	}
}

using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignLineRemoveProductFamilyHandler :
		IServiceBaseHandler<CampaignLineRemoveProductFamilyArguments>
	{
		private readonly IEntityRepository<CampaignLine> Repository;
		private readonly IEntityRepository<ProductFamily> ProductFamilyRepository;

		#region Constructors
		public CampaignLineRemoveProductFamilyHandler(
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
		public async Task<dynamic> ExecuteAsync(CampaignLineRemoveProductFamilyArguments arguments)
		{
			var productFamilies = (await ProductFamilyRepository.GetAsync(arguments.ProductFamilyId));
			if (productFamilies == null)
				throw new ArgumentNullException("ProductFamilyId");

			var item = (await Repository.GetAsync(arguments.Id, "ProductFamilies"));
			if (item == null)
				throw new ArgumentNullException("Id");

			item.ProductFamilies.Remove(productFamilies);

			return item;
		}
		#endregion ExecuteAsync
	}
}

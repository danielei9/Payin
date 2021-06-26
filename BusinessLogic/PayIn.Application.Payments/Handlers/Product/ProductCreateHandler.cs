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
	public class ProductCreateHandler :
		IServiceBaseHandler<ProductCreateArguments>
	{
		private readonly IUnitOfWork                          UnitOfWork;
		private readonly ISessionData                         SessionData;
		private readonly IEntityRepository<Product>           Repository;
		private readonly IEntityRepository<ProductFamily>     FamilyRepository;
        private readonly IEntityRepository<PaymentConcession> RepositoryPaymentConcession;

        #region Constructors
        public ProductCreateHandler(
			IUnitOfWork unitOfWork,
			ISessionData sessionData,
			IEntityRepository<Product>	repository,
            IEntityRepository<PaymentConcession> repositoryPaymentConcession,
            IEntityRepository<ProductFamily>	familyRepository
		)
		{
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (familyRepository == null) throw new ArgumentNullException("familyRepository");
            if (repositoryPaymentConcession == null) throw new ArgumentNullException("repositoryPaymentConcession");


            UnitOfWork = unitOfWork;
			SessionData = sessionData;
			Repository = repository;
			FamilyRepository = familyRepository;
            RepositoryPaymentConcession = repositoryPaymentConcession;
        }
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ProductCreateArguments arguments)
		{
			var concession = (await RepositoryPaymentConcession.GetAsync())
				.Where(x => x.Concession.Supplier.Login == SessionData.Login)
				.FirstOrDefault();

			if (concession == null)
				throw new ApplicationException("No existe identificador de Concesion");

			if (arguments.ToConsult == true)
			{
				var product = new Product
				{
					Name = arguments.Name,
					Code = arguments.Code,
					Description = arguments.Description,
					Price = null,
					FamilyId = arguments.FamilyId,
					State = ProductState.Active,
					PaymentConcessionId = concession.Id,
					PhotoUrl = "",
					IsVisible = arguments.IsVisible,
					Visibility = arguments.Visibility
				};
				await Repository.AddAsync(product);
				return product;
			}
			
			else
			{
				var product = new Product
				{
					Name = arguments.Name,
					Code = arguments.Code,
					Description = arguments.Description,
					Price = arguments.Price,
					FamilyId = arguments.FamilyId,
					State = ProductState.Active,
					PaymentConcessionId = concession.Id,
					PhotoUrl = "",
					IsVisible = true,
					Visibility = arguments.Visibility
				};
				await Repository.AddAsync(product);
				return product;
			}
		}
		#endregion ExecuteAsync
	}
}

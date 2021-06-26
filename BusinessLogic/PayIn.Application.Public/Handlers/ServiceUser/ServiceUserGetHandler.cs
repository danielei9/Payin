using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserGetHandler :
		IQueryBaseHandler<ServiceUserGetArguments, ServiceUserGetResult>
	{
		private readonly IEntityRepository<ServiceUser>		Repository;
		
		#region Constructors
		public ServiceUserGetHandler(
			IEntityRepository<ServiceUser> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceUserGetResult>> ExecuteAsync(ServiceUserGetArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var res1 = items
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					LastName = x.LastName,
					Phone = x.Phone,
					Address = x.Address,
					Email = x.Email,
					//AssertDoc = x.AssertDoc,
					//AssertDocument = x.AssertDocument,
					OwnCard = ((int?)x.Card.ConcessionId ?? 0) == x.ConcessionId,
					VatNumber = x.VatNumber,
					Uid = (long?)x.Card.Uid,
					BirthDate = x.BirthDate,
					Code = x.Code,
					Observations = x.Observations,
					PhotoUrl = x.Photo,
					CardState = (ServiceCardState?)x.Card.State,
					CardConcessionName = x.Card.Concession.Name ?? ""
				});

			var res2 = res1
			.ToList();
			
			var result = res2
				.Select(x => new ServiceUserGetResult
				{
					Id = x.Id,
					Name = x.Name,
					LastName = x.LastName,
					Phone = x.Phone,
					Address = x.Address,
					Email = x.Email,
					//AssertDoc = x.AssertDoc,
					//AssertDocument = new FileDto {
					//	Url = x.AssertDocument
					//},
					OwnCard = x.OwnCard,
					VatNumber = x.VatNumber,
					Uid = x.Uid,
					BirthDate = x.BirthDate,
					PhotoUrl = x.PhotoUrl,
					CardState = x.CardState,
					CardConcessionName = x.CardConcessionName,
					Code = x.Code,
					Observations = x.Observations
				});

			return new ResultBase<ServiceUserGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}


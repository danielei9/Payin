using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.Application.Dto.Payments.Results.Purse;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Payments.Purse;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Common;
using PayIn.Common.Security;

namespace PayIn.Application.Public.Handlers
{
	public class PurseGetHandler :
		IQueryBaseHandler<PurseGetArguments, PurseGetResult>
	{
		private readonly IEntityRepository<serV> _Repository;
		private readonly ISessionData _SessionData;

		#region Constructors
		public PurseGetHandler(IEntityRepository<serV> repository, ISessionData sessionData)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;

			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			_SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PurseGetResult>> ExecuteAsync(PurseGetArguments arguments)
		{
			var now = DateTime.Now.ToUTC();
			var items = (await _Repository.GetAsync())
			.Where(x => x.Id == arguments.Id);


			var result = items
			.Select(x => new 
			{
				Id = x.Id,
				Name = x.Name,				
				Validity = x.Validity,
				Expiration = x.Expiration,
				PhotoUrl = x.Image
			})			
			.ToList()
			.Select(x => new PurseGetResult
			{
				Id = x.Id,
				Name = x.Name,				
				Validity = x.Validity.ToUTC(),
				Expiration = x.Expiration.ToUTC(),
				PhotoUrl = (x.PhotoUrl == null) ? null : x.PhotoUrl

			})
			.ToList();

			return new ResultBase<PurseGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

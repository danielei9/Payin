using PayIn.Application.Dto.Transport.Arguments.TransportCardApplication;
using PayIn.Application.Dto.Transport.Results.TransportCardApplication;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class TransportCardApplicationGetAllHandler :
		IQueryBaseHandler<TransportCardApplicationGetAllArguments, TransportCardApplicationGetAllResult>
	{
		private readonly IEntityRepository<TransportCardApplication> Repository;

		#region Constructor
		public TransportCardApplicationGetAllHandler(
			IEntityRepository<TransportCardApplication> repository
			)		
		{
			if (repository == null) throw new ArgumentNullException("TransportCard");		

			Repository = repository;		
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<ResultBase<TransportCardApplicationGetAllResult>> ExecuteAsync(TransportCardApplicationGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
					   .Where(x => x.ApplicationId == arguments.ApplicationId && x.KeyVersion == arguments.KeyVersion);

			if (items != null)
			{
				var result = items
						.Select(x => new
						{
							Id = x.Id,
							ApplicationId = x.ApplicationId,
							KeyVersion = x.KeyVersion,
							Content = x.Content,
							AccessCondition = x.AccessCondition,
							ReadKey = x.ReadKey,
							WriteKey = x.WriteKey
						})
						.ToList()
						.Select(x => new TransportCardApplicationGetAllResult
						{
							Id = x.Id,
							ApplicationId = x.ApplicationId,
							KeyVersion = x.KeyVersion,
							Content = x.Content,
							AccessCondition = x.AccessCondition,
							ReadKey = x.ReadKey,
							WriteKey = x.WriteKey
						});

				return new ResultBase<TransportCardApplicationGetAllResult> { Data = result };
			}
			else
				return null;
		}
		#endregion ExecuteAsync
	}
}

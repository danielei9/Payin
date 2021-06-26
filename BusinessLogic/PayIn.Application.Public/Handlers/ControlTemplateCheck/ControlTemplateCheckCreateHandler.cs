using PayIn.Application.Dto.Arguments.ControlTemplateCheck;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateCheckCreateHandler :
		IServiceBaseHandler<ControlTemplateCheckCreateArguments>
	{
		private readonly IEntityRepository<ControlTemplateCheck> Repository;
		private readonly IEntityRepository<ServiceCheckPoint> RepositoryServiceCheckPoint;

		#region Constructors
		public ControlTemplateCheckCreateHandler(
			IEntityRepository<ControlTemplateCheck> repository,
			IEntityRepository<ServiceCheckPoint> repositoryServiceCheckPoint
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryServiceCheckPoint == null) throw new ArgumentNullException("repositoryServiceCheckPoint");
			RepositoryServiceCheckPoint = repositoryServiceCheckPoint;
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlTemplateCheckCreateArguments arguments)
		{
			var checkpoint = await RepositoryServiceCheckPoint.GetAsync(arguments.CheckPointId);
			if (checkpoint.Type != Common.CheckPointType.Round)
				throw new ApplicationException(ControlTemplateCheckResources.OnlyTypeRound);

			var itemTemplateCheck = new ControlTemplateCheck
			{
				CheckPointId = arguments.CheckPointId,				
				Time = arguments.Time,
				TemplateId = arguments.TemplateId
			};
			await Repository.AddAsync(itemTemplateCheck);
			return itemTemplateCheck;
		}
		#endregion ExecuteAsync
	}
}

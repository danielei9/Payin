using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlItemAddTagHandler :
		IServiceBaseHandler<ControlItemAddTagArguments>
	{
		private readonly IEntityRepository<ControlItem> Repository;
		private readonly IEntityRepository<ServiceCheckPoint> RepositoryServiceCheckPoint;

		#region Constructors
		public ControlItemAddTagHandler(IEntityRepository<ControlItem> repository, IEntityRepository<ServiceCheckPoint> repositoryServiceCheckPoint)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryServiceCheckPoint == null) throw new ArgumentNullException("repositoryServiceCheckPoint");

			Repository = repository;
			RepositoryServiceCheckPoint = repositoryServiceCheckPoint;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlItemAddTagArguments>.ExecuteAsync(ControlItemAddTagArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id, "y");
			var tag = await RepositoryServiceCheckPoint.GetAsync(arguments.TagId);

			item.CheckPoints.Add(tag);

			return item;
		}
		#endregion ExecuteAsync
	}
}

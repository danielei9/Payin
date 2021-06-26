using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlItemRemoveTagHandler :
		IServiceBaseHandler<ControlItemRemoveTagArguments>
	{
		private readonly IEntityRepository<ControlItem> _Repository;
		private readonly IEntityRepository<ServiceCheckPoint> _RepositoryServiceCheckPoint;

		#region Constructors
		public ControlItemRemoveTagHandler(IEntityRepository<ControlItem> repository, IEntityRepository<ServiceCheckPoint> repositoryServiceCheckPoint)
		{
			if (repository == null)    throw new ArgumentNullException("repository");
			if (repositoryServiceCheckPoint == null) throw new ArgumentNullException("repositoryServiceCheckPoint");

			_Repository = repository;
			_RepositoryServiceCheckPoint = repositoryServiceCheckPoint;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlItemRemoveTagArguments arguments)
		{
			var item = await _Repository.GetAsync(arguments.Id, "y");
			var tag = await _RepositoryServiceCheckPoint.GetAsync(arguments.TagId);

			item.CheckPoints.Remove(tag);

			return item;
		}
		#endregion ExecuteAsync
	}
}

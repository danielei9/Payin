using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public
{
	public class ControlItemDeleteHandler :
		IServiceBaseHandler<ControlItemDeleteArguments>
	{
		private readonly IEntityRepository<ControlItem> Repository;

		#region Constructors
		public ControlItemDeleteHandler(ISessionData sessionData, IEntityRepository<ControlItem> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ControlItemDelete
		async Task<dynamic> IServiceBaseHandler<ControlItemDeleteArguments>.ExecuteAsync(ControlItemDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ControlItemDelete
	}
}
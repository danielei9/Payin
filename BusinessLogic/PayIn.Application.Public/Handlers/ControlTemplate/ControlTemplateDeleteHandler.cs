using PayIn.Application.Dto.Arguments.ControlTemplate;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateDeleteHandler :
			IServiceBaseHandler<ControlTemplateDeleteArguments>
	{
		private readonly IEntityRepository<ControlTemplate> Repository;

		#region Constructors
		public ControlTemplateDeleteHandler(ISessionData sessionData, IEntityRepository<ControlTemplate> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlTemplateDeleteArguments>.ExecuteAsync(ControlTemplateDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}

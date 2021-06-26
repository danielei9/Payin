using PayIn.Application.Dto.Arguments.ControlTemplateItem;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateItemUpdateHandler :
		IServiceBaseHandler<ControlTemplateItemUpdateArguments>
	{
		private readonly IEntityRepository<ControlTemplateItem> Repository;

		#region Constructors
		public ControlTemplateItemUpdateHandler(IEntityRepository<ControlTemplateItem> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlTemplateItemUpdateArguments>.ExecuteAsync(ControlTemplateItemUpdateArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id, new [] { "Since", "Until" });
			item.Since.Time = arguments.Since;
			item.Until.Time = arguments.Until;

			return item;
		}
		#endregion ExecuteAsync
	}
}

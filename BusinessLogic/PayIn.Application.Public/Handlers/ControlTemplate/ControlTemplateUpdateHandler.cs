using PayIn.Application.Dto.Arguments.ControlTemplate;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateUpdateHandler :
		IServiceBaseHandler<ControlTemplateUpdateArguments>
	{
		private readonly IEntityRepository<ControlTemplate> Repository;

		#region Constructors
		public ControlTemplateUpdateHandler(IEntityRepository<ControlTemplate> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlTemplateUpdateArguments>.ExecuteAsync(ControlTemplateUpdateArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			item.Name = arguments.Name;
			item.Observations = arguments.Observations;
			item.CheckDuration = arguments.CheckDuration;
			item.Monday = arguments.Monday;
			item.Tuesday = arguments.Tuesday;
			item.Wednesday = arguments.Wednesday;
			item.Thursday = arguments.Thursday;
			item.Friday = arguments.Friday;
			item.Saturday = arguments.Saturday;
			item.Sunday = arguments.Sunday;

			return item;
		}
		#endregion ExecuteAsync
	}
}

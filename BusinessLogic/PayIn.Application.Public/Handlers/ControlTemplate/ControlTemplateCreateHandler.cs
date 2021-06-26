using PayIn.Application.Dto.Arguments.ControlTemplate;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateCreateHandler :
		IServiceBaseHandler<ControlTemplateCreateArguments>
	{
		private readonly IEntityRepository<ControlTemplate> Repository;

		#region Constructors
		public ControlTemplateCreateHandler(IEntityRepository<ControlTemplate> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlTemplateCreateArguments>.ExecuteAsync(ControlTemplateCreateArguments arguments)
		{
			var item = new ControlTemplate
			{
				Name = arguments.Name ?? "",
				Observations = arguments.Observations ?? "",
				CheckDuration = arguments.CheckDuration == null ? (XpDuration) null : arguments.CheckDuration,
				Monday = arguments.Monday,
				Tuesday = arguments.Tuesday,
				Wednesday = arguments.Wednesday,
				Thursday = arguments.Thursday,
				Friday = arguments.Friday,
				Saturday = arguments.Saturday,
				Sunday = arguments.Sunday,
				ItemId = arguments.ItemId
			};
			await Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}

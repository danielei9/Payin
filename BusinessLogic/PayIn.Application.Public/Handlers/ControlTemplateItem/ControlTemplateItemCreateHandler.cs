using PayIn.Application.Dto.Arguments.ControlTemplateItem;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateItemCreateHandler :
		IServiceBaseHandler<ControlTemplateItemCreateArguments>
	{
		private readonly IEntityRepository<ControlTemplateItem> _Repository;

		#region Constructors
		public ControlTemplateItemCreateHandler(IEntityRepository<ControlTemplateItem> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlTemplateItemCreateArguments arguments)
		{
			var item = new ControlTemplateItem
			{
				TemplateId = arguments.TemplateId,
				Since = new ControlTemplateCheck
				{
					Time = arguments.Since,
					TemplateId = arguments.TemplateId
				},
				Until = new ControlTemplateCheck
				{
					Time = arguments.Until,
					TemplateId = arguments.TemplateId
				}
			};

			await _Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}

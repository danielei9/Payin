using PayIn.Application.Dto.Arguments.ControlFormAssignTemplate;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignTemplateUpdateHandler { //:
	//	IServiceBaseHandler<ControlFormAssignTemplateUpdateArguments>
	//{
	//	private readonly IEntityRepository<ControlFormAssignTemplate> _Repository;

	//	#region Constructors
	//	public ControlFormAssignTemplateUpdateHandler(IEntityRepository<ControlFormAssignTemplate> repository)
	//	{
	//		if (repository == null) throw new ArgumentNullException("repository");
	//		_Repository = repository;
	//	}
	//	#endregion Constructors

		//#region ExecuteAsync
		//public async Task<dynamic> ExecuteAsync(ControlFormAssignTemplateUpdateArguments arguments)
		//{
			//var item = await _Repository.GetAsync(arguments.Id, "Values");

			//foreach (var newValue in arguments.Values)
			//{
			//	var oldValue = item.Values.Where(x => x.Id == newValue.Id).FirstOrDefault();
			//	if (oldValue != null)
			//	{
			//		oldValue.ValueBool = newValue.ValueBool;
			//		oldValue.ValueDateTime = newValue.ValueDateTime;
			//		oldValue.ValueNumeric = newValue.ValueNumeric;
			//		oldValue.ValueString = newValue.ValueString;
			//	}
			//}

			//return item;
		//}
		//#endregion ExecuteAsync
	}
}

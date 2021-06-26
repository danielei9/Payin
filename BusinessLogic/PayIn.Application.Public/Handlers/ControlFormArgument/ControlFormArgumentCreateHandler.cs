using PayIn.Application.Dto.Arguments.ControlFormArgument;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormArgumentCreateHandler :
		IServiceBaseHandler<ControlFormArgumentCreateArguments>
	{
		private readonly IEntityRepository<ControlFormArgument> _Repository;
		private readonly IEntityRepository<ControlForm> ControlFormRepository;

		#region Constructors
		public ControlFormArgumentCreateHandler(
			IEntityRepository<ControlFormArgument> repository,
			IEntityRepository<ControlForm> controlFormRepository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (controlFormRepository == null) throw new ArgumentNullException("controlFormRepository");
			
			_Repository = repository;
			ControlFormRepository = controlFormRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlFormArgumentCreateArguments>.ExecuteAsync(ControlFormArgumentCreateArguments arguments)
		{
			var form = (await ControlFormRepository.GetAsync())
				.Where(x => x.Id == arguments.FormId)
				.FirstOrDefault();

			var objs = (await _Repository.GetAsync())
				.Where(x =>
					x.Order >= arguments.Order &&
					x.FormId == arguments.FormId
				)
				.ToList();

			if (objs.Where(x => x.Order == arguments.Order).Any())
					foreach (var obj in objs)
					obj.Order++;

			if (arguments.Type == Common.ControlFormArgumentType.MultiOption)
			{
				var item = new ControlFormArgument
				{
					Name = arguments.Name ?? "",
					Observations = arguments.Observations ?? "",
					Type = arguments.Type,
					Target = Common.ControlFormArgumentTarget.Check,
					MinOptions = arguments.MinOptions,
					MaxOptions = arguments.MaxOptions,
					Form = form,
					FormId = form.Id,
					State = Common.ControlFormArgumentState.Active,
					Order = arguments.Order,
					Required = false
				};
				await _Repository.AddAsync(item);
				return item;
			}
			else
			{
				int minOptions = 0;
				if (arguments.Required)
				{
					minOptions = 1;
				}
				
				var item = new ControlFormArgument
				{
					Name = arguments.Name ?? "",
					Observations = arguments.Observations ?? "",
					Type = arguments.Type,
					Target = Common.ControlFormArgumentTarget.Check,
					Form = form,
					FormId = form.Id,
					MinOptions = minOptions,
					State = Common.ControlFormArgumentState.Active,
					Order = arguments.Order,
					Required = arguments.Required
				};
				await _Repository.AddAsync(item);
				return item;
			}
		}
		#endregion ExecuteAsync
	}
}

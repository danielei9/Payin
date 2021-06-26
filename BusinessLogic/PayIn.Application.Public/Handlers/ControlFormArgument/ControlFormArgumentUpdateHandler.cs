using PayIn.Application.Dto.Arguments.ControlFormArgument;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using System.Linq;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormArgumentUpdateHandler :
		IServiceBaseHandler<ControlFormArgumentUpdateArguments>
	{
		private readonly IEntityRepository<ControlFormArgument> Repository;

		#region Contructors
		public ControlFormArgumentUpdateHandler(
			IEntityRepository<ControlFormArgument> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlFormArgumentUpdateArguments arguments)
		{
			var formArgument = (await Repository.GetAsync(arguments.Id));

			var min = Math.Min(formArgument.Order, arguments.Order);
			var max = Math.Max(formArgument.Order, arguments.Order);

			var objs = (await Repository.GetAsync())
				.Where(x =>
					x.Order >= min &&
					x.Order <= max &&
					x.FormId == formArgument.FormId
				)
				.ToList();

			if (objs.Where(x => x.Order == arguments.Order).Any())
				foreach (var obj in objs)
					{
						if (obj.Order >= formArgument.Order)
							obj.Order--;

						if (obj.Order >= arguments.Order)
							obj.Order++;
					}

			if (arguments.Type == ControlFormArgumentType.MultiOption)
			{
				formArgument.Name = arguments.Name;
				formArgument.Observations = arguments.Observations;
				formArgument.Type = arguments.Type;
				formArgument.MaxOptions = arguments.MaxOptions;
				formArgument.MinOptions = arguments.MinOptions;
				formArgument.Order = arguments.Order;
				formArgument.Required = false;
			}
			else
			{
				int minOptions = 0;
				if (arguments.Required)
				{
					minOptions = 1;
				}
				
				formArgument.Name = arguments.Name;
				formArgument.Observations = arguments.Observations;
				formArgument.Type = arguments.Type;
				formArgument.MinOptions = minOptions;
				formArgument.Order = arguments.Order;
				formArgument.Required = arguments.Required;
			}

			return formArgument;
		}
		#endregion ExecuteAsync
	}
}

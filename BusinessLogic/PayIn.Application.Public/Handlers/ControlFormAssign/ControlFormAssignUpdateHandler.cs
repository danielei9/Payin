using PayIn.Application.Dto.Arguments.ControlFormAssign;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignUpdateHandler :
		IServiceBaseHandler<ControlFormAssignUpdateArguments>
	{
		private readonly IEntityRepository<ControlFormAssign> _Repository;

		#region Constructors
		public ControlFormAssignUpdateHandler(IEntityRepository<ControlFormAssign> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlFormAssignUpdateArguments arguments)
		{
			foreach (var assign in arguments.Assigns)
			{
				var item = await _Repository.GetAsync(assign.Id, "Values");

				foreach (var newValue in assign.Values)
				{
					var oldValue = item.Values.Where(x => x.Id == newValue.Id).FirstOrDefault();
					if (oldValue != null)
					{
						oldValue.ValueBool = newValue.ValueBool;
						oldValue.ValueDateTime = newValue.ValueDateTime;
						oldValue.ValueNumeric = newValue.ValueNumeric;
						oldValue.ValueString = newValue.ValueString;
					}
				}
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}

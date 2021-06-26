using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormOptionUpdateHandler :
		IServiceBaseHandler<ControlFormOptionUpdateArguments>
	{
		private readonly IEntityRepository<ControlFormOption> Repository;

		#region Constructors
		public ControlFormOptionUpdateHandler(
			IEntityRepository<ControlFormOption> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlFormOptionUpdateArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.Text = arguments.Text;
			item.Value = arguments.Value;

			return item;
		}
		#endregion ExecuteAsync
	}
}

using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ExhibitorDeleteHandler :
		IServiceBaseHandler<ExhibitorDeleteArguments>
	{
		private readonly IEntityRepository<Exhibitor> Repository;

		#region Constructors
		public ExhibitorDeleteHandler(IEntityRepository<Exhibitor> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ExhibitorDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.State = ExhibitorState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}

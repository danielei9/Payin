using PayIn.Application.Dto.Arguments.ControlForm;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormDeleteHandler :
			IServiceBaseHandler<ControlFormDeleteArguments>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ControlForm> Repository;
		private readonly IEntityRepository<PayIn.Domain.Public.ControlFormArgument> ArgumentsRepository;

		#region Constructors
		public ControlFormDeleteHandler(
			IEntityRepository<PayIn.Domain.Public.ControlForm> repository,
			IEntityRepository<PayIn.Domain.Public.ControlFormArgument> argumentsRepository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

			if (argumentsRepository == null)
				throw new ArgumentNullException("repository");
			ArgumentsRepository = argumentsRepository;
		}
		#endregion Constructors

		#region ControlFormDelete
		async Task<dynamic> IServiceBaseHandler<ControlFormDeleteArguments>.ExecuteAsync(ControlFormDeleteArguments arguments)
		{
			//var item = await Repository.GetAsync(arguments.Id, "Arguments");

			//while (item.Arguments.Count() > 0) {
			//	await ArgumentsRepository.DeleteAsync(item.Arguments.First());
			//}

			//await Repository.DeleteAsync(item);

			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = Common.ControlFormState.Deleted;

			return null;
		}
		#endregion ControlFormDelete
	}
}

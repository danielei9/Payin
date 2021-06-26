using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class ActivityUpdateHandler :
		IServiceBaseHandler<ActivityUpdateArguments>
	{
		private readonly IEntityRepository<Activity> Repository;

		#region Constructors
		public ActivityUpdateHandler(
			IEntityRepository<Activity> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ActivityUpdateArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.Name = arguments.Name;
			item.Description = arguments.Description;
			item.Start = arguments.Start;
			item.End = arguments.End;

			return item;
		}
		#endregion ExecuteAsync
	}
}

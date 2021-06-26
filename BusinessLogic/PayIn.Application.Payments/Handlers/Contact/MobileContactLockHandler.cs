using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileContactLockHandler :
		IServiceBaseHandler<MobileContactLockArguments>
	{
		private readonly IEntityRepository<Contact> Repository;

		#region Constructors
		public MobileContactLockHandler(IEntityRepository<Contact> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileContactLockArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.State=ContactState.Locked;

			return item;
		}
		#endregion ExecuteAsync
	}
}

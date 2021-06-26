using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileContactUnlockHandler :
		IServiceBaseHandler<MobileContactUnlockArguments>
	{
		private readonly IEntityRepository<Contact> Repository;

		#region Constructors
		public MobileContactUnlockHandler(IEntityRepository<Contact> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileContactUnlockArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.State = ContactState.Active;

			return item;
		}
		#endregion ExecuteAsync
	}
}

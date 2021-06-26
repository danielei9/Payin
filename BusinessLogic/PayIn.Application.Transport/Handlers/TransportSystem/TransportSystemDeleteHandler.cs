using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Transport.Arguments.TransportSystem;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class TransportSystemDeleteHandler :
		IServiceBaseHandler<TransportSystemDeleteArguments>
	{
		private readonly IEntityRepository<TransportSystem> Repository;

		#region Constructors
		public TransportSystemDeleteHandler(
			IEntityRepository<TransportSystem> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");			
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportSystemDeleteArguments>.ExecuteAsync(TransportSystemDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();
			
			item.State = TransportSystemState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}

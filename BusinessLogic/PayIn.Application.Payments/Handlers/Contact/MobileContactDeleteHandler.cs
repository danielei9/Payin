using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileContactDeleteHandler :
		IServiceBaseHandler<MobileContactDeleteArguments>
	{
		private readonly IEntityRepository<Contact> Repository;
		//private readonly ISessionData SessionData;

		#region Constructors
		public MobileContactDeleteHandler(IEntityRepository<Contact> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileContactDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x =>
					x.Id == arguments.Id
				)
				.FirstOrDefault();

			if (item == null)
				throw new ApplicationException("No hay contactos ha eliminar");

			item.State = ContactState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}

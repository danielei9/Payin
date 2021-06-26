using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	public class PurseImageDeleteHandler :
		IServiceBaseHandler<PurseImageDeleteArguments>
	{
		private readonly IEntityRepository<Purse> Repository;		

		#region Constructors
		public PurseImageDeleteHandler(IEntityRepository<Purse> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");			

			Repository = repository;			
		}
		#endregion Constructors

		#region PurseImageDelete
		public async Task<dynamic> ExecuteAsync(PurseImageDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));		
			var blob = new AzureBlobRepository();

			if (item.Image != "")
			{
				var route = Regex.Split(item.Image, "[/?]");
				var fileName = route[route.Length - 2] + "/" + route[route.Length - 1];
				blob.DeleteFile(item.Image);
				item.Image = null;
			}

			return null;
		}
		#endregion PaymentUserDelete
	}
}

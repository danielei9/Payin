using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using PayIn.Common.Resources;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeUpdatePhotoHandler :
		IServiceBaseHandler<EntranceTypeUpdatePhotoArguments>
	{
		private readonly IEntityRepository<EntranceType> Repository;

		#region Constructors
		public EntranceTypeUpdatePhotoHandler(
			IEntityRepository<EntranceType> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeUpdatePhotoArguments arguments)
		{
			var now = DateTime.Now;
			var entrances = (await Repository.GetAsync())
					.Where(x => x.Id == arguments.Id)
					.FirstOrDefault();

			var azureBlob = new AzureBlobRepository();
			byte[] b1 = System.Text.Encoding.UTF8.GetBytes(arguments.PhotoUrl);
            var name = arguments.Id + "_" + Guid.NewGuid();

            var oldImage = entrances.PhotoUrl;
			entrances.PhotoUrl = azureBlob.SaveImage(EntranceTypeResources.PhotoShortUrl.FormatString(name), b1);

			if (oldImage != null && oldImage != "")
			{
				var route = Regex.Split(oldImage, "[/?]");
				var fileName = route[route.Length - 2] + "/" + route[route.Length - 1];
				azureBlob.DeleteFile(oldImage);
			}

			return entrances;
		}
		#endregion ExecuteAsync
	}
}

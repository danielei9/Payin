using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Payments.Handlers
{
	class NoticeUpdatePhotoHandler :
		IServiceBaseHandler<NoticeUpdatePhotoArguments>
	{
		private readonly IEntityRepository<Notice> Repository;

		#region Constructors
		public NoticeUpdatePhotoHandler(
			IEntityRepository<Notice> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(NoticeUpdatePhotoArguments arguments)
		{
			var now = DateTime.Now;
			var notice = (await Repository.GetAsync())
					.Where(x => x.Id == arguments.Id)
					.FirstOrDefault();

			var azureBlob = new AzureBlobRepository();
			byte[] b1 = System.Text.Encoding.UTF8.GetBytes(arguments.PhotoUrl);
            var name = arguments.Id + "_" + Guid.NewGuid();

            name = NoticeResources.PhotoShortUrl.FormatString(name);

            var oldImage = notice.PhotoUrl;
			notice.PhotoUrl = azureBlob.SaveImage(name, b1);

            // PRUEBA PARA VER SI EL FICHERO SE HA CREADO BIEN - Si se puede borrar, es que existe
            // azureBlob.DeleteFile(name);

			if (oldImage != null && oldImage != "")
			{
				var route = Regex.Split(oldImage, "[/?]");
				var fileName = route[route.Length - 2] + "/" + route[route.Length - 1];
				azureBlob.DeleteFile(oldImage);
			}

			return notice;
		}
#endregion ExecuteAsync
	}
}

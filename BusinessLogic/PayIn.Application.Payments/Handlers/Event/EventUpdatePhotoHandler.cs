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
using Microsoft.Practices.Unity;

namespace PayIn.Application.Payments.Handlers
{
	public class EventUpdatePhotoHandler :
		IServiceBaseHandler<EventUpdatePhotoArguments>
	{
		[Dependency] public IEntityRepository<Event> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EventUpdatePhotoArguments arguments)
		{
			var now = DateTime.Now;
			var events = (await Repository.GetAsync())
					.Where(x => x.Id == arguments.Id)
					.FirstOrDefault();

			var azureBlob = new AzureBlobRepository();
			byte[] b1 = System.Text.Encoding.UTF8.GetBytes (arguments.PhotoUrl);
		    var name = arguments.Id + "_" + Guid.NewGuid();

            var oldImage = events.PhotoUrl;
			events.PhotoUrl = azureBlob.SaveImage(EventResources.PhotoShortUrl.FormatString(name), b1);

			if (oldImage != null && oldImage != "")
			{
				azureBlob.DeleteFile(oldImage);
			}

			return events;
		}
        #endregion ExecuteAsync
    }
}

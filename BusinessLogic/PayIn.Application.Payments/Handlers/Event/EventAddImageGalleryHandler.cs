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
	public class EventAddImageGalleryHandler :
		IServiceBaseHandler<EventAddImageGalleryArguments>
	{
		private readonly IEntityRepository<EventImage> Repository;

		#region Constructors
		public EventAddImageGalleryHandler(
			IEntityRepository<EventImage> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EventAddImageGalleryArguments arguments)
		{
            var now = DateTime.Now;
            var item = (await Repository.GetAsync()).OrderByDescending(o => o.Id).FirstOrDefault();

            var eventImage = new EventImage
            {
                //Id = item.Id + 1,
                EventId = arguments.Id,
                PhotoUrl = arguments.ImageUrl
            };

            var azureBlob = new AzureBlobRepository();
            byte[] b1 = System.Text.Encoding.UTF8.GetBytes(arguments.ImageUrl);
            var name = arguments.Id + "_" + Guid.NewGuid();
			
			eventImage.PhotoUrl = azureBlob.SaveImage(EventGalleryResources.PhotoShortUrl.FormatString(name), b1);


            //eventImage.EventId = arguments.Id;
            //var Image = eventImage.PhotoUrl;
            //var route = Regex.Split(Image, "[/?]");
            //var fileName = route[route.Length - 2] + "/" + route[route.Length - 1];

            await Repository.AddAsync(eventImage);


            return eventImage;

            //            var AzureRepos = new AzureBlobRepository();
            //            var item = await Repository.GetAsync(arguments.Id);

            //            byte[] b1 = System.Text.Encoding.UTF8.GetBytes(arguments.ImageUrl);

            //#if TEST || DEBUG || EMULATOR
            //            item.PhotoUrl = EventGalleryResources.PhotoUrlTest.FormatString(arguments.ImageUrl);
            //            AzureRepos.SaveImage(EventGalleryResources.PhotoUrlTest.FormatString(arguments.Id), b1);
            //#endif // TEST || DEBUG
            //            item.PhotoUrl = EventGalleryResources.PhotoUrl.FormatString(arguments.Id);
            //            AzureRepos.SaveImage(EventGalleryResources.PhotoShortUrl.FormatString(arguments.Id), b1);

            //            return item;
        }
        #endregion ExecuteAsync
    }
}

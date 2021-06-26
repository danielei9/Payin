using PayIn.Application.Dto.Payments.Arguments;
using System.Text.RegularExpressions;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Payments.Handlers
{
	public class EventDeleteImageGalleryHandler :
		IServiceBaseHandler<EventDeleteImageGalleryArguments>
	{
		private readonly IEntityRepository<EventImage> Repository;

		#region Constructors
		public EventDeleteImageGalleryHandler(IEntityRepository<EventImage> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EventDeleteImageGalleryArguments arguments)
		{
            var eventImages = (await Repository.GetAsync()).ToList();
            var image = eventImages.Where(y => y.Id == arguments.Id).FirstOrDefault();

            var azureBlob = new AzureBlobRepository();
            //var url = image.PhotoUrl;
            //var route = Regex.Split(url, "[/?]");
            //var fileName = route[route.Length - 2] + "/" + route[route.Length - 1];


            eventImages.Remove(image);

            azureBlob.DeleteFile(image.PhotoUrl);

            await Repository.DeleteAsync(image);

            return null;
		}
		#endregion ExecuteAsync
	}
}

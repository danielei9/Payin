using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductUpdatePhotoHandler :
		IServiceBaseHandler<ProductUpdatePhotoArguments>
	{
		private readonly IEntityRepository<Product> Repository;

		#region Constructors
		public ProductUpdatePhotoHandler(
			IEntityRepository<Product> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ProductUpdatePhotoArguments arguments)
		{
			var now = DateTime.Now;
			var product = (await Repository.GetAsync())
					.Where(x => x.Id == arguments.Id)
					.FirstOrDefault();

			var azureBlob = new AzureBlobRepository();
			byte[] b1 = System.Text.Encoding.UTF8.GetBytes (arguments.PhotoUrl);
            var name = arguments.Id + "_" + Guid.NewGuid();

            var oldImage = product.PhotoUrl;
			product.PhotoUrl = azureBlob.SaveImage(ProductResources.PhotoShortUrl.FormatString(name), b1);

			if (oldImage != null && oldImage != "")
			{
				var route = Regex.Split(oldImage, "[/?]");
				var fileName = route[route.Length - 2] + "/" + route[route.Length - 1];
				azureBlob.DeleteFile(oldImage);
			}

			return product;
		}
        #endregion ExecuteAsync
    }
}

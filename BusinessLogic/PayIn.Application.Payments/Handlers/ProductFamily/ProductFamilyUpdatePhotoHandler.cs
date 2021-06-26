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

namespace PayIn.Application.Public.Handlers
{
	public class ProductFamilyUpdatePhotoHandler :
		IServiceBaseHandler<ProductFamilyUpdatePhotoArguments>
	{
		private readonly IEntityRepository<ProductFamily> Repository;

		#region Constructors
		public ProductFamilyUpdatePhotoHandler(
			IEntityRepository<ProductFamily> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ProductFamilyUpdatePhotoArguments arguments)
		{
			var now = DateTime.Now;
			var product = (await Repository.GetAsync())
					.Where(x => x.Id == arguments.Id)
					.FirstOrDefault();

			var azureBlob = new AzureBlobRepository();
			byte[] b1 = System.Text.Encoding.UTF8.GetBytes (arguments.PhotoUrl);
            var name = arguments.Id + "_" + Guid.NewGuid();

            var oldImage = product.PhotoUrl;
			product.PhotoUrl = azureBlob.SaveImage(ProductFamilyResources.PhotoShortUrl.FormatString(name), b1);

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

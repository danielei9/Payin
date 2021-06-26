using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
    public class ServiceUserUpdatePhotoHandler :
        IServiceBaseHandler<ServiceUserUpdatePhotoArguments>
    {
        [Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(ServiceUserUpdatePhotoArguments arguments)
        {
            var now = DateTime.Now;
            var serviceUser = (await Repository.GetAsync("Card"))
                    .Where(x => x.Id == arguments.Id)
                    .FirstOrDefault();

            var azureBlob = new AzureBlobRepository();
            byte[] b1 = Encoding.UTF8.GetBytes(arguments.Image);
            var name = arguments.Id + "_" + Guid.NewGuid();

            var oldImage = serviceUser.Photo;
            serviceUser.Photo = azureBlob.SaveImage(ServiceUserResources.PhotoShortUrl.FormatString(name), b1);

            if (oldImage != null && oldImage != "")
            {
                var route = Regex.Split(oldImage, "[/?]");
                var fileName = route[route.Length - 2] + "/" + route[route.Length - 1];
                azureBlob.DeleteFile(oldImage);
            }

            return serviceUser;
        }
        #endregion ExecuteAsync
    }
}

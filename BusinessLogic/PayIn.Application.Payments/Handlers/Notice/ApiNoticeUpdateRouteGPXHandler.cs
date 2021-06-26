using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;
using PayIn.Common.Resources;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiNoticeUpdateRouteGPXHandler :
		IServiceBaseHandler<ApiNoticeUpdateRouteGPXArguments>
	{
		[Dependency] public IEntityRepository<Notice> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ApiNoticeUpdateRouteGPXArguments arguments)
		{
			var notice = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();
			if (notice == null)
				throw new ArgumentNullException("id");

			var oldRouteUrl = notice.RouteUrl;
			if (arguments.RouteUrl != null)
			{
				var azureBlob = new AzureBlobRepository();
				var name = notice.Id + "_" + Guid.NewGuid();
				var fileName = NoticeResources.RouteGPXFile.FormatString(name);

				var routeUrl = azureBlob.SaveFile(fileName, arguments.RouteUrl);
				notice.RouteUrl = routeUrl;
				if (oldRouteUrl != "")
				{
					// Borrar anterior fichero de ruta GPX
				}
			}
			else if (notice.RouteUrl != "")
			{
				notice.RouteUrl = "";
				if (oldRouteUrl != "")
				{
					// Borrar anterior fichero de ruta GPX
				}
			}

			return notice;
		}
		#endregion ExecuteAsync
	}
}

using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility;
using PayIn.Application.Dto.Transport.Results.TransportCardSupportTitleCompatibility;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportCardSupportTitleCompatibilityUpdateIdHandler : IQueryBaseHandler<TransportCardSupportTitleCompatibilityUpdateIdArguments, TransportCardSupportTitleCompatibilityUpdateIdResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<TransportTitle> TransportTitleRepository { get; set; }
		[Dependency] public IEntityRepository<TransportCardSupportTitleCompatibility> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<TransportCardSupportTitleCompatibilityUpdateIdResult>> ExecuteAsync(TransportCardSupportTitleCompatibilityUpdateIdArguments arguments)
        {
            var titleName = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .Select(x => x.TransportTitle.Name)
                .FirstOrDefault();
            if (titleName == null)
                throw new ArgumentNullException("Id");

			var results = (await Repository.GetAsync())
                .Where(x =>
					x.Id == arguments.Id
				)
				.Select(x => new TransportCardSupportTitleCompatibilityUpdateIdResult
				{
					Id = x.Id,
					TransportTitleId = x.TransportTitleId,
					TransportCardSupportId = x.TransportCardSupportId,
					TransportCardSupportName = x.TransportCardSupport.Name
				});

            return new TransportCardSupportTitleCompatibilitypdateIdResultBase
			{
				Data = results,
				TitleName = titleName
			};
		}
		#endregion ExecuteAsync
	}
}


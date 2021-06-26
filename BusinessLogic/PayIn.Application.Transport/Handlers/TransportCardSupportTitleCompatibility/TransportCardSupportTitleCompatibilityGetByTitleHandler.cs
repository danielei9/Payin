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
    public class TransportCardSupportTitleCompatibilityGetByTitleHandler : IQueryBaseHandler<TransportCardSupportTitleCompatibilityArguments, TransportCardSupportTitleCompatibilityGetByTitleResult>
    {
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<TransportTitle> TransportTitleRepository { get; set; }
        [Dependency] public IEntityRepository<TransportCardSupportTitleCompatibility> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<TransportCardSupportTitleCompatibilityGetByTitleResult>> ExecuteAsync(TransportCardSupportTitleCompatibilityArguments arguments)
        {
            var titleName = (await TransportTitleRepository.GetAsync())
                .Where(x => x.Id == arguments.TitleId)
                .Select(x => x.Name)
                .FirstOrDefault();
            if (titleName == null)
                throw new ArgumentNullException("TitleId");

            var results = (await Repository.GetAsync());
            if (!arguments.Filter.IsNullOrEmpty())
                results = results.Where(x => (
                   x.TransportCardSupport.Name.Contains(arguments.Filter) ||
                   x.TransportCardSupport.OwnerName.Contains(arguments.Filter)
                ));

            var items = results
                .Where(x =>
                    x.TransportTitleId == arguments.TitleId
                )
                .OrderBy(x => x.TransportCardSupport.OwnerCode)
                .ThenBy(x => x.TransportCardSupport.Name)
                .Select(x => new TransportCardSupportTitleCompatibilityGetByTitleResult
                {
                    Id = x.Id,
                    Name = x.TransportCardSupport.Name,
                    OwnerCode = x.TransportCardSupport.OwnerCode,
                    OwnerName = x.TransportCardSupport.OwnerName,
                    Type = x.TransportCardSupport.Type,
                    SubType = x.TransportCardSupport.SubType,
                    State = x.TransportCardSupport.State
                });

            return new TransportCardSupportTitleCompatibilityGetByTitleResultBase
            {
                Data = items,
                TitleName = titleName
            };
        }
        #endregion ExecuteAsync
    }
}


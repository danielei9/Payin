using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities;
using PayIn.Application.Dto.Transport.Results.TransportSimultaneousTitleCompatibilities;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
    public class TransportSimultaneousTitleCompatibilitiesGetByTitleHandler : IQueryBaseHandler<TransportSimultaneousTitleCompatibilitiesArguments, TransportSimultaneousTitleCompatibilitiesGetByTitleResult>
    {
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<TransportTitle> TransportTitleRepository { get; set; }
        [Dependency] public IEntityRepository<TransportSimultaneousTitleCompatibility> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<TransportSimultaneousTitleCompatibilitiesGetByTitleResult>> ExecuteAsync(TransportSimultaneousTitleCompatibilitiesArguments arguments)
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
                   x.TransportTitle.Name.Contains(arguments.Filter) ||
                   x.TransportTitle.OwnerName.Contains(arguments.Filter)
                ));

            var items = results
                .Where(x =>
                    x.TransportTitleId == arguments.TitleId
                )
                .Select(x => new TransportSimultaneousTitleCompatibilitiesGetByTitleResult
				{
                    IdSimultaneous = x.Id,   
                    Id = x.TransportTitle2.Id,
                    Name = x.TransportTitle2.Name,
                    OwnerCode = x.TransportTitle2.OwnerCode,
                    OwnerName = x.TransportTitle2.OwnerName,
					Code = x.TransportTitle2.Code,
					EnvironmentAlias = (x.TransportTitle2.Environment == null ? "" : x.TransportTitle2.Environment.ToString()),

				})
				.ToList();

            return new TransportSimultaneousTitleCompatibilitiesGetByTitleResultBase
			{
                Data = items,
                TitleName = titleName
            };
        }
        #endregion ExecuteAsync
    }
}


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
    public class TransportCardSupportTitleCompatibilityCreateGetNameHandler : IQueryBaseHandler<TransportCardSupportTitleCompatibilityCreateGetNameArguments, TransportCardSupportTitleCompatibilityCreateGetNameResult>
    {
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<TransportTitle> TransportTitleRepository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<TransportCardSupportTitleCompatibilityCreateGetNameResult>> ExecuteAsync(TransportCardSupportTitleCompatibilityCreateGetNameArguments arguments)
        {
            var titleName = (await TransportTitleRepository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .Select(x => x.Name)
                .FirstOrDefault();
            if (titleName == null)
                throw new ArgumentNullException("Id");

            return new TransportCardSupportTitleCompatibilityCreateGetNameResultBase
            {                
                TitleName = titleName
            };
        }
        #endregion ExecuteAsync
    }
}









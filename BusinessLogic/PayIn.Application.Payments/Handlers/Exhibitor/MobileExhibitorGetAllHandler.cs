using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class MobileExhibitorGetAllHandler :
        IQueryBaseHandler<MobileExhibitorGetAllArguments, MobileExhibitorGetAllResult>
    {
        private readonly IEntityRepository<Event> Repository;
        private readonly ISessionData SessionData;

        #region Constructors
        public MobileExhibitorGetAllHandler(
            IEntityRepository<Event> repository,
            ISessionData sessionData
        )
        {
            if (repository == null) throw new ArgumentNullException("repository");

            Repository = repository;
            SessionData = sessionData;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<ResultBase<MobileExhibitorGetAllResult>> ExecuteAsync(MobileExhibitorGetAllArguments arguments)
        {
            var result = (await Repository.GetAsync())
                        .Where(y => y.Id == arguments.EventId)
                        .SelectMany(x => x.Exhibitors)
                        .Where(x => x.State ==ExhibitorState.Active || x.State == ExhibitorState.Suspended)
                        .Select(x => new MobileExhibitorGetAllResult
                        {
                            Id   = x.Id,
                            Name = x.Name,
                            State = x.State
                        });

            return new ResultBase<MobileExhibitorGetAllResult> { Data = result};
        }
        #endregion ExecuteAsync
    }
}

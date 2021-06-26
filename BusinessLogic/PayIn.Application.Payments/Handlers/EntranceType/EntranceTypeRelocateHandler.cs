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

namespace PayIn.Application.Public.Handlers
{
    public class EntranceTypeRelocateHandler :
        IServiceBaseHandler<EntranceTypeRelocateArguments>
    {
        private readonly IEntityRepository<EntranceType> Repository;
        private readonly IEntityRepository<Entrance> EntranceRepository;
        private readonly ISessionData SessionData;

        #region Constructors
        public EntranceTypeRelocateHandler(IEntityRepository<EntranceType> repository, 
                                           ISessionData sessionData,
                                           IEntityRepository<Entrance> entranceRepository
                                           )
        {
            if (repository == null) throw new ArgumentNullException("repository");
            if (entranceRepository == null) throw new ArgumentNullException("entranceRepository");
            if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
            EntranceRepository = entranceRepository;
            SessionData = sessionData;
        }
        #endregion Constructors

        #region ExecuteAsync
        async Task<dynamic> IServiceBaseHandler<EntranceTypeRelocateArguments>.ExecuteAsync(EntranceTypeRelocateArguments arguments)
        {
            var oldEntranceType = (await Repository.GetAsync()).
                Where(x =>
                            x.Id == arguments.OldId).FirstOrDefault();

            var assignEntranceType = (await Repository.GetAsync()).
               Where(x =>
                           x.Id == arguments.Id).FirstOrDefault();

            var oldEntrances = (await EntranceRepository.GetAsync())
                .Where(x =>
                            x.EntranceTypeId == oldEntranceType.Id);

            foreach(var entrance in oldEntrances)
            {
                entrance.EntranceTypeId = assignEntranceType.Id;
            }

            oldEntranceType.State = EntranceTypeState.Deleted;

            return null;
        }
		#endregion ExecuteAsync
	}
}
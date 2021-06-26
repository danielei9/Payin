using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ApiEntranceCheckGetAllHandler :
        IQueryBaseHandler<ApiEntranceCheckGetAllArguments, ApiEntranceCheckGetAllResult>
    {
        private readonly IEntityRepository<Check> Repository;

        #region Constructors
        public ApiEntranceCheckGetAllHandler(
            IEntityRepository<Check> repository
		)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            Repository = repository;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<ResultBase<ApiEntranceCheckGetAllResult>> ExecuteAsync(ApiEntranceCheckGetAllArguments arguments)
        {
            var checks = (await Repository.GetAsync())
                 .Where(x =>
                     x.EntranceId == arguments.Id
             )
             .Select(x => new
             {
                 Id = x.Id,
                 TypeEnum = x.Type,
                 Login = x.Login,
                 Observations = x.Observations,
                 TimeStamp = (x.TimeStamp == XpDateTime.MinValue) ? null : (DateTime?)x.TimeStamp,
                 EntranceId = x.EntranceId,
                 EntranceAlias = x.Entrance.Code,
                 TimeStampTime = (x.TimeStamp == XpDateTime.MinValue) ? null : (DateTime?)x.TimeStamp,
                 TimeStampDate = (x.TimeStamp == XpDate.MinValue) ? null : (x.TimeStamp.Day + "-" + x.TimeStamp.Month + "-" + x.TimeStamp.Year),
                 Subtitle = x.Entrance.EntranceType.Name +" ( " + x.Entrance.Code + " )" 
             })
                .ToList()
                .Select(x => new ApiEntranceCheckGetAllResult
                {
                    Id = x.Id,
                    Type = x.TypeEnum,
                    TypeAlias = x.TypeEnum.ToEnumAlias(),
                    Login = x.Login,
                    Observations = x.Observations,
                    TimeStamp = x.TimeStamp.ToUTC(),
                    EntranceId = x.EntranceId,
                    EntranceAlias = x.EntranceAlias.ToString(),
                    TimeStampTime = x.TimeStampTime.ToUTC(),
                    TimeStampDate = x.TimeStampDate,
                    Subtitle = x.Subtitle
                });

            return new ResultBase<ApiEntranceCheckGetAllResult> { Data = checks };
        }
        #endregion ExecuteAsync
    }
}
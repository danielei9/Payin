using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class MobileEntranceGetAllHandler :
        IQueryBaseHandler<MobileEntranceGetAllArguments, MobileEntranceGetAllResult>
    {
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
        [Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobileEntranceGetAllResult>> ExecuteAsync(MobileEntranceGetAllArguments arguments)
        {
            var now = DateTime.UtcNow;

            var entrances = (await ExecuteInternalAsync(now, SessionData.Login, null, arguments.PaymentConcessionId, null));

            return new ResultBase<MobileEntranceGetAllResult>
            {
                Data = entrances
            };
        }
        #endregion ExecuteAsync

        #region ExecuteInternalAsync
        public async Task<IEnumerable<MobileEntranceGetAllResult>> ExecuteInternalAsync(DateTime now, string login, int? systemCardId, int? paymentConcessionId, int? eventId)
        {
            var systemCardMembers = (await SystemCardMemberRepository.GetAsync());

			var entrances1 = (await EntranceRepository.GetAsync())
				.Where(x =>
					(x.State == EntranceState.Active) &&
					(
						(!systemCardMembers
							.Any(y =>
								(y.Login == x.Event.PaymentConcession.Concession.Supplier.Login)
							)
						) ||
						(systemCardMembers
							.Any(y =>
								(y.Login == x.Event.PaymentConcession.Concession.Supplier.Login) &&
								(
									(y.SystemCard.ClientId == "") ||
									(y.SystemCard.ClientId == SessionData.ClientId)
								)
							)
						)
					) &&
					(
						(systemCardId == null) ||
						(systemCardMembers
							.Any(y =>
								(y.Login == x.Event.PaymentConcession.Concession.Supplier.Login) &&
								(y.SystemCardId == systemCardId)
							)
						)
					) &&
					(
						(paymentConcessionId == null) ||
						(x.Event.PaymentConcessionId == paymentConcessionId)
					) &&
					(
						(eventId == null) ||
						(x.EventId == eventId)
					) &&
					(x.Login == login)
				)
				.Select(x => new
				{
					x.Id,
					x.Event.Name,
					x.Event.EventStart,
					x.Event.EventEnd,
					x.Event.CheckInStart,
					x.Event.Place,
					EventState = x.Event.State,
					x.EntranceType.PhotoUrl,
					CodeTemplate = x.Event.EntranceSystem.Template,
					CodeType = x.Event.EntranceSystem.Type,
					EventCode = x.Event.Code,
					EntranceTypeName = x.EntranceType.Name,
					x.UserName,
					UserTaxNumber = x.VatNumber,
					EntranceTypeCode = "",
					EntranceCode = x.Code,
					MinutesToEvent = SqlFunctions.DateDiff("Mi", x.Event.EventStart, now)
				})
				.Where(x => now < SqlFunctions.DateAdd("Mi", 60, x.EventEnd))
				.OrderBy(x => x.EventStart)
				//.OrderBy(x => SqlFunctions.Sign(x.MinutesToEvent))
				//.ThenBy(x => x.MinutesToEvent)
				.ToList();

			var entrances = entrances1
                .Select(x => new MobileEntranceGetAllResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    EventStart = x.EventStart.ToUTC(),
                    CheckInStart = x.CheckInStart.ToUTC(),
                    Place = x.Place,
                    EventState = x.EventState,
                    EventStateName = x.EventState.ToEnumAlias(),
                    UserName = x.UserName,
                    UserTaxNumber = x.UserTaxNumber,
                    EntranceTypeName = x.EntranceTypeName,
                    PhotoUrl = x.PhotoUrl,
                    Code = x.CodeTemplate.FormatString(x.EventCode ?? 0, x.EntranceTypeCode, x.EntranceCode),
                    CodeType = x.CodeType
                });

            return entrances;
        }
        #endregion ExecuteInternalAsync
    }
}

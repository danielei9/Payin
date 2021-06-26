using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class MobileServiceCardGetHandler :
        IQueryBaseHandler<MobileServiceCardGetArguments, ServiceCardReadInfoResult>
    {
        [Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
        [Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
        [Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
        [Dependency] public IEntityRepository<PurseValue> PurseValueRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        private string hexArray = "0123456789ABCDEF";

        #region ExecuteAsync
        public async Task<ResultBase<ServiceCardReadInfoResult>> ExecuteAsync(MobileServiceCardGetArguments arguments)
		{
            var now = DateTime.UtcNow;

            var purseValues = (await PurseValueRepository.GetAsync());
            var card = (await Repository.GetAsync())
				.Where(x =>
					(x.Id == arguments.Id)
				)
				.Select(x => new {
					CardId = x.Id,
					UserId = x.OwnerUserId,
					UserName = x.OwnerUser.Name,
					UserPhoto = x.OwnerUser.Photo,
					UserVatNumber = x.OwnerUser.VatNumber,
					x.SystemCard.ConcessionOwnerId,
					ConcessionOwnerName = x.SystemCard.ConcessionOwner.Name,
					x.SystemCardId,
					x.Alias,
					x.Uid,
					UidText =
						x.ServiceCardBatch == null ?
							x.Uid.ToString() :
						x.ServiceCardBatch.UidFormat == UidFormat.BigEndian ?
							(
								hexArray.Substring((int)((x.Uid / 268435456) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 16777216) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 1048576) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 65536) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 4096) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 256) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 16) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 1) % 16), 1)
							).ToString() :
						x.ServiceCardBatch.UidFormat == UidFormat.LittleEndian ?
							(
								hexArray.Substring((int)((x.Uid / 16) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 1) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 4096) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 256) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 1048576) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 65536) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 268435456) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 16777216) % 16), 1)
							).ToString() :
						x.Uid.ToString(),
					Type = (
						(x.State != ServiceCardState.Emited) ? "No emitida" :
                        (x.LinkedUsers.Any(y => y.Login == SessionData.Login)) ? "Vinculada" :
						(x.OwnerUserId == null) ? "Anónima" :
						(x.Users.Any()) ? "Principal" :
							"Secundaria"
					),
					Groups = x.Groups
						.Select(y => new ServiceCardReadInfoResult_Group
						{
							Id = x.Id,
							Name = x.Alias
						}),
                    Operations = x.Operations
                        .OrderByDescending(y => y.Seq)
                        .Select(y => new
                        {
                            Id = y.Id,
                            Date = y.Date,
                            Type = y.Type,
                            Seq = y.Seq,
                            ESeq = y.ESeq,
                            PurseValues = purseValues
                                .Where(z => z.ServiceOperationId == y.Id)
                                .OrderBy(z => z.Purse.Name)
                                .Select(z => new ServiceCardReadInfoResult_PurseValue
                                {
                                    Name = z.Purse.Name,
                                    Amount = z.Amount
                                })
                        }),
                })
				.FirstOrDefault();
			if (card == null)
				return null;

			var inBlackLists = (await BlackListRepository.GetAsync())
				.Where(y =>
					(y.Uid == card.Uid) &&
					(y.SystemCardId == card.SystemCardId) &&
					(y.State == BlackList.BlackListStateType.Active) &&
					(!y.Resolved)
				)
				.Any();

            var entrances_ = (await EntranceRepository.GetAsync())
                .Where(x =>
                    x.Id == card.CardId
                );
            var entrances = (await GetEntrancesAsync(entrances_, now));

            var result = new ServiceCardReadInfoResultBase {
				CardId = card.UidText.ToString(),
				Owner = card.ConcessionOwnerId,
				OwnerName = card.ConcessionOwnerName,
				TypeName = card.Type,
				Alias = card.Alias,
				InBlackList = inBlackLists,
				IsExpired = false,
				IsRechargable = true,
				IsRevokable = false,
				HasHourValidity = false,
				HasDayValidity = false,
				ApproximateValues = true,
				IsPersonalized = (card.UserId != null),
				IsDamaged = false,
				UserCode = card.UserId,
				UserName = card.UserName,
				UserSurname = "",
				UserDni = card.UserVatNumber,
				UserPhoto = card.UserPhoto,
				LastValidationTypeName = "",
				LastValidationPlace = "",
				LastValidationOperator = "",
				LastValidationTitleName = "",
				LastValidationTitleOwnerName = "",
				DeviceType = EigeFechaPersonalizacion_DispositivoEnum.Tarjeta,
				Groups = card.Groups,
                Entrances = entrances,
                Operations = card.Operations
                    .Select(y => new ServiceCardReadInfoResult_Operation
                    {
                        Id = y.Id,
                        Date = y.Date,
                        Type = y.Type,
                        TypeName = y.Type.ToEnumAlias(),
                        Seq = y.Seq,
                        ESeq = y.ESeq,
                        PurseValues = y.PurseValues
                    }),
                Data = purseValues
                    .Where(x =>
                        (x.ServiceOperation.CardId == card.CardId) &&
                        (x.ServiceOperation.Seq == purseValues
                            .Where(y => y.ServiceOperation.CardId == card.CardId)
                            .Max(y => y.ServiceOperation.Seq)
                        )
                    )
                    .Select(x => new 
                    {
                        Slot =
                            x.Slot == 0 ? SlotEnum.Slot1 :
                            x.Slot == 1 ? SlotEnum.Slot2 :
                            SlotEnum.Slot3,
                        x.Purse.Name,
                        // Balance
                        Balance = x.Amount,
                    })
					.ToList()
					.Select(x => new ServiceCardReadInfoResult
					{
						Slot = x.Slot,
						Name = x.Name,
						OwnerName = "",
						Zone = null,
						Caducity = null,
						IsRechargable = false, // Hay que ver como está esto porque si se pone true hay que poner el RechargeTitle y ponerlo bien
						HasTariff = false,
						IsExhausted = false,
						IsExpired = false,
						// Balance
						HasBalance = true,
						Balance = x.Balance,
						BalanceAcumulated = null,
						BalanceUnits = "€",
						// Temporal
						IsTemporal = false,
						ExhaustedDate = null,
						ActivatedDate = null,
						Ampliation = null,
						AmpliationQuantity = null,
						AmpliationUnits = "",
						// Recharges
						RechargeTitle = null,
						RechargeImpediments = new List<string>(),
						// Others
						ReadAll = false,
						LastRechargeOperationId = null,
						Operation = null,
						MeanTransport = null
					})
			};

			return result;
		}
        #endregion ExecuteAsync

        #region GetEntrancesAsync
        public async Task<IEnumerable<ServiceCardReadInfoResult_Entrance>> GetEntrancesAsync(IQueryable<Entrance> entrances, DateTime now)
        {
            return await Task.Run(() =>
            {
                var result = entrances
                    .Where(x =>
                        (x.State == EntranceState.Active || x.State == EntranceState.Pending) &&
                        x.EntranceType.State == EntranceTypeState.Active &&
                        x.EntranceType.Event.State == EventState.Active //&&
                        //x.EntranceType.CheckInEnd >= now
                    )
                    .OrderBy(x => new
						{
							x.Event.CheckInEnd,
							x.Event.State
						}
					)
                    .Select(x => new ServiceCardReadInfoResult_Entrance
                    {
                        Id = x.Id,
                        Name = x.UserName,
                        LastName = x.LastName,
                        Code = x.Code,
                        EntranceTypeName = x.EntranceType.Name,
                        EventName = x.Event.Name,
						EventStart = x.Event.EventStart,
                        Amount = x.TicketLine.Amount,
						Timestamp = x.Timestamp,
						State = x.State,
						Finished = x.EntranceType.CheckInEnd < now
					});

                return result;
            });
        }
        #endregion GetEntrancesAsync
    }
}

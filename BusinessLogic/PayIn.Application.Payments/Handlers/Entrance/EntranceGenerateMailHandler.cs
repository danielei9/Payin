using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceGenerateMailHandler : 
		IQueryBaseHandler<EntranceGenerateMailArguments, EntranceGenerateMailResult>
	{
		[Dependency] public IEntityRepository<Entrance> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<EntranceGenerateMailResult>> ExecuteAsync (EntranceGenerateMailArguments arguments)
		{
            var entrance = (await Repository.GetAsync())
                .Where(x =>
                    x.State != EntranceState.Deleted &&
                    x.Id == arguments.Id
                )
                .Select(x => new
                {
                    EventName = x.EntranceType.Event.Name,
                    EventLocation = x.EntranceType.Event.Place,
                    EventPhotoUrl = x.EntranceType.Event.PhotoUrl,
                    EventImageUrl = x.EntranceType.Event.EventImages.Where(y => y.EventId == x.EntranceType.EventId).Select(z => z.PhotoUrl).FirstOrDefault(),
                    EventDate = x.EntranceType.Event.EventStart,

                    EntranceTypeName = x.EntranceType.Name,
                    EntrancePrice = x.EntranceType.Price + x.EntranceType.ExtraPrice,
                    EntranceConditions = x.EntranceType.Conditions,
                    EntranceCode = x.Code,
                    EntranceShortDescription = x.EntranceType.ShortDescription,
                    EntranceSellTime = x.Timestamp,

                    UserName = x.UserName +" "+x.LastName ,
                    UserVatnumber = x.VatNumber,

                    ConcessionName = x.EntranceType.Event.PaymentConcession.Concession.Supplier.TaxName,
                    ConcessionVatNumber = x.EntranceType.Event.PaymentConcession.Concession.Supplier.TaxNumber
                }).ToList()
                .Select(x => new EntranceGenerateMailResult
                {
                    EventName = x.EventName,
                    EventLocation = x.EventLocation,
                    EventPhotoUrl = x.EventPhotoUrl,
                    EventDate = (x.EventDate == XpDateTime.MinValue) ? (DateTime?)null : x.EventDate,
                    EventImageUrl= x.EventImageUrl,

                    EntranceTypeName = x.EntranceTypeName,
                    EntrancePrice = x.EntrancePrice,
                    EntranceConditions = x.EntranceConditions,
                    EntranceCode = x.EntranceCode,
                    EntranceShortDescription = x.EntranceShortDescription,
                    EntranceSellTime = (x.EntranceSellTime == XpDateTime.MinValue) ? (DateTime?)null : x.EntranceSellTime,

                    UserName = x.UserName,
                    UserVatnumber = x.UserVatnumber,

                    ConcessionName = x.ConcessionName,
                    ConcessionVatNumber = x.ConcessionVatNumber
                });


            return new ResultBase<EntranceGenerateMailResult> { Data = entrance };
		}
        #endregion ExecuteAsync

        #region ExecuteInternalAsync
        public async Task<ResultBase<EntranceGenerateMailResult>> ExecuteInternalAsync(int id)
        {
            var entrance = (await Repository.GetAsync())
                .Where(x =>
                    x.State != EntranceState.Deleted &&
                    x.Id == id
                )
                .Select(x => new
                {
                    EventName = x.EntranceType.Event.Name,
                    EventLocation = x.EntranceType.Event.Place,
                    EventPhotoUrl = x.EntranceType.Event.PhotoUrl,
                    EventImageUrl = x.EntranceType.Event.EventImages.Where(y => y.EventId == x.EntranceType.EventId).Select(z => z.PhotoUrl).FirstOrDefault(),
                    EventDate = x.EntranceType.Event.EventStart,

                    EntranceTypeName = x.EntranceType.Name,
                    EntrancePrice = x.EntranceType.Price + x.EntranceType.ExtraPrice,
                    EntranceConditions = x.EntranceType.Conditions,
                    EntranceCode = x.Code,
                    EntranceShortDescription = x.EntranceType.ShortDescription,
                    EntranceSellTime = x.Timestamp,

                    UserName = x.UserName + " " + x.LastName,
                    UserVatnumber = x.VatNumber,

                    ConcessionName = x.EntranceType.Event.PaymentConcession.Concession.Supplier.TaxName,
                    ConcessionVatNumber = x.EntranceType.Event.PaymentConcession.Concession.Supplier.TaxNumber
                })
                .ToList()
                .Select(x => new EntranceGenerateMailResult
                {
                    EventName = x.EventName,
                    EventLocation = x.EventLocation,
                    EventPhotoUrl = x.EventPhotoUrl,
                    EventDate = (x.EventDate == XpDateTime.MinValue) ? (DateTime?)null : x.EventDate,
                    EventImageUrl = x.EventImageUrl,

                    EntranceTypeName = x.EntranceTypeName,
                    EntrancePrice = x.EntrancePrice,
                    EntranceConditions = x.EntranceConditions,
                    EntranceCode = x.EntranceCode,
                    EntranceShortDescription = x.EntranceShortDescription,
                    EntranceSellTime = (x.EntranceSellTime == XpDateTime.MinValue) ? (DateTime?)null : x.EntranceSellTime,

                    UserName = x.UserName,
                    UserVatnumber = x.UserVatnumber,

                    ConcessionName = x.ConcessionName,
                    ConcessionVatNumber = x.ConcessionVatNumber
                });
            
            return new ResultBase<EntranceGenerateMailResult> { Data = entrance };
        }
        #endregion ExecuteInternalAsync

    }
}

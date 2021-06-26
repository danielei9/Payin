using PayIn.BusinessLogic.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.Transport;
using PayIn.Application.Dto.Transport.Arguments.TransportTitle;
using PayIn.Application.Dto.Transport.Results.TransportTitle;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportTitleGetHandler :
		IQueryBaseHandler<TransportTitleGetArguments, TransportTitleGetResult>
	{
		private readonly IEntityRepository<TransportTitle> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public TransportTitleGetHandler
		(
			IEntityRepository<TransportTitle> repository, 
			ISessionData sessionData
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportTitleGetResult>> ExecuteAsync(TransportTitleGetArguments arguments)
		{
            var item2 = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .ToList();
            //var item2 = item3
            //.Select(x => new
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    Code = x.Code,
            //    OwnerCode = x.OwnerCode,
            //    OwnerName = x.OwnerName,
            //    Environment = x.Environment,
            //    MaxQuantity = x.MaxQuantity,
            //    Slot = x.Slot,
            //    HasZone = x.HasZone,
            //    OperateByPayIn = x.OperateByPayIn,
            //    IsYoung = x.IsYoung,
            //    IsOverWritable = x.IsOverWritable,
            //    ValidityBit = x.ValidityBit,
            //    TableIndex = x.TableIndex,
            //    MaxExternalChanges = x.MaxExternalChanges,
            //    MaxPeopleChanges = x.MaxPeopleChanges,
            //    ActiveTitle = x.ActiveTitle,
            //    Priority = x.Priority,
            //    TemporalUnit = x.TemporalUnit,
            //    TemporalTypeEnum = x.TemporalType,
            //    Quantity = x.Quantity.Value,
            //    QuantityTypeEnum = x.QuantityType,
            //})
            //    .ToList();

            var item = item2
                .Select(x => new TransportTitleGetResult
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    OwnerCode = x.OwnerCode,
                    OwnerName = x.OwnerName,
                    Environment = x.Environment,

                    OperateByPayIn = x.OperateByPayIn,
                    IsOverWritable = x.IsOverWritable,
                    Slot = x.Slot,
                    HasZone = x.HasZone,

                    TemporalTypeAlias = x.TemporalType?.ToEnumAlias() ?? "",
                    Quantity = x.Quantity,
                    QuantityType = x.QuantityType,
                    QuantityTypeAlias = x.QuantityType?.ToEnumAlias() ?? "",

                    RechargeMin = x.MinCharge,
                    RechargeStep = x.PriceStep,
                    MaxQuantity = x.MaxQuantity,

                    ValidityBit = x.ValidityBit,
                    TableIndex = x.TableIndex,
                    MaxExternalChanges = x.MaxExternalChanges,
                    MaxPeopleChanges = x.MaxPeopleChanges,
                    ActiveTitle = x.ActiveTitle,
                    Priority = x.Priority,
                    TemporalUnit = x.TemporalUnit,
                    TemporalType = x.TemporalType,
				});			

			return new ResultBase<TransportTitleGetResult> { Data = item };
		}
		#endregion ExecuteAsync
	}
}

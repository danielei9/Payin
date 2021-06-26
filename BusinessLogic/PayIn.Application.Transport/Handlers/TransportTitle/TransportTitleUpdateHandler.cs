using PayIn.Application.Dto.Transport.Arguments.TransportTitle;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportTitleUpdateHandler :
		IServiceBaseHandler<TransportTitleUpdateArguments>
	{
		private readonly IEntityRepository<TransportTitle> Repository;
		
		#region Contructors
		public TransportTitleUpdateHandler(
			IEntityRepository<TransportTitle> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

		}
		#endregion Contructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportTitleUpdateArguments>.ExecuteAsync(TransportTitleUpdateArguments arguments)
		{
			var transportTitle = await Repository.GetAsync(arguments.Id);

			transportTitle.Name = arguments.Name;
            transportTitle.Code = arguments.Code;
            transportTitle.OwnerCode = arguments.OwnerCode;
            transportTitle.OwnerName = arguments.OwnerName;
            transportTitle.Environment = arguments.Environment;
            transportTitle.MinCharge = arguments.RechargeMin;
            transportTitle.PriceStep = arguments.RechargeStep;
            transportTitle.MaxQuantity = arguments.MaxQuantity;
            transportTitle.Slot = arguments.Slot;
            transportTitle.HasZone = arguments.HasZone;
            transportTitle.OperateByPayIn = arguments.OperateByPayIn;
			transportTitle.IsYoung = false;
			transportTitle.IsOverWritable = arguments.IsOverWritable;

			transportTitle.ValidityBit = arguments.ValidityBit;
			transportTitle.TableIndex = arguments.TableIndex;
			transportTitle.MaxExternalChanges = arguments.MaxExternalChanges;
			transportTitle.MaxPeopleChanges = arguments.MaxPeopleChanges;
			transportTitle.ActiveTitle = arguments.ActiveTitle;
			transportTitle.Priority = arguments.Priority;

			transportTitle.TemporalUnit = arguments.TemporalUnit;
			transportTitle.TemporalType = arguments.TemporalType;
            transportTitle.Quantity = arguments.Quantity;
            transportTitle.QuantityType = arguments.QuantityType;

			return transportTitle;
		}
		#endregion ExecuteAsync
	}
}

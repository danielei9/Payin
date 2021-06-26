using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.Application.Transport.Services;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Application.Results;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	[XpLog("TransportOperation", "Recharge2", RelatedId = "OperationId")]
	[XpAnalytics("TransportOperation", "Recharge2")]
	public class TransportOperationRecharge2Handler : TransportOperationInternal,
		IServiceBaseHandler<TransportOperationRecharge2Arguments>
	{
		protected readonly TransportOperationRechargeHandler Handler;

		#region Constructors
		public TransportOperationRecharge2Handler(
			EigeService eigeService,
			IEntityRepository<TransportPrice> priceRepository,
			IEntityRepository<BlackList> blackListRepository,
			IEntityRepository<GreyList> greyListRepository,
			TransportOperationRechargeHandler handler
		)
			: base(eigeService, priceRepository, blackListRepository, greyListRepository)
		{
			if (handler == null) throw new ArgumentNullException("handler");
			Handler = handler;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportOperationRecharge2Arguments arguments)
		{
			var argumentsRecharge = new TransportOperationRechargeArguments(
				arguments.Operation.Card.Type,	
				arguments.Operation.Card.Uid.ToHexadecimalBE(),
				arguments.Operation.Card.CardSystem,
				arguments.Script,
				arguments.Code,
				arguments.Quantity,
				arguments.PriceId,
				arguments.TicketId,
				arguments.RechargeType,
				arguments.Promotions,
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				""
#if DEBUG || TEST || HOMO
				, arguments.Now
#endif
			);
			var resultRecharge = await Handler.ExecuteAsync(argumentsRecharge) as ScriptResultBase<TransportOperationGetReadInfoResult>;

			return new Script2ResultBase<Script2Result>
			{
				Data = resultRecharge.Scripts
					.Select(x => new Script2Result
					{
						Keys = x.Keys,
						Script = x.Script
					}),
				//CardData = new CardData(resultRecharge),
				Operation = new Dto.Transport.OperationInfo
				{
					Id = resultRecharge.Operation.Id,
					Card = new Dto.Transport.CardInfo
					{
						Uid = resultRecharge.Operation.Card.Uid,
						CardSystem = arguments.Operation.Card.CardSystem
					},
					IsRead = false,
					Type = resultRecharge.Operation.Type
				}
			};
		}
#endregion ExecuteAsync
	}
}
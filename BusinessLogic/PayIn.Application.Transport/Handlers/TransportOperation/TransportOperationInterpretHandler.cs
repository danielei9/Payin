using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Application.Results;

namespace PayIn.Application.Transport.Handlers
{
	[XpLog("TransportOperation", "Interpret", RelatedId = "OperationId")]
	[XpAnalytics("TransportOperation", "Interpret", Response = new[] { "Data[0].Code", "Data[0].Name", "Data[1].Code", "Data[1].Name" })]
	public class TransportOperationInterpretHandler :
		IQueryBaseHandler<TransportOperationInterpretArguments, Script2Result>
	{
		private const string METHOD_NAME = "TransportOperation_ReadInfo";

		private readonly TransportOperationConfirmAndReadInfoHandler Handler;

		#region Constructors
		public TransportOperationInterpretHandler(
            TransportOperationConfirmAndReadInfoHandler handler
		)
		{
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            Handler = handler;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<Script2Result>> ExecuteAsync(TransportOperationInterpretArguments arguments)
		{
            var argumentsReadInfo = new TransportOperationConfirmAndReadInfoArguments(
                arguments.Operation.Card.Uid.ToHexadecimalBE(),
                arguments.Operation.Card.CardSystem,
                arguments.Script,
                ""
#if DEBUG || TEST || HOMO
                , arguments.Now
#endif
            );
            argumentsReadInfo.OperationId = arguments.Operation.Id;
            var resultReadInfo = await Handler.ExecuteAsync(argumentsReadInfo) as ServiceCardReadInfoResultBase;

            return new Script2ResultBase<Script2Result>
            {
                Data = resultReadInfo.Scripts
                    .Select(x => new Script2Result
                    {
                        Keys = x.Keys,
                        Script = x.Script
                    }),
                CardData = new CardData(resultReadInfo),
                Operation = arguments.Operation
            };
        }
        #endregion ExecuteAsync
    }
}

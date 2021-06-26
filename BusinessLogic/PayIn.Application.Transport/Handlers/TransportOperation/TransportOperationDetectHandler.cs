using PayIn.Application.Dto.Transport;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.Application.Transport.Scripts;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Application.Handlers;
using Xp.Application.Results;
using Xp.Domain;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;
using Xp.Domain.Transport.MifareClassic.Operaons;

namespace PayIn.Application.Transport.Handlers
{
    [XpLog("TransportOperation", "Detect", RelatedId = "OperationId")]
	[XpAnalytics("TransportOperation", "Detect")]
	public class TransportOperationDetectHandler :
		ScriptHandler<TransportOperationDetectArguments, Script2Result>
	{
		private const string METHOD_NAME = "TransportOperation_Detect";

		private readonly ISessionData SessionData;
		private readonly IMifareClassicHsmService Hsm;
		private readonly IEntityRepository<TransportOperation> TransportOperationRepository;
		private readonly IEntityRepository<Log> LogRepository;
		private readonly IUnitOfWork UnitOfWork;

		#region Constructors
		public TransportOperationDetectHandler(
			ISessionData sessionData,
			IMifareClassicHsmService hsm,			
			IEntityRepository<Log> logRepository,
			IEntityRepository<TransportOperation> transportOperationRepository,
			IUnitOfWork unitOfWork
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (hsm == null) throw new ArgumentNullException("hsm");
			if (logRepository == null) throw new ArgumentNullException("logRepository");
			if (transportOperationRepository == null) throw new ArgumentNullException("transportOperationRepository");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			
			SessionData = sessionData;
			Hsm = hsm;
			LogRepository = logRepository;
			TransportOperationRepository = transportOperationRepository;
			UnitOfWork = unitOfWork;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async override Task<ResultBase<Script2Result>> ExecuteAsync(TransportOperationDetectArguments arguments)
		{
#if DEBUG
			var watch = Stopwatch.StartNew();
#endif
			var now = DateTime.Now;

			var script = new TransportCardGetReadInfoScript(SessionData.Login, Hsm);

            #region Leer toda la tarjeta
            for (var i = 0; i <= 15; i++)
				for (var j = 0; j <= 2; j++)
					script.Request.Add(new MifareReadOperation()
					{
						Sector = (byte)i,
						Block = (byte)j,
						Operation = MifareOperationType.ReadBlock
					});
            #endregion Leer toda la tarjeta

            #region Obtener script
            var resultScript = await script.GetRequestAsync();
            var mobilisKeys = !arguments.NeedKeys ? "" : await script.GetKeysEncryptedAsync(Hsm, CardSystem.Mobilis, resultScript, arguments.Card.Uid, script.Card.TituloTuiN.VersionClaves.Value);
            var alicanteKeys = !arguments.NeedKeys ? "" : await script.GetKeysEncryptedAsync(Hsm, CardSystem.FgvAlicante, resultScript, arguments.Card.Uid, script.Card.TituloTuiN.VersionClaves.Value);
            #endregion Obtener script

            // Crear operation
            var operation = new TransportOperation
			{
				OperationDate = now.ToUTC(),
				OperationType = OperationType.Read,
				Uid = arguments.Card.Uid,
				Login = SessionData.Login,
				Device = arguments.Device.ToJson()
			};
			await TransportOperationRepository.AddAsync(operation);
			await UnitOfWork.SaveAsync();
			arguments.OperationId = operation.Id;

            return new Script2ResultBase<Script2Result> {
                Data = new[] {
                    new Script2Result
                    {
                        Condition = await MifareAutenticateOperation.CreateAsync(1, MifareKeyType.A),
                        CardSystem = CardSystem.Mobilis,
                        Script = resultScript,
                        Keys = mobilisKeys
                    },
                    new Script2Result
                    {
                        Condition = await MifareAutenticateOperation.CreateAsync(1, MifareKeyType.A),
                        CardSystem = CardSystem.FgvAlicante,
                        Script = resultScript,
                        Keys = alicanteKeys
                    }
                },
                Operation = new OperationInfo
                {
                    Id = operation.Id,
                    Card = arguments.Card,
                    Type = operation.OperationType,
                    IsRead = true
                }
			};
		}
		#endregion ExecuteAsync
	}
}

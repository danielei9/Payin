using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.Application.Transport.Scripts;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Application.Handlers;
using Xp.Application.Results;
using Xp.Domain;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Handlers
{
	[XpLog("TransportOperation", "GetReadInfo", RelatedId = "OperationId")]
	[XpAnalytics("TransportOperation", "GetReadInfo")]
	public class TransportOperationGetReadInfoHandler :
		ScriptHandler<TransportOperationGetReadInfoArguments, TransportOperationGetReadInfoResult>
	{
		private const string METHOD_NAME = "TransportOperation_GetReadInfo";

		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IMifareClassicHsmService Hsm { get; set; }
		[Dependency] public IEntityRepository<TransportOperation> TransportOperationRepository { get; set; }
		[Dependency] public IEntityRepository<Log> LogRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async override Task<ResultBase<TransportOperationGetReadInfoResult>> ExecuteAsync(TransportOperationGetReadInfoArguments arguments)
		{
#if DEBUG
			var watch = Stopwatch.StartNew();
#endif
			var now = DateTime.Now;

			var serviceCard = (await ServiceCardRepository.GetAsync())
				.Where(x =>
					x.Uid == arguments.Uids &&
					(
						x.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login ||
						x.SystemCard.ConcessionOwner.Supplier.Workers.Any(y => y.Login == SessionData.Login)
					)
				)
				.FirstOrDefault();

			IEnumerable<ScriptResult> result = null;
			if ((serviceCard == null) || (serviceCard.OwnerUserId != null)) // Emited
			{
				var script = new TransportCardGetReadInfoScript(SessionData.Login, Hsm);
				var cardSystem = CardSystem.Mobilis;

				// Leer toda la tarjeta
				for (var i = 0; i <= 15; i++)
					for (var j = 0; j <= 2; j++)
						script.Request.Add(new MifareReadOperation()
						{
							Sector = (byte)i,
							Block = (byte)j,
							Operation = MifareOperationType.ReadBlock
						});

#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Leer toda la tarjeta: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif

				#region Obtener script
				var resultScript = await script.GetRequestAsync();
				var resultKeys = arguments.NeedKeys ? await script.GetKeysEncryptedAsync(Hsm, cardSystem, resultScript, arguments.Uids ?? 0, script.Card.TituloTuiN.VersionClaves.Value) : "";
				result = arguments.Cards
					.Select(x => new ScriptResult
					{
						Card = new ScriptResult.CardId
						{
							Uid = x.Uid,
							Type = x.Type,
							System = cardSystem
						},
						Script = resultScript,
						Keys = resultKeys
					});
#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Obtener script: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Obtener script
			}

			// Crear operation
			var operation = new TransportOperation
			{
				OperationDate = now.ToUTC(),
				OperationType = OperationType.Read,
				Uid = arguments.Uids,
				Login = SessionData.Login,
				Device = arguments.Device
			};
			await TransportOperationRepository.AddAsync(operation);
			await UnitOfWork.SaveAsync();
			arguments.OperationId = operation.Id;

#if DEBUG
			watch.Stop();
			Debug.WriteLine(METHOD_NAME + " - Crear operation: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
			watch = Stopwatch.StartNew();
#endif
			
			return new ScriptResultBase<TransportOperationGetReadInfoResult> {
				Scripts = result,
				Operation = new ScriptResultBase<TransportOperationGetReadInfoResult>.ScriptResultOperation
				{
					Type = operation.OperationType,
					Uid = operation.Uid,
					Id = operation.Id
				},
				Device = arguments.Device
			};
		}
		#endregion ExecuteAsync
	}
}

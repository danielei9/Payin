using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("EntranceCheck", "CreateQr")]
	[XpAnalytics("EntranceCheck", "CreateQr")]
	public class MobileEntranceCheckCreateQrHandler :
		IServiceBaseHandler<MobileEntranceCheckCreateQrArguments>
	{
		[Dependency] public IEntityRepository<EntranceSystem> EntranceSystemRepository { get; set; }
		[Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public MobileEntranceCheckCreateTextHandler EntranceCheckMobileCreateTextHandler { get; set; }

		public class Codes
		{
			public long? EventCode { get; set; }
			public long EntranceCode { get; set; }

			public EntranceSystem EntranceSystem { get; set; }
			public Event Event { get; set; }
			public EntranceType EntranceType { get; set; }
			public Entrance Entrance { get; set; }
		}

		#region GetEntranceInfoFromCodeQrAsync
		public async Task<Codes> GetEntranceInfoFromCodeQrAsync(string code, int? eventId = null)
		{
			// Get entrance system
			var entranceSystems = (await EntranceSystemRepository.GetAsync("Events"))
				.Where(x =>
					x.PaymentConcessions
						.Any(y =>
							y.Concession.Supplier.Login == SessionData.Login ||
							y.PaymentWorkers.Any(z => z.Login == SessionData.Login)
						)
				)
				.ToList();

			// Check codes
			var defaultCode = (Codes)null;
			foreach (var entranceSystem in entranceSystems)
			{
				var codes = entranceSystem.GetEntranceCodeQr(code, null);
				if (codes == null)
					continue;

				var entrance = (await EntranceRepository.GetAsync("Event", "EntranceType", "Checks"))
					.Where(x =>
						(
							(x.State == EntranceState.Active) ||
							(x.State == EntranceState.Validated)
						) &&
						(x.Code == codes.EntranceCode) &&
						(x.Event.State == EventState.Active) &&
						((eventId == null) || (x.EventId == eventId)) &&
						(
							(codes.EventCode == null) ||
							(x.Event.Code == codes.EventCode)
						)
					)
					.FirstOrDefault();
				defaultCode = new Codes
				{
					EventCode = codes.EventCode,
					EntranceCode = codes.EntranceCode
				};

				// Entrada no encontrada
				if (entrance == null)
					continue;

				defaultCode.EntranceSystem = entranceSystem;
				defaultCode.Event = entrance.Event;
				defaultCode.EntranceType = entrance.EntranceType;
				defaultCode.Entrance = entrance;
				break;
			}

			return defaultCode;
		}
		#endregion GetEntranceInfoFromCodeQrAsync

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileEntranceCheckCreateQrArguments arguments)
		{
			var now = DateTime.UtcNow;

			// Obtener codes
			var codes = await GetEntranceInfoFromCodeQrAsync(arguments.Code, arguments.EventId);
			if (codes == null)
				throw new ApplicationException(EntranceCheckResources.NoCodeException);
			if (codes.Event == null)
				throw new ApplicationException(EntranceCheckResources.NoEventException);
			if (codes.Entrance == null)
				throw new ApplicationException(EntranceCheckResources.NoEntranceException);

			return await EntranceCheckMobileCreateTextHandler.ValidateAsync(
				codes.Entrance,
				codes.EntranceCode,
				arguments.Force,
				arguments.Observations,
				arguments.Type,
				codes.Event
			);
		}
		#endregion ExecuteAsync
	}
}

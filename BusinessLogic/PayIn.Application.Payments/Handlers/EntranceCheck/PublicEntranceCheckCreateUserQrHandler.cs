using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("EntranceCheck", "CreateUserQr")]
	[XpAnalytics("EntranceCheck", "CreateUserQr")]
	public class PublicEntranceCheckCreateUserQrHandler :
		IServiceBaseHandler<PublicEntranceCheckCreateUserQrArguments>
	{
		[Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
		[Dependency] public MobileEntranceCheckCreateTextHandler EntranceCheckMobileCreateTextHandler { get; set; }
		[Dependency] public MobileEntranceCheckCreateQrHandler MobileEntranceCheckCreateQrHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PublicEntranceCheckCreateUserQrArguments arguments)
		{
			var now = DateTime.UtcNow;

			// Obtener codes
			var codes = await MobileEntranceCheckCreateQrHandler.GetEntranceInfoFromCodeQrAsync(arguments.Code);
			if (codes.Event == null)
				throw new ApplicationException(EntranceCheckResources.NoEventException);

			// Crear entrada si no existe
			if (codes.Entrance == null)
			{
				// Crear entrada
				var entrance = new Entrance(
					code: codes.EntranceCode,
					state: EntranceState.Active,
					sendingCount: 0,
					now: now,
					vatNumber: arguments.VatNumber,
					userName: arguments.Name,
					lastName: arguments.LastName,
					uid: null,
					email: arguments.Email,
					login: "",
					entranceTypeId: null,
					eventId: codes.Event.Id
				);
				await EntranceRepository.AddAsync(entrance);
			}

			return await EntranceCheckMobileCreateTextHandler.ValidateAsync(
				codes.Entrance,
				codes.EntranceCode,
				arguments.Force,
				"",
				arguments.Type,
				codes.Event
			);
		}
		#endregion ExecuteAsync
	}
}

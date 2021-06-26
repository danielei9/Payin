using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileEntranceLoadQrHandler :
		IServiceBaseHandler<MobileEntranceLoadQrArguments>
	{
		[Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public MobileEntranceCheckCreateQrHandler MobileEntranceCheckCreateQrHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileEntranceLoadQrArguments arguments)
		{
			var now = DateTime.UtcNow;

			// Obtener codes
			var codes = await MobileEntranceCheckCreateQrHandler.GetEntranceInfoFromCodeQrAsync(arguments.Code);
			if (codes == null)
				throw new ApplicationException("Formato de la entrada no correcto");
			if (codes.Event == null)
				throw new ApplicationException(EntranceCheckResources.NoEventException);

			if (codes.Entrance == null)
			{
				// Crear entrada si no existe
				codes.Entrance = new Entrance(
					code: codes.EntranceCode,
					state: EntranceState.Active,
					sendingCount: 0,
					now: now,
					vatNumber: SessionData.TaxNumber,
					userName: SessionData.Name,
					lastName: "",
					uid: null,
					email: SessionData.Email,
					login: SessionData.Login,
					entranceTypeId: null,
					eventId: codes.Event.Id
				);
				await EntranceRepository.AddAsync(codes.Entrance);
			}
			else if (codes.Entrance.Login.IsNullOrEmpty())
				// Asignar entrada si no está asignada
				codes.Entrance.Login = SessionData.Login;
			else
				throw new ApplicationException("Entrada ya asignada a un usuario.");

			return codes.Entrance;
		}
		#endregion ExecuteAsync
	}
}

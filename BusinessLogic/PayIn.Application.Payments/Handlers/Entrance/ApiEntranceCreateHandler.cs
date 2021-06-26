using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ApiEntranceCreateHandler :
		IServiceBaseHandler<ApiEntranceCreateArguments>
	{
		[Dependency] public IEntityRepository<Entrance> Repository { get; set; }
		[Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ApiEntranceCreateArguments arguments)
		{
			// Get entranceType
			var entranceType = (await EntranceTypeRepository.GetAsync())
				.Where(x =>
					x.Id == arguments.EntranceTypeId &&
					x.State == EntranceTypeState.Active
				)
				.FirstOrDefault();
			if (entranceType == null)
				throw new ArgumentNullException("EntranceTypeId");

			var entrances = await CreateEntranceAsync(
				entranceType, null,
				arguments.Quantity, arguments.Quantity, arguments.Amount,
				arguments.TaxNumber, arguments.TaxName, arguments.Email, arguments.Login,
				arguments.Uid, arguments.Payed, arguments.Now,
				null
			);

			return entrances;
		}
		#endregion ExecuteAsync

		#region CreateEntranceAsync
		public async Task<IEnumerable<Entrance>> CreateEntranceAsync(
			EntranceType entranceType, TicketLine line,
			decimal quantity, decimal entrancesToBuyOfEvent, decimal amount,
			string taxNumber, string taxName, string email, string login,
			long? uid, bool payed, DateTime now,
			List<ControlFormValue>  values
		)
		{
			var entrances = new List<Entrance>();

			// Check entrace count
			var countEntrances = (await Repository.GetAsync())
				.Where(x =>
					x.EntranceTypeId == entranceType.Id &&
					x.State == EntranceState.Active
				)
				.Count();
			if (countEntrances + quantity > entranceType.MaxEntrance)
				// You can't take {0} '{1}' entrances.You exceed maximum number of available entrances. You can only buy {2} entrances of this type.
				throw new ApplicationException(EntranceResources.MaxEntrancesExceedException.FormatString(
					countEntrances,
					entranceType.Name,
					entranceType.MaxEntrance
				));

			// Comprobar limitación de número de entradas por pulsera, solo si no es una recarga de saldo
			if (uid != null) // && !isRecharge)
			{
				var entranceTypeErrors = (await EntranceTypeRepository.GetAsync())
				.Where(x =>
					x.Id == entranceType.Id
				)
				.Select(x => new
				{
					EventName = x.Event.Name,
					x.Event.MaxEntrancesPerCard,
					EntranceCount = x.Event.Entrances
						.Where(y =>
							(
								y.State == EntranceState.Active // ||
								// y.State == EntranceState.Pending
							) && y.Uid == uid
						)
					.Count()
				})
				.Where(x =>
					x.EntranceCount + entrancesToBuyOfEvent > x.MaxEntrancesPerCard
				);

				var entranceTypeErrorsList = entranceTypeErrors.ToList();

				foreach (var entranceTypeError in entranceTypeErrorsList)
					throw new ArgumentException($"El número máximo de entradas por tarjeta para evento {entranceTypeError.EventName} es de {entranceTypeError.MaxEntrancesPerCard} y se han intentado comprar {entrancesToBuyOfEvent} más a las {entranceTypeError.EntranceCount} ya compradas.");
			}		

			// Crear entrances
			for (int i = 0; i < quantity; i++)
			{
				var code = await GenerateCodeAsync(entrances, entranceType, i);

				var entrance = new Entrance(
					code: code,
					state: (payed) ? EntranceState.Active : EntranceState.Pending,
					sendingCount: 0,
					now: now,
					vatNumber: taxNumber,
					userName: taxName,
					lastName: "",
					uid: uid,
					email: email,
					login: login,
					entranceType: entranceType,
					ticketLine: line,
					formValues: null
				);
				
				await Repository.AddAsync(entrance);

				entrances.Add(entrance);
			}
			
			return entrances;
		}
		#endregion CreateEntranceAsync

		#region GenerateCodeAsync
		private static Random RandomGenerator = new Random();
		public async Task<long> GenerateCodeAsync(IEnumerable<Entrance> entrances, EntranceType entranceType, int countEntrances)
		{
			if (entranceType.RangeMin != null)
			{
				// Rango
				var previousCode = (await Repository.GetAsync())
					 .Where(x =>
						 x.EntranceTypeId == entranceType.Id &&
						 x.State == EntranceState.Active
					 )
					 .Select(x => (long?)x.Code)
					 .Max() ?? (entranceType.RangeMin.Value - 1);
				if (previousCode + 1 + countEntrances > entranceType.RangeMax)
					throw new ApplicationException("Rango completo!!!");

				return previousCode + 1 + countEntrances;
			}

			// Aleatorio
			var code = RandomGenerator.Next(0, int.MaxValue);
			while (
				(
					entrances.Any(x => x.Code == code)
				) || (
					(await Repository.GetAsync())
						.Any(x =>
							x.EntranceTypeId == x.EntranceType.Id &&
							x.Code == code
						)
				)
			)
			{
				code = RandomGenerator.Next(0, int.MaxValue);
			}

			return code;
		}
		#endregion GenerateCodeAsync
	}
}

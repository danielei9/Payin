using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceSyncronizeHandler :
		IServiceBaseHandler<EntranceTypeSyncronizeArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Event> RepositoryEvent { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeSyncronizeArguments arguments)
		{
			var now = DateTime.UtcNow;

			var _event = (await RepositoryEvent.GetAsync("EntranceTypes"))
				.Where(x => 
					(x.Id == arguments.EventId) &&
					(x.PaymentConcession.Concession.State == ConcessionState.Active) &&
					(
						(
							(x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) &&
							(x.PaymentConcession.Concession.State == ConcessionState.Active)
						) || (
							(x.PaymentConcession.PaymentWorkers
								.Any(y =>
									(y.State == WorkerState.Active) &&
									(y.Login == SessionData.Login)
								)
							)
						)
					) && 
					(
						(x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
						(x.PaymentConcession.PaymentWorkers
							.Any(y => y.Login == SessionData.Login))
					) &&
					(x.State != EventState.Deleted) &&
					(x.PaymentConcession.Concession.State == ConcessionState.Active)
				)
				.FirstOrDefault();
			if (_event == null)
				throw new ApplicationException("No existe identificador de evento");

			// Recorrer los tipos de entrada de la BD
			// - Si no está en arguments.entranceTypes lo elimino
			foreach (var entranceType in _event.EntranceTypes)
				if (!arguments.EntranceTypes.Any(x => x.Code == entranceType.Code))
					entranceType.State = EntranceTypeState.Deleted;

					// Recorrer arguments.entranceTypes:
					// - Si está en BD lo modifico
					// - Si no en BD está lo creo
			foreach (var entranceTypeArgument in arguments.EntranceTypes)
			{
				var entranceType = _event.EntranceTypes
					.Where(x =>
						x.Code == entranceTypeArgument.Code &&
						x.State == EntranceTypeState.Active
					)
					.FirstOrDefault();

				if (entranceType != null)
				{
					entranceType.Name = entranceTypeArgument.Name;
					entranceType.Price = entranceTypeArgument.Price;
					entranceType.MaxEntrance = entranceTypeArgument.MaxEntrance ?? int.MaxValue;
					entranceType.SellStart = entranceTypeArgument.SellStart;
					entranceType.SellEnd = entranceTypeArgument.SellEnd;
					entranceType.CheckInStart = entranceTypeArgument.CheckInStart;
					entranceType.CheckInEnd = entranceTypeArgument.CheckInEnd;
					entranceType.ExtraPrice = entranceTypeArgument.ExtraPrice;
					entranceType.NumDay = entranceTypeArgument.NumDays;
					entranceType.StartDay = entranceTypeArgument.StartDay;
					entranceType.EndDay = entranceTypeArgument.EndDay;
					entranceType.IsVisible = entranceTypeArgument.IsVisible;
					entranceType.Visibility = entranceTypeArgument.Visibility;
				}
				else
				{
					entranceType = new EntranceType
					{
						Code = entranceTypeArgument.Code,
						Name = entranceTypeArgument.Name,
						Description = "",
						Price = entranceTypeArgument.Price,
						MaxEntrance = entranceTypeArgument.MaxEntrance ?? int.MaxValue,
						PhotoUrl = "",
						SellStart = entranceTypeArgument.SellStart,
						SellEnd = entranceTypeArgument.SellEnd,
						CheckInStart = entranceTypeArgument.CheckInStart,
						CheckInEnd = entranceTypeArgument.CheckInEnd,
						ExtraPrice = entranceTypeArgument.ExtraPrice,
						NumDay = entranceTypeArgument.NumDays,
						StartDay = entranceTypeArgument.StartDay,
						EndDay = entranceTypeArgument.EndDay,
						State = EntranceTypeState.Active,
						IsVisible = entranceTypeArgument.IsVisible,
						MaxSendingCount = 0,
						ShortDescription = "",
						Conditions = "",
						Visibility = entranceTypeArgument.Visibility,
						EventId = arguments.EventId

					};
					_event.EntranceTypes.Add(entranceType);
				}
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}

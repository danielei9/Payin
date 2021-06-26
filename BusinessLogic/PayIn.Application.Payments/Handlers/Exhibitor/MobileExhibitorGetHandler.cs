using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Exhibitor", "MobileGet")]
	public class MobileExhibitorGetHandler :
		IQueryBaseHandler<ExhibitorMobileGetArguments, ExhibitorMobileGetResult>
	{
		[Dependency] public IEntityRepository<Exhibitor> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ExhibitorMobileGetResult>> ExecuteAsync(ExhibitorMobileGetArguments arguments)
		{
			var now = DateTime.UtcNow;

			var notifications = (await ServiceNotificationRepository.GetAsync());

			var items = (await Repository.GetAsync())
				.Where(x =>
					x.Id == arguments.Id &&
					x.State == ExhibitorState.Active
				)
				.Select(x => new {
					Id = x.Id,
					Name = x.Name,
					Address = x.Address,
					Phone = x.Phone,
					Email = x.Email,
					ConcessionId = x.PaymentConcession.Concession.Id,
					ConcessionLogin = x.PaymentConcession.Concession.Supplier.Login,
					Contact = x.Contacts
						.FirstOrDefault(y =>
							y.VisitorLogin == SessionData.Login
						),
					Products = (
						x.PaymentConcession.Products
							.Where(y =>
								y.State == ProductState.Active &&
								y.IsVisible
							)
							.Select(y => new ExhibitorMobileGetResult_Product
							{
								Id = y.Id,
								Description = y.Description,
								FamilyId = y.FamilyId,
								PhotoUrl = y.PhotoUrl,
								Name = y.Name,
								Price = y.Price,
								Type = ExhibitorMobileGetResult_ProductTypeEnum.Product
							})
					).Union(
						x.PaymentConcession.Families
							.Where(y =>
								y.State == ProductFamilyState.Active &&
								y.IsVisible
							)
							.Select(y => new ExhibitorMobileGetResult_Product
							{
								Id = y.Id,
								Description = y.Description,
								FamilyId = y.SuperFamilyId,
								PhotoUrl = y.PhotoUrl,
								Name = y.Name,
								Price = null, //y.Price,
								Type = ExhibitorMobileGetResult_ProductTypeEnum.Family
							})
					),
					Events = x.Events
						.Where(y =>
							(y.IsVisible) &&
							(y.EventEnd > now) &&
							(y.State == EventState.Active) &&
							(
								(y.Visibility != EventVisibility.Members) ||
								(x.PaymentConcession.Concession.ServiceUsers
									.Where(z => z.Login == SessionData.Login)
									.Any()
								)
							)
						)
						.OrderBy(y => y.EventStart)
						.Select(y => new
						{
							Id = y.Id,
							Name = y.Name,
							Place = y.Place,
							PhotoUrl = y.PhotoUrl,
							Description = y.Description,
							EventStart = y.EventStart,
							EventEnd = y.EventEnd,
							MinPrice = y.EntranceTypes
								.Select(z => (decimal?)z.Price)
								.Min() ?? 0,
							MaxPrice = y.EntranceTypes
								.Select(z => (decimal?)z.Price)
								.Max() ?? 0
						}),
					Notifications = notifications
						.Where(y =>
							(
								(y.State == NotificationState.Actived) ||
								(y.State == NotificationState.Read)
							) && (
								(
									(y.SenderLogin == SessionData.Login) &&
									(y.ReceiverLogin == x.PaymentConcession.Concession.Supplier.Login)
								) || (
									(y.SenderLogin == x.PaymentConcession.Concession.Supplier.Login) &&
									(y.ReceiverLogin == SessionData.Login)
								)
							) &&
							(y.Message != null) &&
							(y.Message != "") &&
							(y.Type == NotificationType.ChatSend)
						)
						.OrderByDescending(y => y.CreatedAt)
						.Select(y => new
						{
							y.Id,
							y.Message,
							y.SenderLogin,
							y.ReceiverLogin,
							y.State,
							y.CreatedAt
						})
				})
				.ToList();
			var items2 = items
				.Select(x => new ExhibitorMobileGetResult
				{
					Id = x.Id,
					Name = x.Name,
					Address = x.Address,
					Phone = x.Phone,
					Email = x.Email,
					ContactState = x.Contact?.State,
					ConcessionId = x.ConcessionId,
					ConcessionLogin = x.ConcessionLogin ?? "",
					Products = x.Products,
					Events = x.Events
						.Select(y => new ExhibitorMobileGetResult_Event {
							Id = y.Id,
							Name = y.Name,
							Place = y.Place,
							PhotoUrl = y.PhotoUrl,
							Description = y.Description,
							EventStart = y.EventStart.ToUTC(),
							EventEnd = y.EventEnd.ToUTC(),
							MinPrice = y.MinPrice,
							MaxPrice = y.MaxPrice
						}),
					Notifications = x.Notifications
						.ToList()
						.Select(y => new ExhibitorMobileGetResult_Notification {
							Id = y.Id,
							Message = y.Message,
							SenderLogin = y.SenderLogin,
							ReceiverLogin = y.ReceiverLogin,
							State = y.State,
							CreatedAt = y.CreatedAt.ToUTC()
						})
				})
				.ToList();

			return new ResultBase<ExhibitorMobileGetResult> {
				Data = items2
			};
		}
		#endregion ExecuteAsync
	}
}

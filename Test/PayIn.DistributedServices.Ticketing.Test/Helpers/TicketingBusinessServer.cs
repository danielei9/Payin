using PayIn.Common;
using PayIn.DistributedServices.Common.Test;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xp.Common;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Dto.Internal.Results;
using PayIn.Application.Dto.Arguments;

namespace PayIn.DistributedServices.Test.Helpers
{
	public class TicketingBusinessServer : BaseServer, IDisposable
	{
		#region CampaignsGetAllAsync
		public async Task<IEnumerable<PublicCampaignGetByUserResult>> CampaignsGetByUserAsync(string login, int eventId)
		{
            var now = DateTime.UtcNow;

            using (var server = new HttpServer())
			{
				var items = await server.GetAsync<ResultBase<PublicCampaignGetByUserResult>>(
					"/public/campaign/v1/user",
					new PublicCampaignGetByUserArguments(login, eventId, "", now)
				);
				return items.Data;
			}
		}
		#endregion CampaignsGetAllAsync

		#region EntranceGetByUserAsync
		public async Task<IEnumerable<MobilePaymentMediaGetAllResult_Entrance>> EntranceGetByUserAsync(string login)
		{
			using (var server = new HttpServer())
			{
				var items = await server.GetAsync<ResultBase<MobilePaymentMediaGetAllResult_Entrance>>(
					"/public/entrance/v1/User",
					new PublicEntranceGetByUserArguments(login)
				);
				return items.Data;
			}
		}
		#endregion EntranceGetByUserAsync

		#region EntranceTypeCreateAsync
		public async Task<int> EntranceTypeCreateAsync(int eventId, string name, string description,
			decimal price, int? maxEntrance, XpDateTime sellStart, XpDateTime sellEnd,
			XpDateTime checkInStart, XpDateTime checkInEnd, decimal? extraPrice,
			int? rangeMin, int? rangeMax, int maxSendingCount, string shortDescription, string conditions,
			int? numDays, XpTime startDay, XpTime endDay)
		{
			using (var server = new HttpServer())
			{
				var items = await server.PostAsync<IdResult>(
					"/public/entrancetype/v1",
					eventId,
					new
					{
						Name = name,
						Description = description,
						Price = price,
						MaxEntrance = maxEntrance,
						SellStart = sellStart,
						SellEnd = sellEnd,
						CheckInStart = checkInStart,
						CheckInEnd = checkInEnd,
						ExtraPrice = extraPrice,
						RangeMin = rangeMin,
						RangeMax = rangeMax,
						MaxSendingCount = maxSendingCount,
						ShortDescription = shortDescription,
						Conditions = conditions,
						NumDays = numDays,
						StartDay = startDay,
						EndDay = endDay
					}
				);
				return items.Id;
			}
		}
		#endregion EntranceTypeCreateAsync

		#region EntranceTypeDeleteAsync
		public async Task EntranceTypeDeleteAsync(int id)
		{
			using (var server = new HttpServer())
			{
				await server.DeleteAsync(
					"/public/entrancetype/v1",
					id
				);
			}
		}
		#endregion EntranceTypeDeleteAsync

		#region EntranceTypeGetAsync
		public class EntranceTypeGetResult
		{
			public int Id { get; set; }
			public bool IsVisible { get; set; }
			public string Name { get; set; }
			public EntranceTypeState State { get; set; }
			public string Description { get; set; }
			public decimal Price { get; set; }
			public string PhotoUrl { get; set; }
			public int? MaxEntrance { get; set; }
			public XpDateTime SellStart { get; set; }
			public XpDateTime SellEnd { get; set; }
			public XpDateTime CheckInStart { get; set; }
			public XpDateTime CheckInEnd { get; set; }
			public decimal ExtraPrice { get; set; }
			public int EventId { get; set; }
			public string EventName { get; set; }
			public int? RangeMin { get; set; }
			public int? RangeMax { get; set; }
			public string ShortDescription { get; set; }
			public string Conditions { get; set; }
			public int MaxSendingCount { get; set; }
			public int? NumDays { get; set; }
			public XpTime StartDay { get; set; }
			public XpTime EndDay { get; set; }
		}
		public async Task<EntranceTypeGetResult> EntranceTypeGetAsync(int id)
		{
			using (var server = new HttpServer())
			{
				var items = await server.GetAsync<ResultBase<EntranceTypeGetResult>>(
					"/public/entrancetype/v1",
					id
				);
				return items.Data.FirstOrDefault();
			}
		}
		#endregion EntranceTypeGetAsync

		#region EntranceTypeGetAllAsync
		public class EntranceTypeGetAllResult
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }
			public decimal Price { get; set; }
			public string PhotoUrl { get; set; }
			public int? MaxEntrance { get; set; }
			public XpDateTime SellStart { get; set; }
			public XpDateTime SellEnd { get; set; }
			public XpDateTime CheckInStart { get; set; }
			public XpDateTime CheckInEnd { get; set; }
			public decimal ExtraPrice { get; set; }
			public EntranceTypeState State { get; set; }
			public int EventId { get; set; }
			public int SellEntrances { get; set; }
			public decimal? PercentCapacity { get; set; }
			public int? EventCapacity { get; set; }
			public string EventName { get; set; }
			public int? SumCapacity { get; set; }
			public bool IsVisible { get; set; }
			public decimal TotalAmount { get; set; }
			public int Forms { get; set; }
		}
		public async Task<IEnumerable<EntranceTypeGetAllResult>> EntranceTypeGetAllAsync(int eventId)
		{
			using (var server = new HttpServer())
			{
				var items = await server.GetAsync<ResultBase<EntranceTypeGetAllResult>>(
					"/public/entrancetype/v1",
					new
					{
						eventId = eventId
					}
				);
				return items.Data;
			}
		}
		#endregion EntranceTypeGetAllAsync

		#region EventCreateAsync
		public async Task<int> EventCreateAsync(int paymentConcessionId, decimal? longitude, decimal? latitude, string place,
			string name, string description, int? capacity, DateTime? eventStart, DateTime? eventEnd,
			DateTime checkInStart, DateTime checkInEnd, int entranceSystemId, long? code,
			string shortDescription, string conditions, EventVisibility visibility)
		{
			using (var server = new HttpServer())
			{
				var items = await server.PostAsync<IdResult>(
					"/public/event/v1",
					new
					{
						PaymentConcessionId = paymentConcessionId,
						Longitude = longitude,
						Latitude = latitude,
						Place = place,
						Name = name,
						Description = description,
						Capacity = capacity,
						EventStart = eventStart,
						EventEnd = eventEnd,
						CheckInStart = checkInStart,
						CheckInEnd = checkInEnd,
						EntranceSystemId = entranceSystemId,
						Code = code,
						ShortDescription = shortDescription,
						Conditions = conditions,
						Visibility = visibility
					}
				);
				return items.Id;
			}
		}
		#endregion EventCreateAsync

		#region EventDeleteAsync
		public async Task EventDeleteAsync(int id)
		{
			using (var server = new HttpServer())
			{
				await server.DeleteAsync(
					"/public/event/v1",
					id
				);
			}
		}
		#endregion EventDeleteAsync

		#region EventGetAsync
		public class EventGetResult
		{
			public int Id { get; set; }
			public decimal? Longitude { get; set; }
			public decimal? Latitude { get; set; }
			public string Place { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }
			public string PhotoUrl { get; set; }
			public int? Capacity { get; set; }
			public XpDateTime EventStart { get; set; }
			public XpDateTime EventEnd { get; set; }
			public XpDateTime CheckInStart { get; set; }
			public XpDateTime CheckInEnd { get; set; }
			public EventState State { get; set; }
			public int EntranceSystemId { get; set; }
			public string EntranceSystemName { get; set; }
			public bool IsVisible { get; set; }
			public long? Code { get; set; }
			public string ShortDescription { get; set; }
			public string Conditions { get; set; }
			public EventVisibility Visibility { get; set; }
		}
		public async Task<EventGetResult> EventGetAsync(int id)
		{
			using (var server = new HttpServer())
			{
				var items = await server.GetAsync<ResultBase<EventGetResult>>(
					"/public/event/v1",
					id
				);
				return items.Data.FirstOrDefault();
			}
		}
		#endregion EventGetAllAsync

		#region EventGetAllAsync
		public class EventGetAllResult
		{
			public XpDateTime EventEnd { get; set; }
			public XpDateTime EventStart { get; set; }
			public int Id { get; set; }
			public bool IsVisible { get; set; }
			public string Name { get; set; }
			public string Place { get; set; }
			public EventState State { get; set; }
			public decimal TotalAmount { get; set; }
		}
		public async Task<IEnumerable<EventGetAllResult>> EventGetAllAsync(int paymentConcessionId)
		{
			using (var server = new HttpServer())
			{
				var items = await server.GetAsync<ResultBase<EventGetAllResult>>(
					"/public/event/v1",
					new
					{
						paymentConcessionId = paymentConcessionId
					}
				);
				return items.Data;
			}
		}
		#endregion EventGetAllAsync

		#region PaymentConcessionGetSelectorAsync
		public async Task<IEnumerable<SelectorResult>> PaymentConcessionGetSelectorAsync()
		{
			using (var server = new HttpServer())
			{
				var items = await server.GetAsync<ResultBase<SelectorResult>>(
					"/public/paymentconcession/v1/selector"
				);

				return items.Data;
			}
		}
		#endregion PaymentConcessionGetSelectorAsync

		#region PaymentMediaCreateWebCardAsync
		public async Task<PaymentMediaCreateWebCardResult> PaymentMediaCreateWebCardByUserAsync(string login, string name, string pin, string bankEntity)
		{
			using (var server = new HttpServer())
			{
				var result = await server.PostAsync<PaymentMediaCreateWebCardResult>(
					"/public/paymentmedia/v1/webcarduser",
					new
					{
						Login = login,
						UserName = "Xavi Paper",
						UserTaxName = "Javier",
						UserTaxLastName = "Jorge Cerdá",
						//userBirthday = new DateTime(1976, 5, 9),
						//UserTaxNumber = "12345678A",
						//UserTaxAddress = "C/ Libertad",
						//UserPhone = "12345678",
						UserEmail = login,
						//Name = name,
						Pin = pin,
						//BankEntity = bankEntity
					}
				);
				return result;
			}
		}
		#endregion PaymentMediaCreateWebCardAsync

		#region PaymentMediaGetByUserAsync
		public async Task<IEnumerable<MobilePaymentMediaGetAllResult>> PaymentMediaGetByUserAsync(string login)
		{
			using (var server = new HttpServer())
			{
				var items = await server.GetAsync<ResultBase<MobilePaymentMediaGetAllResult>>(
					"/public/paymentmedia/v1/user",
					new
					{
						Login = login
					}
				);
				return items.Data;
			}
		}
		#endregion PaymentMediaGetByUserAsync

		#region TicketCreateEntrancesAsync
		public async Task<MobileTicketCreateAndGetResultBase> TicketCreateEntrancesAsync(string reference, int concessionId, IEnumerable<PublicTicketCreateEntrancesAndGetArguments_TicketLine> lines, TicketType? type, string email, string login, DateTime now)
		{
			using (var server = new HttpServer())
			{
                var items = await server.PostAsync<MobileTicketCreateAndGetResultBase>(
                    "/public/ticket/v1/entrances",
                    new PublicTicketCreateEntrancesAndGetArguments(
                        email,
                        login,
                        reference,
                        concessionId,
                        null,
                        lines,
                        type,
                        now
                    )
				);
				return items;
			}
		}
		#endregion TicketCreateEntrancesAsync

		#region TicketCreateProductsAsync
		public async Task<MobileTicketCreateAndGetResultBase> TicketCreateProductsAsync(string reference, int concessionId, IEnumerable<PublicTicketCreateProductsAndGetArguments_TicketLine> lines, TicketType? type, string email, string login, DateTime? now = null)
		{
			using (var server = new HttpServer())
			{
				var items = await server.PostAsync<MobileTicketCreateAndGetResultBase>(
					"/public/ticket/v1/products",
					new
					{
						Reference = reference,
						ConcessionId = concessionId,
						Lines = lines,
						Type = type,
						Email = email,
						Login = login,
						Now = now
					}
				);
				return items;
			}
		}
		#endregion TicketCreateProductsAsync

		#region TicketPayUserAsync
		public async Task<int> TicketPayUserAsync(int ticketId, string login, string name, string taxNumber, string taxName, string taxAddress, int paymentMediaId, string pin, decimal amount)
		{
			using (var server = new HttpServer())
			{
				var items = await server.PostAsync<IdResult>(
					"/public/ticket/v1/payuser",
					ticketId,
					new PublicTicketPayUserArguments(
						login: login,
						name: name,
						taxNumber: taxNumber,
						taxName: taxName,
						taxAddress: taxAddress,
						pin: pin,
						paymentMedias: new List<PublicTicketPayUserArguments_PaymentMedia>()
						{
							new PublicTicketPayUserArguments_PaymentMedia
							{
								Id = paymentMediaId,
								Balance = amount,
								Order = 0
							}
						}
					)
				);
				return items.Id;
			}
		}
		#endregion TicketPayUserAsync

		#region TicketPayAndCreateWebCardByUser
		public async Task<ResultBase<PaymentMediaCreateWebCardResult>> TicketPayAndCreateWebCardByUser(string userEmail, string userTaxNumber, string userName, string userTaxName, string userTaxLastName, string userTaxAddress, XpDateTime userBirthday, string userPhone, string login, int ticketId, string pin, string name, string bankEntity)
		{
			using (var server = new HttpServer())
			{
				var items = await server.PostAsync<ResultBase<PaymentMediaCreateWebCardResult>>(
					"/public/ticket/v1/payandcreatewebcardbyuser",
					new PublicTicketPayAndCreateWebCardByUserArguments(
						userEmail: userEmail,
						userName: userName,
						userTaxName: userTaxName,
						userTaxLastName: userTaxLastName,
						userTaxNumber: userTaxNumber,
						userTaxAddress: userTaxAddress,
						userBirthday: userBirthday,
						userPhone: userPhone,
						login: login,
						ticketId: ticketId,
						pin: pin,
						name: name,
						bankEntity: bankEntity,
						deviceManufacturer: "",
						deviceModel: "",
						deviceName: "",
						deviceSerial: "",
						deviceId: "",
						deviceOperator: "",
						deviceImei: "",
						deviceMac: "",
						operatorSim: "",
						operatorMobile: ""
					)
				);
				return items;
			}
		}
		#endregion TicketPayAndCreateWebCardByUser

		#region ServiceUserCreateAsync
		public async Task<IdResult> ServiceUserCreateAsync(int systemCardId)
		{
			using (var server = new HttpServer())
			{
				var items = await server.PostAsync<IdResult>(
					"/public/serviceuser/v1",
					new ServiceUserCreateArguments(
						phone: 12345678,
						mail: "user@pay-in.es",
						assertDoc: null,
						birthDate: new DateTime(2000, 8, 11),
						uid: null,
						cardState: ServiceCardState.Active,
						name: "User",
						lastname: "Normal",
						vatNumber: "12345678A",
						systemCardId: systemCardId,
						code: "",
						observations: "Observaciones",
						address: "Calle usuario S/N"
					)
				);
				return items;
			}
		}
		#endregion ServiceUserCreateAsync

		#region SystemCardGetSelectorAsync
		public async Task<IEnumerable<SelectorResult>> SystemCardGetSelectorAsync()
		{
			using (var server = new HttpServer())
			{
				var items = await server.GetAsync<ResultBase<SelectorResult>>(
					"/public/systemcard/v1/selector"
				);
				return items.Data;
			}
		}
		#endregion SystemCardGetSelectorAsync

		#region Dispose
		public void Dispose()
		{
		}
		#endregion Dispose
	}
}

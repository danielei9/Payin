using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers.Main
{
    public class MobileMainGetAllV4Handler :
        IQueryBaseHandler<MobileMainGetAllV4Arguments, MobileMainGetAllV4Result>
    {
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public IEntityRepository<Event> EventRepository { get; set; }
        [Dependency] public IEntityRepository<Notice> NoticeRepository { get; set; }
        [Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }
        [Dependency] public IEntityRepository<ServiceUser> ServiceUserRepository { get; set; }
        [Dependency] public IEntityRepository<Translation> TranslationRepository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobileMainGetAllV4Result>> ExecuteAsync(MobileMainGetAllV4Arguments arguments)
        {
            var now = DateTime.UtcNow;
            var allSystemCards = (await SystemCardRepository.GetAsync());
            var mySystemCard = allSystemCards
                .Where(x => x.Id == arguments.SystemCardId);
            var allServiceConcessions = (await ServiceConcessionRepository.GetAsync())
                .Where(x => x.State == ConcessionState.Active);
            var allPaymentConcessions = (await PaymentConcessionRepository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active)
                );
            var myPaymentConcessions = (await PaymentConcessionRepository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active) &&
                    (
                        (x.Concession.Supplier.Login == SessionData.Login) ||
                        (x.PaymentWorkers
                            .Where(y =>
                                (y.State == WorkerState.Active) &&
                                (y.Login == SessionData.Login)
                            )
                            .Any()
                        )
                    )
                )
                .Select(x => new
                {
                    PaymentConcessionId = x.Id,
                    x.ConcessionId,
                    x.Concession.Supplier.Login
                });

            #region Translations
            var translations = (await TranslationRepository.GetAsync())
                .Where(y =>
                    (y.Language == arguments.Language)
                );
            #endregion Translations

            #region System Card Profiles
            var resultSystemCard = (await SystemCardRepository.GetAsync())
                .Where(x =>
                    (x.ProfileId != null) &&
                    (x.ConcessionOwner.State == ConcessionState.Active) &&
                    (
                        // BORAR
                        (x.Id == arguments.SystemCardId) ||
                        // Es afiliado
                        (
                            (
                                (SessionData.ClientId == AccountClientId.AndroidNative) ||
                                (SessionData.ClientId == AccountClientId.AndroidFallesNative) ||
                                (SessionData.ClientId == AccountClientId.AndroidVilamarxantNative)
                            ) &&
                            (x.ClientId == SessionData.ClientId) &&
                            (
                                (x.ConcessionOwner.ServiceUsers
                                    .Any(a =>
                                        (a.State == ServiceUserState.Active) &&
                                        (a.Login == SessionData.Login)
                                    )
                                ) ||
                                (x.SystemCardMembers
                                    .Any(y =>
                                        (allServiceConcessions
                                            .Any(z =>
                                                (z.State == ConcessionState.Active) &&
                                                (z.ServiceUsers
                                                    .Any(a =>
                                                        (a.State == ServiceUserState.Active) &&
                                                        (a.Login == SessionData.Login)
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        ) ||
                        // Tiene tarjeta
                        (
                            (
                                (SessionData.ClientId == AccountClientId.AndroidNative) ||
                                (SessionData.ClientId == AccountClientId.AndroidFallesNative) ||
                                (SessionData.ClientId == AccountClientId.AndroidVilamarxantNative)
                            ) &&
                            (x.ClientId == SessionData.ClientId) &&
                            (x.Cards
                                .Any(y =>
                                    (
                                        (y.State == ServiceCardState.Active) ||
                                        (y.State == ServiceCardState.Emited)
                                    ) &&
                                    (
                                        // Tarjeta vinculada
                                        (y.OwnerLogin == SessionData.Login) ||
                                        // Tarjeta afiliado
                                        (y.Users
                                            .Any(z =>
                                                (z.State == ServiceUserState.Active) &&
                                                (z.Login == SessionData.Login)
                                            )
                                        )
                                    )
                                )
                            )
                        ) ||
                        // Para usarios profesionales
                        (myPaymentConcessions
                            .Any(myConcession =>
                                // Es la empresa propietaria o trabajador
                                (x.ConcessionOwner.Supplier.Login == myConcession.Login) ||
                                // Es una empresa miembro o trabajador
                                (x.SystemCardMembers
                                    .Where(z => z.Login == myConcession.Login)
                                    .Any()
                                )
                            )
                        )
                    )
                )
                .Select(x => new
                {
                    x.Profile.Name,
                    IconUrl = x.Profile.Icon,
                    BackgroundUrl = x.Profile.ImageUrl,
                    x.Profile.Color,
                    SystemCardId = x.Id,
                    SystemCardPhotoUrl = x.PhotoUrl,
                    SystemCardSynchronizationInterval = x.SynchronizationInterval,
                    ServiceConcessionId =
                        // Mi empresa (como empresa o trabajador)
                        allPaymentConcessions
                            .Where(y => x.SystemCardMembers
                                .Any(z =>
                                    z.Login == y.Concession.Supplier.Login
                                )
                            )
                            .Select(y => (int?)y.ConcessionId)
                            .FirstOrDefault(),
                    Layout = (SessionData.ClientId == AccountClientId.CashlessProApp) ? x.Profile.LayoutPro : x.Profile.Layout
                })
                .ToList()
                .Select(x => new MobileMainGetAllV4Result
                {
                    Name = x.Name,
                    IconUrl = x.IconUrl,
                    BackgroundUrl = x.BackgroundUrl,
                    Color = x.Color,
                    SystemCardId = x.SystemCardId,
                    SystemCardPhotoUrl = x.SystemCardPhotoUrl,
                    SystemCardSynchronizationInterval = x.SystemCardSynchronizationInterval,
                    EventId = null,
                    PaymentConcessionId = allPaymentConcessions
                        .Where(y => y.ConcessionId == x.ServiceConcessionId)
                        .Select(y => y.Id)
                        .FirstOrDefault(),
                    Layout = x.Layout
                        .SplitString(";")
                        .Select(y => new MobileMainGetAllV4Result_Row
                        {
                            Options = y.SplitString(",")
                                .Select(z => new MobileMainGetAllV4Result_Option
                                {
                                    Name = z
                                })
                        })
                })
#if DEBUG
                .ToList()
#endif // DEBUG
                ;
            #endregion System Card Profiles

            #region Event Profiles
            var resultEvent = (await EventRepository.GetAsync())
                .Where(x =>
                    (x.ProfileId != null) &&
                    (
                        (SessionData.ClientId == "") ||
                        (SessionData.ClientId == AccountClientId.AndroidFallesNative) ||
                        (SessionData.ClientId == AccountClientId.AndroidVilamarxantNative) ||
                        (SessionData.ClientId == AccountClientId.AndroidFinestratNative) ||
                        (SessionData.ClientId == AccountClientId.AndroidNative)
                    ) &&
                    (x.State == EventState.Active) &&
                    (x.EventEnd >= now) &&
                    (
                        // Tiene entrada
                        (x.EntranceTypes
                            .Any(y =>
                                (y.State == EntranceTypeState.Active) &&
                                (
                                    (y.EndDay == null) ||
                                    (y.EndDay >= now)
                                ) &&
                                (y.Entrances
                                    .Any(z =>
                                        (z.State == EntranceState.Active || z.State == EntranceState.Validated) &&
                                        (z.Login == SessionData.Login)
                                    )
                                )
                            )
                        ) || (
                            // Es expositor
                            x.Exhibitors
                                .Where(y =>
                                    (y.State == ExhibitorState.Active) &&
                                    (myPaymentConcessions
                                        .Where(myConcession => myConcession.Login == y.PaymentConcession.Concession.Supplier.Login)
                                        .Any()
                                    )
                                )
                                .Any()
                        )
                    )
                )
                .Select(x => new
                {
                    x.Profile.Name,
                    IconUrl = x.Profile.Icon,
                    BackgroundUrl = x.Profile.ImageUrl,
                    x.Profile.Color,
                    EventId = x.Id,
                    PaymentConcessionId =
                        // Eres la empresa del evento o trabajador
                        allPaymentConcessions
                            .Where(z => z.Concession.Supplier.Login == x.PaymentConcession.Concession.Supplier.Login)
                            .Select(z => (int?)z.ConcessionId)
                            .Any() ? x.PaymentConcessionId :
                        // Eres un expositor o trabajador
                        x.Exhibitors
                        .Where(y =>
                            allPaymentConcessions
                                .Where(z => z.Concession.Supplier.Login == y.PaymentConcession.Concession.Supplier.Login)
                                .Any()
                        )
                        .Select(y => y.PaymentConcessionId)
                        .FirstOrDefault(),
                    x.Profile.Layout, // En evento siempre es el perfil de usuario
                    ExhibitorIds = x.Exhibitors
                        .Select(y => y.Id)
                })
                .ToList()
                .Select(x => new MobileMainGetAllV4Result
                {
                    //Name = translations
                    //		.Where(y => y.EventNameId == x.EventId)
                    //		.Select(y => y.Text)
                    //		.FirstOrDefault(),
                    Name = x.Name,
                    IconUrl = x.IconUrl,
                    BackgroundUrl = x.BackgroundUrl,
                    Color = x.Color,
                    SystemCardId = null,
                    SystemCardPhotoUrl = "",
                    SystemCardSynchronizationInterval = null,
                    EventId = x.EventId,
                    PaymentConcessionId = x.PaymentConcessionId,
                    Layout = x.Layout
                        .SplitString(";")
                        .Select(y => new MobileMainGetAllV4Result_Row
                        {
                            Options = y.SplitString(",")
                                .Select(z => new MobileMainGetAllV4Result_Option
                                {
                                    Name = z
                                })
                        })
                })
#if DEBUG
                .ToList()
#endif // DEBUG
                ;
            #endregion Event Profiles

            #region Events to show
            var events = default(IEnumerable<MobileMainGetAllV4Result_Event>);
            if (
                (SessionData.ClientId == "") ||
                (SessionData.ClientId == AccountClientId.FiraValenciaApp) ||
                (SessionData.ClientId == AccountClientId.AndroidFallesNative) ||
                (SessionData.ClientId == AccountClientId.AndroidVilamarxantNative) ||
                (SessionData.ClientId == AccountClientId.AndroidFinestratNative)
            )
            {
                events = (await GetPublicEventsAsync(arguments.SystemCardId, arguments.PaymentConcessionId, 5, now))
                    .Select(x => new MobileMainGetAllV4Result_Event
                    {
                        Id = x.Id,
                        Name = translations
                            .Where(y => y.EventNameId == x.Id)
                            .Select(y => y.Text)
                            .FirstOrDefault() ?? x.Name,
                        Icon = x.PhotoMenuUrl
                    });
            }
            #endregion Events to show

            #region Notices to show
            var notices = default(IEnumerable<MobileMainGetAllV4Result_Notice>);
            if (
                (SessionData.ClientId == "") ||
                (SessionData.ClientId == AccountClientId.FiraValenciaApp) ||
                (SessionData.ClientId == AccountClientId.AndroidFallesNative) ||
                (SessionData.ClientId == AccountClientId.AndroidVilamarxantNative) ||
                (SessionData.ClientId == AccountClientId.AndroidFinestratNative)
            )
            {
                notices = (await GetPublicNoticesAsync(arguments.SystemCardId, arguments.PaymentConcessionId, 5, now))
                    .Select(x => new MobileMainGetAllV4Result_Notice
                    {
                        Id = x.Id,
                        Name = translations
                            .Where(y => y.NoticeNameId == x.Id)
                            .Select(y => y.Text)
                            .FirstOrDefault() ?? x.Name,
                        Icon = x.PhotoUrl
                    });
            }
            #endregion Notices to show

            var photo = (await ServiceUserRepository.GetAsync())
                .Where(x => x.Login == SessionData.Login)
                .Select(x => x.Photo)
                .FirstOrDefault();

            return new MobileMainGetAllV4ResultBase
            {
                Name = SessionData.Name,
                Email = SessionData.Email,
                IconUrl = photo ?? "",
                Events = events,
                Notices = notices,
                Data = resultSystemCard.Union(resultEvent)
            };
        }
        #endregion ExecuteAsync

        #region GetPublicEventsAsync
        public async Task<IQueryable<Event>> GetPublicEventsAsync(int? systemCardId, int? paymentConcessionId, int? take, DateTime now)
        {
            // SystemCard
            var allSystemCards = (await SystemCardRepository.GetAsync());
            var mySystemCard = allSystemCards
                .Where(x => x.Id == systemCardId);

            // Events
            IQueryable<Event> items = (await EventRepository.GetAsync())
                .Where(x =>
                    (x.PaymentConcessionId == paymentConcessionId) &&
                    (x.State == EventState.Active) &&
                    (x.IsVisible) &&
                    (x.Visibility == EventVisibility.Public) &&
                    (x.EventEnd >= now) //&&
                    // XAVI REVISAR
                    //(x.PaymentConcession.CanHasPublicEvent) && 
                    //(
                    //    (
                    //        // Eventos públicos
                    //        (x.Visibility == EventVisibility.Public)
                    //    ) ||
                    //    (
                    //        // Eventos internos al estar en el perfil
                    //        (x.Visibility == EventVisibility.Internal) &&
                    //        (arguments.PaymentConcessionId != null) &&
                    //        (x.PaymentConcessionId == arguments.PaymentConcessionId)
                    //    ) ||
                    //    (
                    //        // Eventos solo para miembros
                    //        (x.Visibility == EventVisibility.Members) &&
                    //        (arguments.PaymentConcessionId != null) &&
                    //        (x.PaymentConcessionId == arguments.PaymentConcessionId) &&
                    //        (allPaymentConcessions
                    //            .Any(y => y.Concession.ServiceUsers
                    //                .Any(z => z.Login == SessionData.Login)
                    //            )
                    //        )
                    //    )
                    //) &&
                    // Comprobar ClientId
                    // XAVI REVISAR
                    //(mySystemCard
                    //	.Any(y =>
                    //		((SessionData.ClientId == "") || (y.ClientId == SessionData.ClientId)) &&
                    //		(
                    //			(y.ConcessionOwner.Supplier.Login == x.PaymentConcession.Concession.Supplier.Login) ||
                    //			(y.SystemCardMembers
                    //				.Any(z =>
                    //					(z.Login == x.PaymentConcession.Concession.Supplier.Login)
                    //				)
                    //			)
                    //		)
                    //	)
                    //)
                )
                .OrderBy(x => x.EventStart);

            if (take != null)
                items = items
                    .Take(5);

            return items;
        }
        #endregion GetPublicEventsAsync

        #region GetPublicNoticesAsync
        public async Task<IQueryable<Notice>> GetPublicNoticesAsync(int? systemCardId, int? paymentConcessionId, int? take, DateTime now)
        {
            // SystemCard
            var allSystemCards = (await SystemCardRepository.GetAsync());
            var mySystemCard = allSystemCards
                .Where(x => x.Id == systemCardId);

            // Events
            IQueryable<Notice> items = (await NoticeRepository.GetAsync())
                .Where(x =>
                    (x.PaymentConcessionId == paymentConcessionId) &&
                    (x.State == NoticeState.Active) &&
                    (x.IsVisible) &&
                    (x.Type == NoticeType.Notice) &&
                    (x.Visibility == NoticeVisibility.Public) &&
                    (x.SuperNoticeId == null) &&
                    (x.Start <= now) //&&
                    //(
                    //	(
                    //		// Noticias públicos
                    //		(x.Visibility == NoticeVisibility.Public)
                    //	) ||
                    //	(
                    //		// Noticias internas al estar en el perfil
                    //		(x.Visibility == NoticeVisibility.Internal) &&
                    //		(arguments.PaymentConcessionId != null) &&
                    //		(x.PaymentConcessionId == arguments.PaymentConcessionId)
                    //	) ||
                    //	(
                    //		// Noticias solo para miembros
                    //		(x.Visibility == NoticeVisibility.Members) &&
                    //		(arguments.PaymentConcessionId != null) &&
                    //		(x.PaymentConcessionId == arguments.PaymentConcessionId) &&
                    //		(allPaymentConcessions
                    //			.Any(y => y.Concession.ServiceUsers
                    //				.Any(z => z.Login == SessionData.Login)
                    //			)
                    //		)
                    //	)
                    //) &&
                    // Comprobar ClientId / SystemCard
                    // XAVI REVISAR
                    //(mySystemCard
                    //	.Any(y =>
                    //		((SessionData.ClientId == "") || (y.ClientId == SessionData.ClientId)) &&
                    //		(
                    //			(y.ConcessionOwnerId == x.PaymentConcession.ConcessionId) ||
                    //			(y.SystemCardMembers
                    //				.Any(z =>
                    //					(z.Login == x.PaymentConcession.Concession.Supplier.Login)
                    //				)
                    //			)
                    //		)
                    //	)
                    //)
                )
                .OrderByDescending(x => x.Start);
            //var temp = items.ToList();

            if (take != null)
                items = items
                    .Take(5);

            return items;
        }
        #endregion GetPublicNoticesAsync

        #region GetPublicEdictsAsync
        public async Task<IQueryable<Notice>> GetPublicEdictsAsync(int? systemCardId, int? paymentConcessionId, int? take, DateTime now)
        {
            // SystemCard
            var allSystemCards = (await SystemCardRepository.GetAsync());
            var mySystemCard = allSystemCards
                .Where(x => x.Id == systemCardId);

            // Events
            IQueryable<Notice> items = (await NoticeRepository.GetAsync())
                .Where(x =>
                    (x.PaymentConcessionId == paymentConcessionId) &&
                    (x.State == NoticeState.Active) &&
                    (x.IsVisible) &&
                    (x.Type == NoticeType.Edict) &&
                    (x.Visibility == NoticeVisibility.Public) &&
                    (x.IsVisible) &&
                    (x.Start <= now && x.End >= now) //&&
                    //(
                    //	(
                    //		// Noticias públicos
                    //		(x.Visibility == NoticeVisibility.Public)
                    //	) ||
                    //	(
                    //		// Noticias internas al estar en el perfil
                    //		(x.Visibility == NoticeVisibility.Internal) &&
                    //		(arguments.PaymentConcessionId != null) &&
                    //		(x.PaymentConcessionId == arguments.PaymentConcessionId)
                    //	) ||
                    //	(
                    //		// Noticias solo para miembros
                    //		(x.Visibility == NoticeVisibility.Members) &&
                    //		(arguments.PaymentConcessionId != null) &&
                    //		(x.PaymentConcessionId == arguments.PaymentConcessionId) &&
                    //		(allPaymentConcessions
                    //			.Any(y => y.Concession.ServiceUsers
                    //				.Any(z => z.Login == SessionData.Login)
                    //			)
                    //		)
                    //	)
                    //) &&
                    // Comprobar ClientId / SystemCard
                    // XAVI REVISAR
                    //(mySystemCard
                    //	.Any(y =>
                    //		((SessionData.ClientId == "") || (y.ClientId == SessionData.ClientId)) &&
                    //		(
                    //			(y.ConcessionOwnerId == x.PaymentConcession.ConcessionId) ||
                    //			(y.SystemCardMembers
                    //				.Any(z =>
                    //					(z.Login == x.PaymentConcession.Concession.Supplier.Login)
                    //				)
                    //			)
                    //		)
                    //	)
                    //)
                )
                .OrderByDescending(x => x.Start);

            if (take != null)
                items = items
                    .Take(5);

            return items;
        }
        #endregion GetPublicEdictsAsync

        #region GetPublicPagesAsync
        public async Task<IQueryable<Notice>> GetPublicPagesAsync(int? systemCardId, int? paymentConcessionId, int? take, DateTime now)
        {
            // SystemCard
            var allSystemCards = (await SystemCardRepository.GetAsync());
            var mySystemCard = allSystemCards
                .Where(x => x.Id == systemCardId);

            // Events
            IQueryable<Notice> items = (await NoticeRepository.GetAsync())
                .Where(x =>
                    (x.PaymentConcessionId == paymentConcessionId) &&
                    (x.State == NoticeState.Active) &&
                    (x.IsVisible) &&
                    (x.Type == NoticeType.Page) &&
                    (x.Visibility == NoticeVisibility.Public) &&
                    (x.IsVisible) &&
                    (x.SuperNoticeId == null) &&
                    (x.Start <= now && x.End >= now) //&&
                    //(
                    //	(
                    //		// Noticias públicos
                    //		(x.Visibility == NoticeVisibility.Public)
                    //	) ||
                    //	(
                    //		// Noticias internas al estar en el perfil
                    //		(x.Visibility == NoticeVisibility.Internal) &&
                    //		(arguments.PaymentConcessionId != null) &&
                    //		(x.PaymentConcessionId == arguments.PaymentConcessionId)
                    //	) ||
                    //	(
                    //		// Noticias solo para miembros
                    //		(x.Visibility == NoticeVisibility.Members) &&
                    //		(arguments.PaymentConcessionId != null) &&
                    //		(x.PaymentConcessionId == arguments.PaymentConcessionId) &&
                    //		(allPaymentConcessions
                    //			.Any(y => y.Concession.ServiceUsers
                    //				.Any(z => z.Login == SessionData.Login)
                    //			)
                    //		)
                    //	)
                    //) &&
                    // Comprobar ClientId / SystemCard
                    // XAVI REVISAR
                    //(mySystemCard
                    //	.Any(y =>
                    //		((SessionData.ClientId == "") || (y.ClientId == SessionData.ClientId)) &&
                    //		(
                    //			(y.ConcessionOwnerId == x.PaymentConcession.ConcessionId) ||
                    //			(y.SystemCardMembers
                    //				.Any(z =>
                    //					(z.Login == x.PaymentConcession.Concession.Supplier.Login)
                    //				)
                    //			)
                    //		)
                    //	)
                    //)
                )
                .OrderByDescending(x => x.Start);

            if (take != null)
                items = items
                    .Take(5);

            return items;
        }
        #endregion GetPublicPagesAsync
    }
}

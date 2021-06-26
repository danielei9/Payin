using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Application.Results;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    [XpAnalytics("Event", "MobileGet", Response = new[] { "Id:Data[0].Id", "Nombre:Data[0].Name" })]
    public class MobileEventGetHandler :
		IQueryBaseHandler<MobileEventGetArguments, MobileEventGetResult>
	{
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public IEntityRepository<Translation> TranslationRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileEventGetResult>> ExecuteAsync(MobileEventGetArguments arguments)
		{
			var now = DateTime.UtcNow;
			var language = arguments.Language;

			var translations = (await TranslationRepository.GetAsync());
			var translationName = (await TranslationRepository.GetAsync())
				.Where(y =>
					(y.EventNameId == arguments.Id) &&
					(y.Language == arguments.Language)
				)
				.Select(y => y.Text)
				.FirstOrDefault();
			var translationDescription = (await TranslationRepository.GetAsync())
				.Where(y =>
					(y.EventDescriptionId == arguments.Id) &&
					(y.Language == arguments.Language)
				)
				.Select(y => y.Text)
				.FirstOrDefault();

			var result = (await Repository.GetAsync())
				.Where(x =>
					(x.Id == arguments.Id) &&
					(x.State == EventState.Active) &&
					(x.IsVisible)
				)
				.Select(x => new {
					x.Id,
					Name = translationName ?? x.Name,
					Description = translationDescription ?? x.Description,
					x.PhotoUrl,
					x.MapUrl,
					x.Place,
					x.EventStart,
					x.EventEnd,
					x.CheckInStart,
					x.CheckInEnd,
					x.Conditions,
					x.PaymentConcessionId,
					Images = x.EventImages
						.Select(y => new MobileEventGetResult_Images
						{
							Id = y.Id,
							PhotoUrl = y.PhotoUrl
						}),
					EntranceTypes = x.EntranceTypes
						.Where(y =>
							(y.IsVisible) &&
							(y.SellStart <= now) &&
							(y.SellEnd >= now)
						)
						.Select(y => new {
							y.Id,
							y.Name,
							y.Description,
							y.PhotoUrl,
							y.Price,
							y.ExtraPrice,
							y.MaxEntrance,
							SelledEntrance = y.Entrances
								.Where(z =>
									(z.State == EntranceState.Active)
								)
								.Count(),
							y.CheckInStart,
							y.CheckInEnd,
							y.SellEnd,
							Forms = y.EntranceTypeForms
								.Select(z => new IdResult {
									Id = z.FormId
								})

						}),
					Activities = x.Activities
						.Where(y =>
							y.EventId == arguments.Id
						)
						.Select(y => new
						{
							y.Id,
							y.Name,
							y.Description,
							y.Start,
							y.End
						}),
					Notices = x.Notices
						.Where(y =>
							y.State != NoticeState.Deleted &&
							y.IsVisible == true &&
							y.Visibility != NoticeVisibility.Members
						)
						.Select(y => new
						{
							y.Id,
							y.Name,
							y.ShortDescription,
							y.PhotoUrl,
							y.Start
						}),
					Exhibitors = x.Exhibitors
						.Where(y =>
							y.State == ExhibitorState.Active
						)
						.Select(y => new
						{
							y.Id,
							y.Name,
							y.City,
							y.Province,
							y.Url
						})
				})
				.ToList();

			// TRADUCE AQUÍ
			
			var rslt = result
				.Select(x => new MobileEventGetResult

				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					PhotoUrl = x.PhotoUrl,
					Place = x.Place,
					MapUrl = x.MapUrl,
					Conditions = x.Conditions,
					EventStart = x.EventStart.ToUTC(),
					EventEnd = x.EventEnd.ToUTC(),
					CheckInStart = x.CheckInStart.ToUTC(),
					CheckInEnd = x.CheckInEnd.ToUTC(),
					Images = x.Images,
					PaymentConcessionId = x.PaymentConcessionId,
					EntranceTypes = x.EntranceTypes
						.Select(y => new MobileEventGetResult_EntranceTypes {
							Id = y.Id,
							Name = y.Name,
							Description = y.Description,
							PhotoUrl = y.PhotoUrl,
							Price = y.Price,
							ExtraPrice = y.ExtraPrice,
							MaxEntrance = y.MaxEntrance == int.MaxValue ? (int?)null : y.MaxEntrance,
							SelledEntrance = y.SelledEntrance,
							CheckInStart = y.CheckInStart.ToUTC(),
							CheckInEnd = y.CheckInEnd.ToUTC(),
							SellEnd = y.SellEnd.ToUTC(),
                            Forms = y.Forms
                        }),
					Activities = x.Activities
						.Select(y => new MobileEventGetResult_Activities
						{
							Id = y.Id,
							Name = y.Name,
							Description = y.Description,
							Start = y.Start.ToUTC(),
							End = y.End.ToUTC()
						}),
                    Notices = x.Notices
                        .Select(y => new MobileEventGetResult_Notices
                        {
                            Id = y.Id,
							Name = y.Name,
							ShortDescription = y.ShortDescription,
							PhotoUrl = y.PhotoUrl,
                            Start = y.Start.ToUTC()
                        }),
                    Exhibitors = x.Exhibitors
                        .OrderBy(y => y.Name)
                        .Select(y => new MobileEventGetResut_Exhibitors
                        {
                            Id = y.Id,
                            Name = y.Name,
                            City = y.City,
                            Province = y.Province,
                            Url = y.Url
                        })
                })
#if DEBUG
				.ToList()
#endif // DEBUG
				;

			return new ResultBase<MobileEventGetResult> { Data = rslt };
		}
		#endregion ExecuteAsync
	}
}

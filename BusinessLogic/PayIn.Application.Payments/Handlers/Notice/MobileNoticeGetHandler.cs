using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    [XpAnalytics("Notice", "MobileGet", Response = new[] { "Id:Data[0].Id", "Nombre:Data[0].Name" })]
    public class MobileNoticeGetHandler : IQueryBaseHandler<MobileNoticeGetArguments, MobileNoticeGetResult>
    {
        [Dependency] public IEntityRepository<Notice> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<Translation> TranslationRepository { get; set; }

        #region ExecuteAsync

        public async Task<ResultBase<MobileNoticeGetResult>> ExecuteAsync(MobileNoticeGetArguments arguments)
        {
            var now = DateTime.UtcNow;

            var translations = (await TranslationRepository.GetAsync())
                .Where(y =>
                    (y.Language == arguments.Language)
                );
            var translationName = (await TranslationRepository.GetAsync())
                .Where(y =>
                    (y.NoticeNameId == arguments.Id) &&
                    (y.Language == arguments.Language)
                )
                .Select(y => y.Text)
                .FirstOrDefault();
            var translationDescription = (await TranslationRepository.GetAsync())
                .Where(y =>
                    (y.NoticeDescriptionId == arguments.Id) &&
                    (y.Language == arguments.Language)
                )
                .Select(y => y.Text)
                .FirstOrDefault();
            var translationShortDescription = (await TranslationRepository.GetAsync())
                .Where(y =>
                    (y.NoticeShortDescriptionId == arguments.Id) &&
                    (y.Language == arguments.Language)
                )
                .Select(y => y.Text)
                .FirstOrDefault();

            // No se comprueba visibilidad para poder ver una noticia como menú
            var items = (await Repository.GetAsync())
                .Where(x =>
                    (x.Id == arguments.Id) &&
                    (x.State == NoticeState.Active) &&
                    (x.Start <= now) &&
                    (x.End >= now)
                )
                .Select(x => new
                {
                    x.Id,
                    Name = translationName ?? x.Name,
                    Description = translationDescription ?? x.Description,
                    ShortDescription = translationShortDescription ?? x.ShortDescription,
                    x.PhotoUrl,
                    x.Start,
                    SubNotices = x.SubNotices
                        .Where(y =>
                            (y.State == NoticeState.Active) &&
                            (y.Start <= now) &&
                            (y.End >= now)
                        )
                        .Select(y => new MobileNoticeGetResult_SubNotice
                        {
                            Id = y.Id,
                            Name =
                                translations
                                    .Where(z =>
                                        (z.NoticeNameId == y.Id)
                                    )
                                    .Select(z => z.Text)
                                    .FirstOrDefault() ??
                                y.Name,
                            PhotoUrl = y.PhotoUrl
                        }),
                    Poi = x.Longitude == null || x.Latitude == null ?
                        null :
                        new PoiResult
                        {
                            Name = x.Name,
                            Longitude = x.Longitude ?? 0,
                            Latitude = x.Latitude ?? 0
                        },
                    Pois =
                        (
                            x.SubNotices
                            .Where(y => y.Longitude != null && y.Latitude != null)
                            .Select(y => new PoiResult
                            {
                                Name = translations
                                    .Where(z =>
                                        (z.NoticeNameId == y.Id)
                                    )
                                    .Select(z => z.Text)
                                    .FirstOrDefault() ??
                                    y.Name,
                                Longitude = y.Longitude ?? 0,
                                Latitude = y.Latitude ?? 0
                            })
                        ).ToList()
                        //.Union(
                        //    x.SubNotices
                        //    .SelectMany(y => y.SubNotices
                        //        .Where(z => z.Longitude != null && z.Latitude != null)
                        //        .Select(z =>
                        //            new PoiResult
                        //            {
                        //                Name = translations
                        //                    .Where(t =>
                        //                        (t.NoticeNameId == z.Id)
                        //                    )
                        //                    .Select(t => t.Text)
                        //                    .FirstOrDefault() ??
                        //                    z.Name,
                        //                Longitude = z.Longitude ?? 0,
                        //                Latitude = z.Latitude ?? 0
                        //            }
                        //        ).ToList()
                        //    ).ToList()
                        //)
                        //.Union(
                        //    x.SubNotices
                        //    .SelectMany(y => y.SubNotices
                        //        .SelectMany(z => z.SubNotices
                        //            .Where(a => a.Longitude != null && a.Latitude != null)
                        //            .Select(a =>
                        //                new PoiResult
                        //                {
                        //                    Name = translations
                        //                        .Where(t =>
                        //                            (t.NoticeNameId == a.Id)
                        //                        )
                        //                        .Select(t => t.Text)
                        //                        .FirstOrDefault() ??
                        //                        a.Name,
                        //                    Longitude = a.Longitude ?? 0,
                        //                    Latitude = a.Latitude ?? 0
                        //                }
                        //            ).ToList()
                        //        ).ToList()
                        //    )
                        //)
                        //.Union(
                        //    x.SubNotices
                        //    .SelectMany(y => y.SubNotices
                        //        .SelectMany(z => z.SubNotices
                        //            .SelectMany(a => a.SubNotices
                        //                .Where(b => b.Longitude != null && b.Latitude != null)
                        //                .Select(b =>
                        //                    new PoiResult
                        //                    {
                        //                        Name = translations
                        //                            .Where(t =>
                        //                                (t.NoticeNameId == b.Id)
                        //                            )
                        //                            .Select(t => t.Text)
                        //                            .FirstOrDefault() ??
                        //                            b.Name,
                        //                        Longitude = b.Longitude ?? 0,
                        //                        Latitude = b.Latitude ?? 0
                        //                    }
                        //                ).ToList()
                        //            ).ToList()
                        //        ).ToList()
                        //    ).ToList()
                        //)
                        //.Union(
                        //    x.SubNotices
                        //    .SelectMany(y => y.SubNotices
                        //        .SelectMany(z => z.SubNotices
                        //            .SelectMany(a => a.SubNotices
                        //                .SelectMany(b => b.SubNotices
                        //                    .Where(c => c.Longitude != null && c.Latitude != null)
                        //                    .Select(c =>
                        //                        new PoiResult
                        //                        {
                        //                            Name = translations
                        //                                .Where(t =>
                        //                                    (t.NoticeNameId == c.Id)
                        //                                )
                        //                                .Select(t => t.Text)
                        //                                .FirstOrDefault() ??
                        //                                c.Name,
                        //                            Longitude = c.Longitude ?? 0,
                        //                            Latitude = c.Latitude ?? 0
                        //                        }
                        //                    ).ToList()
                        //                ).ToList()
                        //            ).ToList()
                        //        ).ToList()
                        //    ).ToList()
                        //).ToList()
                }).ToList();

            foreach (var item in items)
                if (item.Poi != null)
                    item.Pois.Add(item.Poi);

            var result = items
                .Select(x => new MobileNoticeGetResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    PhotoUrl = x.PhotoUrl,
                    Start = x.Start.ToUTC(),
                    SubNotices = x.SubNotices,
                    Pois = x.Pois
                });

            return new ResultBase<MobileNoticeGetResult> { Data = result };
        }

        #endregion
    }
}

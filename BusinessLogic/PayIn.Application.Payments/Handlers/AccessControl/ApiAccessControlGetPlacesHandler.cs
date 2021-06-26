using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlGetPlacesHandler :
        IQueryBaseHandler<ApiAccessControlGetPlacesArguments, ApiAccessControlGetPlacesResult>
    {
        public class StatsHelper
        {
            public int Key { get; set; }
            public string Hour { get; set; }
            public decimal Value { get; set; }
            public string BarColor { get; set; }
        }

        [Dependency] public IEntityRepository<AccessControl> Repository { get; set; }

        #region ExecuteAsync

        public async Task<ResultBase<ApiAccessControlGetPlacesResult>> ExecuteAsync(ApiAccessControlGetPlacesArguments arguments)
        {
            var accesses = (await Repository.GetAsync());

            var dates = Enumerable.Range(10, 11)
                .Select(x => new StatsHelper
                {
                    Key = x,
                    Hour = $"{x}h",
                    Value = 0.0m,
                    BarColor = "#212492"
                });

            var end = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            var start = end.AddDays(-7);

            var result = accesses
                .Select(x => new
                {
                    x.Id,
                    Name = x.Name.Replace("\"", "'"),
                    x.Schedule,
                    x.CurrentCapacity,
                    x.MaxCapacity,
                    x.MapUrl,
                    DateTime = ((DateTime?)x.Entrances.SelectMany(y => y.Entries).Max(y => y.EntryDateTime)) ?? DateTime.UtcNow,
                    Entries = x.Entrances
                    .SelectMany(y => y.Entries
                        .Where(z =>
                            z.EntryDateTime >= start &&
                            z.EntryDateTime < end
                        )
                        .Select(z => new
                        {
                            z.EntryDateTime,
                            z.CapacityAfterEntrance
                        })
                    ),
                })
                .OrderByDescending(x => x.Id)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Schedule,
                    x.CurrentCapacity,
                    x.MaxCapacity,
                    Map = x.MapUrl,
                    //DateTime = DateTime.UtcNow,
                    x.DateTime,
                    Stats = x.Entries
                    .GroupBy(y => TimeZoneInfo.ConvertTimeFromUtc(y.EntryDateTime, TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time")).Hour)
                    .Where(y => y.Key > 9 && y.Key < 21)
                    .Select(y => new StatsHelper
                    {
                        Key = y.Key,
                        Hour = $"{y.Key}h",
                        Value = (decimal)(y.Average(z => z.CapacityAfterEntrance) * 100 / x.MaxCapacity),
                        BarColor = "#212492"
                    })
                })
                .Select(x => new ApiAccessControlGetPlacesResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Schedule = x.Schedule,
                    CurrentCapacity = x.CurrentCapacity,
                    MaxCapacity = x.MaxCapacity,
                    Map = x.Map,
                    DateTime = x.DateTime.ToUTC(),
                    Stats = Enumerable.Concat(x.Stats, dates)
                        .GroupBy(y => y.Key)
                        .Select(y => new
                        {
                            y.Key,
                            y.FirstOrDefault()?.Hour,
                            y.FirstOrDefault()?.BarColor,
                            Value = y.Sum(z => z.Value)
                        })
                        .OrderBy(y => y.Key)
                });

            return new ResultBase<ApiAccessControlGetPlacesResult> { Data = result };
        }

        #endregion
    }
}

using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductGetAllByDashboardHandler :
		IQueryBaseHandler<ProductGetAllByDashboardArguments, ProductGetAllByDashboardResult>
	{
		private readonly IEntityRepository<Product> Repository;
		private readonly IEntityRepository<Log> LogRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public ProductGetAllByDashboardHandler(
			IEntityRepository<Product> repository,
			IEntityRepository<Log> logRepository,
			ISessionData sessionData
			)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (logRepository == null) throw new ArgumentNullException("logRepository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			LogRepository = logRepository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ProductGetAllByDashboardResult>> ExecuteAsync(ProductGetAllByDashboardArguments arguments)
		{
			var now = DateTime.UtcNow;
			var lastYear = DateTime.UtcNow.AddMonths(-12);
			var previousYear = DateTime.UtcNow.AddMonths(-24);
			var lastMonth = DateTime.UtcNow.AddMonths(-1);
			var previousMonth = DateTime.UtcNow.AddMonths(-2);
			var lastWeek = DateTime.UtcNow.AddDays(-7);
			var previousWeek = DateTime.UtcNow.AddDays(-14);
			var lastDay = DateTime.UtcNow.AddDays(-1);
			var previousDay = DateTime.UtcNow.AddDays(-2);

			var logs = (await LogRepository.GetAsync())
				.Where(x =>
					(x.DateTime <= now) &&
					(x.DateTime >= previousYear) &&
					(x.Error != "") &&
					(x.Login == SessionData.Login) &&
					(x.RelatedClass == "Product") &&
					(x.RelatedMethod == "MobileGet")
				);

			var product = (await Repository.GetAsync())
				.Where(x =>
					x.State != ProductState.Deleted &&
					x.PaymentConcession.Concession.Supplier.Login == SessionData.Login
				)
				.Select(x => new 
				{
					Id = x.Id,
					Name = x.Name,
					Price = x.Price,
					LastDay = logs
						.Where(y => 
							(y.DateTime >= lastDay) &&
							(y.Arguments
								.Any(z =>
									z.Name == "Id" &&
									SqlFunctions.StringConvert((decimal)x.Id) == z.Value
								)
							)
						)
						.Count(),
					PreviousDay = logs
						.Where(y =>
							(y.DateTime >= lastDay) &&
							(y.DateTime >= previousDay) &&
							(y.Arguments
								.Any(z =>
									z.Name == "Id" &&
									SqlFunctions.StringConvert((decimal)x.Id) == z.Value
								)
							)
						)
						.Count(),
					LastWeek = logs
						.Where(y =>
							(y.DateTime >= lastWeek) &&
							(y.Arguments
								.Any(z =>
									z.Name == "Id" &&
									SqlFunctions.StringConvert((decimal)x.Id) == z.Value
								)
							)
						)
						.Count(),
					PreviousWeek = logs
						.Where(y =>
							(y.DateTime < lastWeek) &&
							(y.DateTime >= previousWeek) &&
							(y.Arguments
								.Any(z =>
									z.Name == "Id" &&
									SqlFunctions.StringConvert((decimal)x.Id) == z.Value
								)
							)
						)
						.Count(),
					LastMonth = logs
						.Where(y =>
							(y.DateTime >= lastMonth) &&
							(y.Arguments
								.Any(z =>
									z.Name == "Id" &&
									SqlFunctions.StringConvert((decimal)x.Id) == z.Value
								)
							)
						)
						.Count(),
					PreviousMonth = logs
						.Where(y =>
							(y.DateTime < lastMonth) &&
							(y.DateTime >= previousMonth) &&
							(y.Arguments
								.Any(z =>
									z.Name == "Id" &&
									SqlFunctions.StringConvert((decimal)x.Id) == z.Value
								)
							)
						)
						.Count(),
					LastYear = logs
						.Where(y =>
							(y.DateTime >= lastYear) &&
							(y.Arguments
								.Any(z =>
									z.Name == "Id" &&
									SqlFunctions.StringConvert((decimal)x.Id) == z.Value
								)
							)
						)
						.Count(),
					PreviousYear = logs
						.Where(y =>
							(y.DateTime < lastYear) &&
							(y.DateTime >= previousYear) &&
							(y.Arguments
								.Any(z =>
									z.Name == "Id" &&
									SqlFunctions.StringConvert((decimal)x.Id) == z.Value
								)
							)
						)
						.Count()
				})
				.ToList();
			var result = product
				.Select(x => new ProductGetAllByDashboardResult
				{
					Id = x.Id,
					Name = x.Name,
					Price = x.Price,
					LastDay = x.LastDay,
					LastWeek = x.LastWeek,
					LastMonth = x.LastMonth,
					LastYear = x.LastYear
				});

			var resumen = new
			{
				LastDay = product.Sum(x => (int?)x.LastDay) ?? 0,
				LastWeek = product.Sum(x => (int?)x.LastWeek) ?? 0,
				LastMonth = product.Sum(x => (int?)x.LastMonth) ?? 0,
				LastYear = product.Sum(x => (int?)x.LastYear) ?? 0,
				PreviousDay = product.Sum(x => (int?)x.PreviousDay) ?? 0,
				PreviousWeek = product.Sum(x => (int?)x.PreviousWeek) ?? 0,
				PreviousMonth = product.Sum(x => (int?)x.PreviousMonth) ?? 0,
				PreviousYear = product.Sum(x => (int?)x.PreviousYear) ?? 0
			};

			return new ProductGetAllByDashboardResultBase
			{
				Data = result,
				LastDay = resumen.LastDay,
				LastWeek = resumen.LastWeek,
				LastMonth = resumen.LastMonth,
				LastYear = 50, //resumen.LastYear,
				PercentDay = resumen.PreviousDay == 0 ? 0 : 100 - resumen.LastDay * 100 / resumen.PreviousDay,
				PercentWeek = resumen.PreviousWeek == 0 ? 0 : 100 - resumen.LastWeek * 100 / resumen.PreviousWeek,
				PercentMonth = resumen.PreviousMonth == 0 ? 0 : 100 - resumen.LastMonth * 100 / resumen.PreviousMonth,
				PercentYear = resumen.PreviousYear == 0 ? 0 : 100 - resumen.LastYear * 100 / resumen.PreviousYear
			};
		}
		#endregion ExecuteAsync
	}
}

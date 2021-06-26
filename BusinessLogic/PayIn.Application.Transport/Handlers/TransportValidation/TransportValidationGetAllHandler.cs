using PayIn.Application.Dto.Transport.Arguments.TransportValidation;
using PayIn.Application.Dto.Transport.Results.TransportValidation;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using Xp.Domain.Transport;
using static PayIn.Application.Dto.Transport.Results.TransportValidation.TransportValidationGetAllResultBase;

namespace PayIn.Application.Public.Handlers
{
	public class TransportValidationGetAllHandler :
		IQueryBaseHandler<TransportValidationGetAllArguments, TransportValidationGetAllResult>
	{		
		private readonly IEntityRepository<TransportValidation> Repository;
		

		#region Constructors
		public TransportValidationGetAllHandler(			
			IEntityRepository<TransportValidation> repository
		)
		{			
			if (repository == null) throw new ArgumentNullException("repository");
						
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportValidationGetAllResult>> ExecuteAsync(TransportValidationGetAllArguments arguments)
		{
			var now = DateTime.Now;
			var today = now.Date;

			if (arguments.Since.Value > arguments.Until.Value)
				return new TransportValidationGetAllResultBase();

			var since = arguments.Since;
			var until = arguments.Until.AddDays(1);

			var items = (await Repository.GetAsync("Company"));
				

			if (!arguments.Filter.IsNullOrEmpty())
			{
				items = items.Where(x => (
					x.Uid.ToString().Contains(arguments.Filter) || // mejorar usando el int y no el string cuando mostremos el id en pantalla
					x.Company.Concession.Concession.Name.Contains(arguments.Filter)
				));
			}

			//var result = items
			//	.OrderByDescending(x => x.Date)
			//	.Select(x => new
			//	{
			//		Id = x.Id,
			//		Company = x.Company.Concession.Concession,
			//		Date = x.Date,
			//		ValidationType = x.ValidationType,
			//		Line = x.Line,
			//		Station = x.Station,
			//		Vehicle = x.Vehicle,
			//		Sense = x.Sense,
			//		Travellers = x.Travellers,
			//		Price  = x.Price,
			//		Uid  = x.Uid
			//	})
			//	.ToList()
			//	.Select(x => new TransportValidationGetAllResult
			//	{
			//		Id = x.Id,
			//		Company = x.Company.Name,
			//		Date = x.Date,
			//		ValidationType = x.ValidationType,
			//		ValidationTypeAlias = x.ValidationType.ToEnumAlias(),
			//		Line = x.Line,
			//		Station = x.Station,
			//		Vehicle = x.Vehicle,
			//		Sense = x.Sense,
			//		SenseAlias = x.Sense.ToEnumAlias(),
			//		Travellers = x.Travellers,
			//		Price = x.Price,
			//		Uid = x.Uid
			//	});

			var items2 = items
				.Where(x =>
					x.Date < today && x.Date > SqlFunctions.DateAdd("dd", -1 ,today)   //Ayer
				);		

		var items3 = items
				.Where(x =>
				//x.Date.Date == now.Date.AddDays(-7)   //Última semana
				x.Date < today && x.Date > SqlFunctions.DateAdd("dd", -7, today)
				);

			var items4 = items
				.Where(x =>
				//x.Date.Date == now.Date.AddDays(-30) //Último mes
				x.Date < today && x.Date > SqlFunctions.DateAdd("dd", -30,today)
				);

			if (arguments.ConcessionId != null)
				items2 = items2
					.Where(x => x.Company.ConcessionId == arguments.ConcessionId);

			var yesterday = items2
				.GroupBy(x => new {
					Hour = x.Date.Hour
				})
				.Select(x => new ValidationsYesterday
				{
					Hour = x.Key.Hour,
					Value = x.Sum(y =>
						y.Price
					)
				});
			
			var lastWeek = items3
			.GroupBy(x => new
			{
				DayOfWeek = SqlFunctions.DatePart("weekday", x.Date)
			})
			.Select(x => new ValidationsLastWeek
			{
				Date = (x.Select(z => z.Date).FirstOrDefault()),
				Value = x.Sum(y =>
					y.Price
				)
			});

			var lastMonth = items4
				.GroupBy(x => new 
				{
					Year = x.Date.Year,
					Month = x.Date.Month,
					Day = x.Date.Day
				})
				.ToList()
				.Select(x => new ValidationsLastMonth
				{
					Date = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day),
					Value = x.Sum(y =>
					y.Price
				   )
				});

			return new TransportValidationGetAllResultBase
			{								
				ValidationYesterday = yesterday,
				ValidationLastWeek = lastWeek,
				ValidationLastMonth = lastMonth
			};
		}
		#endregion ExecuteAsync
	}
}

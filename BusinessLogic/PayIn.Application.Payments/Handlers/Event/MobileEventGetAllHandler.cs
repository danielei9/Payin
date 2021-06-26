using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Public.Handlers.Main;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileEventGetAllHandler :
		IQueryBaseHandler<MobileEventGetAllArguments, MobileEventGetAllResult>
	{
		[Dependency] public IEntityRepository<Translation> TranslationRepository { get; set; }
		[Dependency] public MobileMainGetAllV4Handler MobileMainGetAllV4Handler { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileEventGetAllResult>> ExecuteAsync(MobileEventGetAllArguments arguments)
		{
			var now = DateTime.UtcNow;
			
			var translations = (await TranslationRepository.GetAsync())
				.Where(y =>
					(y.Language == arguments.Language)
				);

			var result = (await MobileMainGetAllV4Handler.GetPublicEventsAsync(arguments.SystemCardId, arguments.PaymentConcessionId, null, now))
				.Where(x =>
					arguments.Filter == "" ||
					x.Name.Contains(arguments.Filter) ||
					x.Place.Contains(arguments.Filter)
				)
				.Select(x => new
				{
					x.Id,
					Name = translations
						.Where(y => y.EventNameId == x.Id)
						.Select(y => y.Text)
						.FirstOrDefault() ?? x.Name,
					x.Place,
					x.PhotoUrl,
					x.EventStart
				})
				.ToList()
				.Select(x => new MobileEventGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					Place = x.Place,
					PhotoUrl = x.PhotoUrl,
					EventStart = x.EventStart.ToUTC()
				});
			
			return new ResultBase<MobileEventGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

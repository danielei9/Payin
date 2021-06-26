using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Public.Handlers.Main;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileNoticeGetEdictsHandler :
		IQueryBaseHandler<MobileNoticeGetEdictsArguments, MobileNoticeGetEdictsResult>
	{
		[Dependency] public IEntityRepository<Translation> TranslationRepository { get; set; }
		[Dependency] public MobileMainGetAllV4Handler MobileMainGetAllV4Handler { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileNoticeGetEdictsResult>> ExecuteAsync(MobileNoticeGetEdictsArguments arguments)
		{
			var now = DateTime.UtcNow;

			var translations = (await TranslationRepository.GetAsync())
				.Where(y =>
					(y.Language == arguments.Language)
				);

			var result = (await MobileMainGetAllV4Handler.GetPublicEdictsAsync(arguments.SystemCardId, arguments.PaymentConcessionId, null, now))
				.Select(x => new
				{
					x.Id,
					Name = translations
						.Where(y => y.EventNameId == x.Id)
						.Select(y => y.Text)
						.FirstOrDefault() ?? x.Name,
					x.PhotoUrl,
					x.Start
				})
				.ToList()
				.Select(x => new MobileNoticeGetEdictsResult
				{
					Id = x.Id,
					Name = x.Name,
					PhotoUrl = x.PhotoUrl,
					Start = x.Start.ToUTC()
				});

			return new ResultBase<MobileNoticeGetEdictsResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}

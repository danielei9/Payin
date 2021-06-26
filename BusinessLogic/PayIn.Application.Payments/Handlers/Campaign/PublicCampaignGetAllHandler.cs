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
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class PublicCampaignGetAllHandler :
        IQueryBaseHandler<PublicCampaignGetAllArguments, PublicCampaignGetAllResult>
    {
        [Dependency]
        public IEntityRepository<Campaign> Repository { get; set; }
        [Dependency]
        public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<PublicCampaignGetAllResult>> ExecuteAsync(PublicCampaignGetAllArguments arguments)
        {
            var items = (await Repository.GetAsync(
            ))
                .Where(x =>
                    (x.Since <= arguments.Now) &&
                    (x.Until >= arguments.Now) &&
                    (x.State == CampaignState.Active) &&
                    (
                        // Sólo para Propietario o trabajador
                        (x.Concession.Concession.Supplier.Login == SessionData.Login) ||
                        (x.Concession.PaymentWorkers.Any(y => y.Login == SessionData.Login))
                        //) ||
                        //	(x.PaymentConcessionCampaigns
                        //		.Any(y =>
                        //			(
                        //				(y.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
                        //				(y.PaymentConcession.PaymentWorkers.Any(z => z.Login == SessionData.Login))
                        //			) &&
                        //			(y.State == PaymentConcessionCampaignState.Active)
                        //		)
                        //	)
                    ) &&
                    (
                        (arguments.EventId == null) ||
                        (x.TargetEvents.Count() == 0) ||
                        (x.TargetEvents.Any(y => y.Id == arguments.EventId))
                    )
                )
                //.ToList() // Quitar
                ;
            if (!arguments.Filter.IsNullOrEmpty())
                items = items.Where(x =>
                    x.Title.Contains(arguments.Filter)
                )
                //.ToList() // Quitar
                ;
            ;

            var result = items
            .SelectMany(x => x.CampaignLines
                .Select(y => new
                {
                    Id = y.Id,
                    Title = x.Title,
                    Since = y.SinceTime,
                    Until = y.UntilTime,
                    Caducity = x.Until
                })
            )
            .ToList();

            var resultBase = new ResultBase<PublicCampaignGetAllResult>
            {
                Data = result
                    .Select(x => new PublicCampaignGetAllResult
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Since = x.Since.ToUTC(),
                        Until = x.Until.ToUTC(),
                        Caducity = x.Caducity.ToUTC()
                    })
            };
            return resultBase;
        }
        #endregion ExecuteAsync
    }
}

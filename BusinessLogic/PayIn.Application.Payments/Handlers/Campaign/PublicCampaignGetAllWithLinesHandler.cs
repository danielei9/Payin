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
    public class PublicCampaignGetAllWithLinesHandler :
        IQueryBaseHandler<PublicCampaignGetAllWithLinesArguments, PublicCampaignGetAllWithLinesResult>
    {
        [Dependency] public IEntityRepository<Campaign> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<PublicCampaignGetAllWithLinesResult>> ExecuteAsync(PublicCampaignGetAllWithLinesArguments arguments)
        {
            var items = (await Repository.GetAsync())
                .Where(x =>
                    (x.Since <= arguments.Now) &&
                    (x.Until >= arguments.Now) &&
                    (x.State == CampaignState.Active) &&
                    (
                        // Sólo para Propietario o trabajador
                        (x.Concession.Concession.Supplier.Login == SessionData.Login) ||
                        (x.Concession.PaymentWorkers.Any(y => y.Login == SessionData.Login))
                    ) &&
                    (
                        (arguments.EventId == null) ||
                        (x.TargetEvents.Count() == 0) ||
                        (x.TargetEvents.Any(y => y.Id == arguments.EventId))
                    )
                );
            if (!arguments.Filter.IsNullOrEmpty())
                items = items.Where(x =>
                    x.Title.Contains(arguments.Filter)
                );

            var result = items
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.Since,
                    x.Until,
                    Lines = x.CampaignLines
                        .Select(y => new
                        {
                            y.Id,
                            y.Quantity,
                            y.Type,
                            y.SinceTime,
                            y.UntilTime,
                            y.AllProduct,
                            Products = y.Products.Select(z => new PublicCampaignGetAllWithLinesResult_Product { Code = z.Code }),
                            y.AllEntranceType,
                            EntranceTypes = y.EntranceTypes.Select(z => new PublicCampaignGetAllWithLinesResult_EntranceType { Code = z.Code })
                        })
                })
                .ToList();

            var resultBase = new ResultBase<PublicCampaignGetAllWithLinesResult>
            {
                Data = result
                    .Select(x => new PublicCampaignGetAllWithLinesResult
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Since = x.Since.ToUTC(),
                        Until = x.Until.ToUTC(),
                        Lines = x.Lines
                            .Select(y => new PublicCampaignGetAllWithLinesResult_Line
                            {
                                Id = y.Id,
                                Quantity = y.Quantity,
                                Type = y.Type,
                                SinceTime = y.SinceTime.ToUTC(),
                                UntilTime = y.UntilTime.ToUTC(),
                                AllProduct = y.AllProduct,
                                Products = y.Products,
                                AllEntranceType = y.AllEntranceType,
                                EntranceTypes = y.EntranceTypes
                            })
                    })
            };
            return resultBase;
        }
        #endregion ExecuteAsync
    }
}

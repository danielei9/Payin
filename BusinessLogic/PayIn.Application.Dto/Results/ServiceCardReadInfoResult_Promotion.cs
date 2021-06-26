using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
    public class ServiceCardReadInfoResult_Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public XpDateTime EndDate { get; set; }
        public string Concession { get; set; }
        public string Image { get; set; }
        public IEnumerable<ServiceCardReadInfoResult_RechargeTitle> Titles { get; set; }
        public IEnumerable<ServiceCardReadInfoResult_PromotionAction> Actions { get; set; }
    }
}

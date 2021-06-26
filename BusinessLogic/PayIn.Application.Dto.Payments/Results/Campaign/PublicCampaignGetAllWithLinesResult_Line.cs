using PayIn.Common;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
    public class PublicCampaignGetAllWithLinesResult_Line
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public CampaignLineType Type { get; set; }
        public XpTime SinceTime { get; set; }
        public XpTime UntilTime { get; set; }
        public bool AllProduct { get; set; }
        public bool AllEntranceType { get; set; }
        
        public IEnumerable<PublicCampaignGetAllWithLinesResult_Product> Products { get; set; }
        public IEnumerable<PublicCampaignGetAllWithLinesResult_EntranceType> EntranceTypes { get; set; }
    }
}

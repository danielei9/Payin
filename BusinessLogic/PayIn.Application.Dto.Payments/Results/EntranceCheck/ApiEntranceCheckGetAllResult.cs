using PayIn.Common;
using Xp.Common;
using System;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ApiEntranceCheckGetAllResult
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Observations { get; set; }
        public XpDateTime TimeStamp { get; set; }

        public CheckInType Type { get; set; }
        public int EntranceId { get; set; }

        public string TypeAlias { get; set; }
        public string EntranceAlias { get; set; }

        public XpTime TimeStampTime { get; set; }
        public String TimeStampDate { get; set; }

        public String Subtitle { get; set; }
    }
}

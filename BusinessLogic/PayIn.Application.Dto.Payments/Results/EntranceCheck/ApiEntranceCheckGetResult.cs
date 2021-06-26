using PayIn.Common;
using Xp.Common;
using System;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ApiEntranceCheckGetResult
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Observation { get; set; }
        public XpDateTime TimeStamp { get; set; }

        public CheckInType Type { get; set; }
        public int EntraceId { get; set; }

        public string TypeAlias { get; set; }
        public int EntranceAlias { get; set; }

        public XpTime TimeStampTime { get; set; }
        public String TimeStampDate { get; set; }
    }
}


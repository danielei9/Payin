using PayIn.Domain.Transport;
using System;
using static PayIn.Domain.Transport.WhiteList;

namespace PayIn.Application.Transport.Services
{
    public class WhiteListDto
    {
        public long Uid { get; set; }
        public WhiteListSourceType Source { get; set; }
        public WhiteListStateType State { get; set; }
        public int OperationNumber { get; set; }
        public WhiteListOperationType OperationType { get; set; }
        public WhiteListTitleType TitleType { get; set; }
        public decimal Amount { get; set; }
        public DateTime? InclusionDate { get; set; }
        public DateTime? ExclusionDate { get; set; }

        public WhiteList GetWhiteList(DateTime now)
        {
            var item = new WhiteList();
            item.Uid = Uid;
            item.Source = Source;
            item.State = State;
            item.OperationNumber = OperationNumber;
            item.TitleType = TitleType;
            item.InclusionDate = InclusionDate ?? now;
            item.Amount = Amount;
            item.ExclusionDate = ExclusionDate;
            
            return item;
        }
    }
}

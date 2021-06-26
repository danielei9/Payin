using PayIn.Domain.Transport;
using System;
using static PayIn.Domain.Transport.WhiteList;

namespace PayIn.Application.Dto.Transport.Services
{
    public class WhiteListDownloadResult_Item
    {
        public long Uid { get; set; }
        public WhiteListSourceType Source { get; set; }
        public WhiteListStateType State { get; set; }
        public WhiteListOperationType OperationType { get; set; }
        public int OperationNumber { get; set; }
        public WhiteListTitleType TitleType { get; set; }
        public decimal Amount { get; set; }
        public DateTime InclusionDate { get; set; }
        public DateTime? ExclusionDate { get; set; }

        #region Constructors
        public WhiteListDownloadResult_Item(WhiteList item)
        {
            Uid = item.Uid;
            Source = item.Source;
            State = item.State;
            OperationType = item.OperationType;
            OperationNumber = item.OperationNumber;
            TitleType = item.TitleType;
            Amount = item.Amount;
            InclusionDate = item.InclusionDate;
            ExclusionDate = item.ExclusionDate;
        }
        #endregion Constructors
    }
}

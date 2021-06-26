using PayIn.Domain.Transport;
using System;

namespace PayIn.Application.Dto.Transport.Services
{
    public class GreyListDownloadResult_Item
    {
        public long Uid { get; set; }
        public int OperationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public GreyList.ActionType Action { get; set; }
        public string Field { get; set; }
        public string NewValue { get; set; }
        public bool Resolved { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public GreyList.MachineType Machine { get; set; }
        public bool IsConfirmed { get; set; }
        public string CodeReturned { get; set; }
        public string OldValue { get; set; }
        public GreyList.GreyListSourceType Source { get; set; }
        public GreyList.GreyListStateType State { get; set; }

        #region Constructors
        public GreyListDownloadResult_Item(GreyList item)
        {
            Action = item.Action;
            CodeReturned = item.CodeReturned;
            Field = item.Field;
            IsConfirmed = item.IsConfirmed;
            Machine = item.Machine;
            NewValue = item.NewValue;
            OldValue = item.OldValue;
            OperationNumber = item.OperationNumber;
            RegistrationDate = item.RegistrationDate;
            ResolutionDate = item.ResolutionDate;
            Resolved = item.Resolved;
            Source = item.Source;
            State = item.State;
            Uid = item.Uid;
        }
        #endregion Constructors
    }
}

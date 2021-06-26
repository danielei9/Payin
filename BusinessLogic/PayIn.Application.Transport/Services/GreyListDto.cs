using PayIn.Domain.Transport;
using System;
using static PayIn.Domain.Transport.GreyList;

namespace PayIn.Application.Transport.Services
{
    public class GreyListDto
    {
        public long Uid { get; set; }
        public int OperationNumber { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public ActionType Action { get; set; }
        public string Field { get; set; }
        public string NewValue { get; set; }
        public bool Resolved { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public MachineType Machine { get; set; }
        public string OldValue { get; set; }
        public GreyListSourceType Source { get; set; }
        public GreyListStateType State { get; set; }

        public GreyList GetGreyList(DateTime now)
        {
            var item = new GreyList();
            item.Uid = Uid;
            item.OperationNumber = OperationNumber;
            item.RegistrationDate = RegistrationDate ?? now;
            item.Action = Action;
            item.Field = Field;
            item.NewValue = NewValue;
            item.Resolved = Resolved;
            item.ResolutionDate = ResolutionDate;
            item.Machine = Machine;
            item.IsConfirmed = false;
            item.CodeReturned = "";
            item.OldValue = OldValue;
            item.Source = Source;
            item.State = State;

            return item;
        }
    }
}

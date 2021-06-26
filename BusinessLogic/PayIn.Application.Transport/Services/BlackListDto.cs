using PayIn.Domain.Transport;
using System;
using static PayIn.Domain.Transport.BlackList;

namespace PayIn.Application.Transport.Services
{
    public class BlackListDto
    {
        public long Uid { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public bool Resolved { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public BlackListMachineType Machine { get; set; }
        public bool Rejection { get; set; }
        public int Concession { get; set; }
        public BlackListServiceType Service { get; set; }
        public BlackListSourceType Source { get; set; }
        public BlackListStateType State { get; set; }

        public BlackList GetBlackList(DateTime now)
        {
            var item = new BlackList();
            item.Uid = Uid;
            item.RegistrationDate = RegistrationDate ?? now;
            item.Resolved = Resolved;
            item.ResolutionDate = ResolutionDate;
            item.Machine = Machine;
            item.Rejection = Rejection;
            item.Concession = Concession;
            item.Service = Service;
            item.IsConfirmed = false;
            item.CodeReturned = "";
            item.Source = Source;
            item.State = State;

            return item;
        }
    }
}

using PayIn.Domain.Transport;
using System;

namespace PayIn.Application.Dto.Transport.Services
{
    public class BlackListDownloadResult_Item
    {
        public long Uid { get; set; }
        //public DateTime RegistrationDate { get; set; }
        //public bool Resolved { get; set; }
        //public DateTime? ResolutionDate { get; set; }
        //public BlackListMachineType Machine { get; set; }
        //public bool Rejection { get; set; }
        //public int Concession { get; set; }
        //public BlackListServiceType Service { get; set; }
        //public bool IsConfirmed { get; set; }
        //public string CodeReturned { get; set; }
        public BlackList.BlackListSourceType Source { get; set; }
        //public BlackList.BlackListStateType State { get; set; }

        #region Constructors
        public BlackListDownloadResult_Item(BlackList item)
        {
            Uid = item.Uid;
            //RegistrationDate = item.RegistrationDate;
            //Resolved = item.Resolved;
            //ResolutionDate = item.ResolutionDate;
            //Machine = item.Machine;
            //Rejection = item.Rejection;
            //Concession = item.Concession;
            //Service = item.Service;
            //IsConfirmed = item.IsConfirmed;
            //CodeReturned = item.CodeReturned;
            Source = item.Source;
            //State = item.State;
        }
        #endregion Constructors
    }
}

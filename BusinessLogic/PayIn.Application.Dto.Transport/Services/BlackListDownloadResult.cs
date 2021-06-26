using System.Collections.Generic;

namespace PayIn.Application.Dto.Transport.Services
{
    public class BlackListDownloadResult
    {
        public IEnumerable<BlackListDownloadResult_Item> ItemsAdded { get; set; }
        public IEnumerable<BlackListDownloadResult_Item> ItemsUpdated { get; set; }
        public IEnumerable<BlackListDownloadResult_Item> ItemsRemoved { get; set; }
    }
}

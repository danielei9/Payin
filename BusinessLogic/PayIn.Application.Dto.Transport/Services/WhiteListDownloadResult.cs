using System.Collections.Generic;

namespace PayIn.Application.Dto.Transport.Services
{
    public class WhiteListDownloadResult
    {
        public IEnumerable<WhiteListDownloadResult_Item> ItemsAdded { get; set; }
        public IEnumerable<WhiteListDownloadResult_Item> ItemsUpdated { get; set; }
        public IEnumerable<WhiteListDownloadResult_Item> ItemsRemoved { get; set; }
    }
}

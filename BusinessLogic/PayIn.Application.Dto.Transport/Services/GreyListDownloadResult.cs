using System.Collections.Generic;

namespace PayIn.Application.Dto.Transport.Services
{
    public class GreyListDownloadResult
    {
        public IEnumerable<GreyListDownloadResult_Item> ItemsAdded { get; set; }
        public IEnumerable<GreyListDownloadResult_Item> ItemsUpdated { get; set; }
        public IEnumerable<GreyListDownloadResult_Item> ItemsRemoved { get; set; }
    }
}

using PayIn.Common;
using System.Collections.Generic;
using Xp.Common;
using PayIn.Domain.Payments;

namespace PayIn.Application.Dto.Results.Shop
{
    public partial class ShopEventGetAllResult
    {
        public int                 Id            { get; set; }
        public string              Place         { get; set; }
        public string              Name          { get; set; }
        public XpDateTime          EventStart    { get; set; }
        public string              PhotoUrl      { get; set; }
        public List<EntranceType>  EntranceTypes { get; set; }
    }
}

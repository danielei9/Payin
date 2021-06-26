using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results.Shop
{
    public class ShopConcessionGetEventsResult
    {
        public int Id                    { get; set; }
        public string Name               { get; set; }
        public string Place              { get; set; }
        public string PhotoUrl           { get; set; }
        public XpDateTime Date           { get; set; }
        public decimal PriceStart        { get; set; }
        public int ConcessionId          { get; set; }
        public string ConcessionName     { get; set; }
        public string ConcessionPhotoUrl { get; set; }
    }
}
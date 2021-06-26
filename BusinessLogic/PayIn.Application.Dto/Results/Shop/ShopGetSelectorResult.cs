using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results.Shop
{
    public class ShopGetSelectorResult
    {

        public int      Id      { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string PhotoUrl { get; set; }
        public Object Concession { get; set; }
        public XpDateTime Date { get; set; }
        public decimal PriceStart { get; set; }
        public List<Object> Images { get; set; }

    }
}
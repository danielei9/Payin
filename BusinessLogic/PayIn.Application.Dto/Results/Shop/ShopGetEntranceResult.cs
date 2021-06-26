using System;
using Xp.Common;

namespace PayIn.Application.Dto.Results.Shop
{
    public partial class ShopGetEntranceResult
    {
        public int         Id                       { get; set; }
        public string      Name                     { get; set; }
        public string      EntranceDescription      { get; set; }
        public string      EntranceShortDescription { get; set; }
        public decimal     Price                    { get; set; }

        public string      EventName         { get; set; }
        public XpDateTime  EventStart        { get; set; }
        public XpDateTime  EventEnd          { get; set; }
        public string      Place             { get; set; }
        public string      PhotoUrl          { get; set; }
        public string      Description       { get; set; }
        public string      ShortDescription  { get; set; }
        public string      Conditions        { get; set; }
        public int         ConcessionId      { get; set; }
        public String      FormId            { get; set; }
    }
}

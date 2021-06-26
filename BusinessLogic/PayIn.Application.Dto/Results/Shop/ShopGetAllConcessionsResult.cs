using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.Shop
{
	public partial class ShopGetAllConcessionsResult
    {
        public int                                                  Id            { get; set; }
        public string                                               Name          { get; set; }
        public string                                               PhotoUrl      { get; set; }
        public IEnumerable<ShopGetAllConcessionsResult_EventImages> EventImages   { get; set; }
    }
}

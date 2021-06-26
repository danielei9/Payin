using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.Shop
{
    public class ShopGetMyCardsSelectorResult
	{
		public int Id { get; set; }
		public long Uid { get; set; }
		public string Alias { get; set; }
    }
}
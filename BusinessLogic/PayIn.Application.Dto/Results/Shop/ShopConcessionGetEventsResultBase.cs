using System;
using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results.Shop
{
    public class ShopConcessionGetEventsResultBase : ResultBase<ShopConcessionGetEventsResult>
	{
		public string ConcessionPhotoUrl { get; set; }
		public string ConcessionLogoUrl { get; set; }
    }
}
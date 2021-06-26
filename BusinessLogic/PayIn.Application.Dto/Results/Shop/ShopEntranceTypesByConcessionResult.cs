using PayIn.Common;
using System.Collections.Generic;
using Xp.Common;
using PayIn.Domain.Payments;
using System;

namespace PayIn.Application.Dto.Results.Shop
{
    public partial class ShopEventGetEntranceTypesResult
    {
        public int			Id							{ get; set; }
        public string		EntranceName				{ get; set; }       
        //public string		EntranceConditions			{ get; set; }       
        public decimal		EntrancePrice				{ get; set; }
        
		//EventData	-> Usados en ShopEventgetEntranceTypesHandler
        public string		Place						{ get; set; }
        public string		Name						{ get; set; }
        public XpDateTime	EventStart					{ get; set; }
        public string		PhotoUrl					{ get; set; }

        //EntranceTypeData	-> Usados en ShopByConcessionHandler
        public decimal		EntranceExtraPrice			{ get; set; }
		public int			Quantity					{ get; set; }
		//public int		EntranceMax					{ get; set; }
		public int			EntranceSoldByType			{ get; set; }

		public int			EntranceMaxEntrancesPerCard	{ get; set; }
		//public int		MaxEntrancesAvailableByType { get; set; }
		//public int		EntrancesAvailableByType	{ get; set; }
		//public int		EntrancesCount				{ get; set; }
		//public int		MaxEntrance					{ get; set; }
		//public int		EntrancesCountByType		{ get; set; }
	}
}

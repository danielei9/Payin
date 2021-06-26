using System.Collections.Generic;
using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EntranceTypeGetAllResult
	{
		public int						Id						{ get; set; }
		public string					Name					{ get; set; }
		public decimal					Price					{ get; set; }
		public decimal					ExtraPrice				{ get; set; }
        public int						SellEntrances			{ get; set; }
		public int?						MaxEntrance				{ get; set; }
        public decimal?					SellEntrancesPercent	{ get; set; }
		public XpDateTime				SellStart				{ get; set; }
		public XpDateTime				SellEnd					{ get; set; }
		public EntranceTypeState		State					{ get; set; }
		public bool						IsVisible				{ get; set; }
		public decimal					TotalAmount				{ get; set; }
		public decimal					TotalExtraPrice			{ get; set; }
		public int						Forms					{ get; set; }
        public string					Code					{ get; set; }
		public EntranceTypeVisibility	Visibility				{ get; set; }
		public int						GroupsCount				{ get; set; }
	}
}

using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EntranceTypeGetResult
	{
		public int						Id					{ get; set; }
		public bool                     IsVisible           { get; set; }
		public string					Name				{ get; set; }
		public EntranceTypeState		State				{ get; set; }
		public string					Description			{ get; set; }
		public decimal					Price				{ get; set; }
		public string					PhotoUrl			{ get; set; }
		public int?						MaxEntrance			{ get; set; }
		public XpDateTime				SellStart			{ get; set; }
		public XpDateTime				SellEnd				{ get; set; }
		public XpDateTime				CheckInStart		{ get; set; }
		public XpDateTime				CheckInEnd			{ get; set; }
		public decimal					ExtraPrice			{ get; set; }
		public int						EventId				{ get; set; }
		public string                   EventName           { get; set; }
		public int?						RangeMin			{ get; set; }
		public int?						RangeMax			{ get; set; }
        public string                   ShortDescription    { get; set; }
        public string                   Conditions          { get; set; }
        public int                      MaxSendingCount     { get; set; }
		public int?						NumDays				{ get; set; }
		public XpTime					StartDay			{ get; set; }
		public XpTime					EndDay				{ get; set; }
        public string                   Code                { get; set; }
		public EntranceTypeVisibility	Visibility			{ get; set; }

	}
}

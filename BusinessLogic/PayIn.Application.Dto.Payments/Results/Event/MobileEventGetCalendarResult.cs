using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class MobileEventGetCalendarResult
    {
		public int Id							{ get; set; }
		public string Place						{ get; set; }
		public string Name						{ get; set; }
		public XpDateTime EventStart			{ get; set; }
		public XpDateTime EventEnd				{ get; set; }
		public EventState State					{ get; set; }
        // Properties calendar
        public string Foo                       { get; set; }
        public string EventClass                { get; set; }
        public string Date                      { get; set; }
	}
}
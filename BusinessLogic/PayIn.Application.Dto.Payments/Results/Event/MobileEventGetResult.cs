using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class MobileEventGetResult
	{
        public int Id					               { get; set; }
		public string Place				               { get; set; }
		public string Name				               { get; set; }
		public string Description		               { get; set; }
		public string PhotoUrl			               { get; set; }
		public string MapUrl						   { get; set; }
		public string Conditions                       { get; set; }
		public XpDateTime EventStart	               { get; set; }
		public XpDateTime EventEnd		               { get; set; }
		public XpDateTime CheckInStart	               { get; set; }
		public XpDateTime CheckInEnd	               { get; set; }
		public int PaymentConcessionId                 { get; set; }

		public IEnumerable<MobileEventGetResult_EntranceTypes> EntranceTypes { get; set; }
		public IEnumerable<MobileEventGetResult_Activities> Activities { get; set; }
		public IEnumerable<MobileEventGetResult_Images> Images { get; set; }
        public IEnumerable<MobileEventGetResult_Notices> Notices { get; set; }
        public IEnumerable<MobileEventGetResut_Exhibitors> Exhibitors { get; set; }
    }
}

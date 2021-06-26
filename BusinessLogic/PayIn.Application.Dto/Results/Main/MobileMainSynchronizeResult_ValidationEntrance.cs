using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_ValidationEntrance
    {
		public int Id { get; set; }
		public long Code { get; set; }
		public long? Uid { get; set; }
		public string VatNumber { get; set; }
		public string UserName { get; set; }
		public string LastName { get; set; }
		public string EntranceTypeName { get; set; }
		public XpDateTime CheckInStart { get; set; }
		public XpDateTime CheckInEnd { get; set; }
	}
}

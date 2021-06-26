using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class MobileActivityGetByEventResult
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public XpDateTime Start { get; set; }
        public XpDateTime End { get; set; }
	}
}

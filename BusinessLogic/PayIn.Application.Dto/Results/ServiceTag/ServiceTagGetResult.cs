using PayIn.Common;

namespace PayIn.Application.Dto.Results.ServiceTag
{
	public partial class ServiceTagGetResult
	{
		public int            Id           { get; set; }
		public string         Reference    { get; set; }
		public CheckPointType Type         { get; set; }
	}
}

using PayIn.Domain.SmartCity.Enums;
using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiComponentGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Model { get; set; }
		public ComponentType Type { get; set; }
		public string TypeName { get; set; }
		public XpDateTime LastTimestamp { get; set; }
		public int SensorsNumber { get; set; }
	}
}

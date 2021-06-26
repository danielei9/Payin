using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results
{
	public partial class MobileMainGetAllV4ResultBase : ResultBase<MobileMainGetAllV4Result>
	{
		public string IconUrl { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }

		public IEnumerable<MobileMainGetAllV4Result_Event> Events { get; set; }
		public IEnumerable<MobileMainGetAllV4Result_Notice> Notices { get; set; }
	}
}

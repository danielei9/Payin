using System.Collections.Generic;
using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public class MobileServiceIncidenceGetAllResult
	{
		public int						Id				{ get; set; }
		public IncidenceType			Type			{ get; set; }
		public string					TypeName		{ get; set; }
		public IncidenceCategory		Category		{ get; set; }
		public string					CategoryName	{ get; set; }
		public IncidenceSubCategory		SubCategory		{ get; set; }
		public string					SubCategoryName	{ get; set; }
		public XpDateTime				DateTime		{ get; set; }
		public IncidenceState			State			{ get; set; }
		public string					StateName		{ get; set; }
		public string					Name			{ get; set; }
		public IEnumerable<Domain.Public.ServiceNotification> Notifications { get; set; } = new List<Domain.Public.ServiceNotification>();
	}
}
using System.Collections.Generic;
using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public class ServiceIncidenceGetResult
	{
		public string					UserLogin				{ get; set; }
		public string					User_NameLastname		{ get; set; }
		public string					User_Phone				{ get; set; }
		public string					User_Email				{ get; set; }
		public IncidenceType			Type					{ get; set; }
		public string					TypeName				{ get; set; }
		public IncidenceCategory		Category				{ get; set; }
		public string					CategoryName			{ get; set; }
		public IncidenceSubCategory		SubCategory				{ get; set; }
		public string					SubCategoryName			{ get; set; }
		public XpDateTime				DateTime				{ get; set; }
		public IncidenceState			State					{ get; set; }
		public string					StateName				{ get; set; }
		public string					Name					{ get; set; }
		public string					InternalObservations	{ get; set; }
		//public string					PhotoUrl				{ get; set; }
		//public string					UserLogin				{ get; set; }
		//public string					UserName				{ get; set; }
		//public string					UserPhone				{ get; set; }
		public decimal?					Longitude				{ get; set; }
		public decimal?					Latitude				{ get; set; }
		public ICollection<Domain.Public.ServiceNotification> Notifications { get; set; } = new List<Domain.Public.ServiceNotification>();
	}
}
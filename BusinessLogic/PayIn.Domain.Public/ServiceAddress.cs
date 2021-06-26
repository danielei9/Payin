using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceAddress : IEntity
	{
		                                      public int                 Id    { get; set; }
		[Required(AllowEmptyStrings = false)] public string              Name  { get; set; }
		                                      public int?                From  { get; set; }
		                                      public int?                Until { get; set; }
		                                      public ServiceAddressSide? Side  { get; set; }

		#region Names
		// Cascade on delete configured in PublicContext
		public ICollection<ServiceAddressName> Names { get; set; }
		#endregion Names

		#region City
		public int CityId { get; set; }
		public ServiceCity City { get; set; }
		#endregion City

		#region Zone
		public int ZoneId { get; set; }
		public ServiceZone Zone { get; set; }
		#endregion Zone

		#region Constructors
		public ServiceAddress()
		{
			Names = new List<ServiceAddressName>();
		}
		#endregion
	}
}

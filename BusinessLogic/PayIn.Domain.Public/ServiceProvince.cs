using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceProvince : IEntity
	{
		                                      public int    Id   { get; set; }
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }

		#region Country
		public int CountryId { get; set; }
		public ServiceCountry Country { get; set; }
		#endregion Country

		#region Cities
		[InverseProperty("Province")]
		public ICollection<ServiceCity> Cities { get; set; }
		#endregion Cities

		#region Constructors
		public ServiceProvince()
		{
			Cities = new List<ServiceCity>();
		}
		#endregion Constructors
	}
}

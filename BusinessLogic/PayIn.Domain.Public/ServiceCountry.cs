using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceCountry : IEntity
	{
		                                      public int    Id   { get; set; }
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }

		#region Provinces
		[InverseProperty("Country")]
		public ICollection<ServiceProvince> Provinces { get; set; }
		#endregion Provinces

		#region Constructors
		public ServiceCountry()
		{
			Provinces = new List<ServiceProvince>();
		}
		#endregion Constructors
	}
}

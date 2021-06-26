using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PayIn.Common;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceIncidenceCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.incidence.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Display(Name = "resources.incidence.type")]
		public IncidenceType Type { get; set; }

		[Display(Name = "resources.incidence.category")]
		public IncidenceCategory Category { get; set; }

		[Display(Name = "resources.incidence.subCategory")]
		public IncidenceSubCategory SubCategory { get; set; }

		[Display(Name = "resources.incidence.concession")]
		[Required]
		public int ConcessionId { get; set; }

		[Display(Name = "resources.incidence.photoUrl")]
		public string PhotoUrl { get; set; }

		[Display(Name = "resources.incidence.observations")]
		public string Observations { get; set; }

		[Display(Name = "resources.incidence.userLogin")]
		public string UserLogin { get; set; }

		[Display(Name = "resources.incidence.userName")]
		public string UserName { get; set; }

		[Display(Name = "resources.incidence.userPhone")]
		public string UserPhone { get; set; }

		[Display(Name = "resources.incidence.longitude")]
		public decimal? Longitude { get; set; }

		[Display(Name = "resources.incidence.latitude")]
		public decimal? Latitude { get; set; }

		#region Constructors
		public ServiceIncidenceCreateArguments(string name, IncidenceType type, IncidenceCategory category, IncidenceSubCategory subCategory, int concessionId, string photoUrl, string observations, string userLogin, string userName, string userPhone, decimal? longitude, decimal? latitude)
		{
			ConcessionId = concessionId;
			Name = name;
			Type = type;
			Category = category;
			SubCategory = subCategory;
			PhotoUrl = photoUrl;
			Observations = observations;
			UserLogin = userLogin;
			UserName = userName;
			UserPhone = userPhone;
			Longitude = longitude;
			Latitude = latitude;
		}
		#endregion Constructors
	}
}

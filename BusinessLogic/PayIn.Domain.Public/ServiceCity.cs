using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceCity : IEntity
	{
		                                      public int    Id   { get; set; }
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }
        [Precision(9, 6)]                     public decimal? Latitude { get; set; }
        [Precision(9, 6)]                     public decimal? Longitude { get; set; }


        #region Province
        public int ProvinceId { get; set; }
		public ServiceProvince Province { get; set; }
		#endregion Province

		#region Addresses
		[InverseProperty("City")]
		public ICollection<ServiceAddress> Addresses { get; set; }
        #endregion Addresses

        #region Concession
        public int? ConcessionId { get; set; }
        [ForeignKey("ConcessionId")]
        public ServiceConcession Concession { get; set; }
        #endregion Concession

        #region Constructors
        public ServiceCity()
		{
			Addresses = new List<ServiceAddress>();
		}
		#endregion Constructors
	}
}

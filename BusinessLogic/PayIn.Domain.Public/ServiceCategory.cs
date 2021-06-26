using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceCategory : Entity
	{
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }
		public bool AllMembersInSomeGroup { get; set; } = true;
		public bool AMemberInOnlyOneGroup { get; set; } = true;
		public bool AskWhenEmit { get; set; } = true;

		#region DefaultGroupWhenEmit
		public int? DefaultGroupWhenEmitId { get; set; }
		[ForeignKey("DefaultGroupWhenEmitId")]
		public ServiceGroup DefaultGroupWhenEmit { get; set; }
		#endregion DefaultGroupWhenEmit

		#region ServiceConcession
		public int ServiceConcessionId { get; set; }
        [ForeignKey("ServiceConcessionId")]
        public ServiceConcession ServiceConcession { get; set; }
        #endregion ServiceConcession

        #region Groups
        [InverseProperty("Category")]
        public ICollection<ServiceGroup> Groups { get; set; }
        #endregion Groups
    }
}

using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceUser : Entity
	{
		[Required(AllowEmptyStrings = true)]	              public string Name            { get; set; }
		[Required(AllowEmptyStrings = true)]	              public string LastName        { get; set; }
		[Required(AllowEmptyStrings = true)]	              public string Photo           { get; set; }
		[Required(AllowEmptyStrings = true)]	              public string VatNumber       { get; set; }
		[Required(AllowEmptyStrings = true)] [MaxLength(200)] public string Login			{ get; set; }
												              public ServiceUserState State	{ get; set; }
												              public long? Phone			{ get; set; }
		[Required(AllowEmptyStrings = true)]	              public string Address			{ get; set; }
												              public string Email			{ get; set; }
												              public DateTime? BirthDate	{ get; set; }
        [Required(AllowEmptyStrings = true)]                  public string AssertDoc		{ get; set; }
		[Required(AllowEmptyStrings = true)]	              public string Code            { get; set; }
		[Required(AllowEmptyStrings = true)]	              public string Observations    { get; set; }

		#region Concession
		public int ConcessionId { get; set; }
		[ForeignKey("ConcessionId")]
		public ServiceConcession Concession { get; set; }
		#endregion Concession

		#region Card
		public int? CardId { get; set; }
		[ForeignKey("CardId")]
		public ServiceCard Card { get; set; }
		#endregion Card

		#region OnwnerCards
		[InverseProperty("OwnerUser")]
		public ICollection<ServiceCard> OnwnerCards { get; set; }
        #endregion OnwnerCards

        #region Groups
        public ICollection<ServiceGroup> Groups { get; set; } = new List<ServiceGroup>();
        #endregion Groups
    }
}

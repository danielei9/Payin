using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceCard : Entity
	{
                                                              public long Uid { get; set; }
        [Required(AllowEmptyStrings = true)]                  public string UidText { get; set; }
                                                              public ServiceCardState State { get; set; }
        [Required(AllowEmptyStrings = true)]                  public string Alias { get; set; } = "";
		//[DatabaseGenerated(DatabaseGeneratedOption.Computed)] public int LastSeq { get; private set; }
		//[DatabaseGenerated(DatabaseGeneratedOption.Computed)] public decimal LastBalance { get; private set; }
		[Required(AllowEmptyStrings = true)]                  public string OwnerLogin { get; set; } = ""; // es posible que tenga un propietario no afiliado 

		#region Concession
		public int ConcessionId { get; set; }
		public ServiceConcession Concession { get; set; }
		#endregion Concession

		#region OwnerUser
		public int? OwnerUserId { get; set; }
		[ForeignKey("OwnerUserId")]
		public ServiceUser OwnerUser { get; set; }
		#endregion OwnerUser

		#region Users
        // Esto realmente debería ser una relación 1 : 1
		[InverseProperty("Card")]
		public ICollection<ServiceUser> Users { get; set; } = new List<ServiceUser>();
        #endregion Users

        #region LinkedUsers
        [InverseProperty("Card")]
		public ICollection<ServiceUserLink> LinkedUsers { get; set; } = new List<ServiceUserLink>();
        #endregion LinkedUsers

        #region SystemCard
        public int SystemCardId { get; set; }
		[ForeignKey(nameof(ServiceCard.SystemCardId))]
		public SystemCard SystemCard { get; set; }
        #endregion SystemCard

        #region ServiceCardBatch
        public int? ServiceCardBatchId { get; set; }
        [ForeignKey("ServiceCardBatchId")]
        public ServiceCardBatch ServiceCardBatch { get; set; }
        #endregion ServiceCardBatch

        #region Groups
        public ICollection<ServiceGroup> Groups { get; set; } = new List<ServiceGroup>();
		#endregion Groups

		#region Operations
		[InverseProperty("Card")]
		public ICollection<ServiceOperation> Operations { get; set; } = new List<ServiceOperation>();
		#endregion Operations

		#region GetUidText
		public string GetUidText()
        {
            return GetUidText(Uid, ServiceCardBatch?.UidFormat ?? UidFormat.Numeric);
        }
        public static string GetUidText(long uid, UidFormat uidFormat)
        {
            return
                uidFormat == UidFormat.BigEndian ? uid.ToHexadecimalBE() :
                uidFormat == UidFormat.LittleEndian ? uid.ToHexadecimal() :
                uidFormat == UidFormat.Mobilis ? uid.ToString() :
                uid.ToString();
        }
		#endregion GetUidText

		#region GetUidLong
		public static long GetUidLong(string uid, UidFormat uidFormat)
		{
			return
				uidFormat == UidFormat.BigEndian ? uid.FromHexadecimalBE().ToInt32() ?? 0:
				uidFormat == UidFormat.LittleEndian ? uid.FromHexadecimal().ToInt32() ?? 0 :
				uidFormat == UidFormat.Mobilis ? Convert.ToInt64(uid) :
				Convert.ToInt64(uid);
		}
		#endregion GetUidLong
	}
}

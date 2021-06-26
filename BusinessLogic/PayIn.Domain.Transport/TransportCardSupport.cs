using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Transport
{
	public class TransportCardSupport : Entity
	{
		[Required(AllowEmptyStrings = false)]   public string Name { get; set; }
		                                        public int OwnerCode { get; set; }
		                                        public string OwnerName { get; set; }
		                                        public int Type { get; set; }
		                                        public int? SubType { get; set; }
												public TransportCardSupportState State { get; set; }
												public bool UsefulWhenExpired { get; set; }

		#region TransportCardSupportTitleCompatibility
		[InverseProperty("TransportCardSupport")]
		public ICollection<TransportCardSupportTitleCompatibility> TransportCardSupportTitleCompatibility { get; set; }
		#endregion TransportCardSupportTitleCompatibility

		#region Constructors
		public TransportCardSupport()
		{
			TransportCardSupportTitleCompatibility = new List<TransportCardSupportTitleCompatibility>();
		}
		#endregion Constructors

		#region GetUidCrc
		public static string GetUidCrc(string hexadecimal)
		{
			var bytes = hexadecimal.FromHexadecimalBE();
			var integer =
				(
					bytes[3] ^
					bytes[2] ^
					bytes[1] ^
					bytes[0]
				) % 100;

			var result = integer.ToString("00");
			return result;
		}
		#endregion GetUidCrc
	}
}
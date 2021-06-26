using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class NoticeImage : Entity
	{
		[Required(AllowEmptyStrings = false)] public string PhotoUrl { get; set; }

		#region Notice
		public int NoticeId { get; set; }
		[ForeignKey("NoticeId")]
		public Notice Notice { get; set; }
		#endregion Notice
	}
}

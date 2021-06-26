using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Bus
{
	public class Request : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Login       { get; set; }
		[Required(AllowEmptyStrings = true)]  public string UserName    { get; set; }
		                                      public DateTime Timestamp { get; set; }

		#region From
		public int FromId { get; set; }
		[ForeignKey(nameof(FromId))]
		public RequestStop From { get; set; }
		#endregion From

		#region To
		public int ToId { get; set; }
		[ForeignKey(nameof(ToId))]
		public RequestStop To { get; set; }
		#endregion To
	}
}

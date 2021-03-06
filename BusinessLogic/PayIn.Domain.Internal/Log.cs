using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Internal
{
	public class Log : Entity
	{
		[Required]                            public DateTime DateTime      { get; set; }
		[Required]                            public TimeSpan Duration      { get; set; }
		[Required(AllowEmptyStrings = true)]  public string   Login         { get; set; }
        [Required(AllowEmptyStrings = true)]  public string   ClientId      { get; set; }
		[Required(AllowEmptyStrings = true)]  public string   RelatedClass  { get; set; }
		[Required(AllowEmptyStrings = true)]  public string   RelatedMethod { get; set; }
		[Required]                            public long     RelatedId     { get; set; }
		[Required(AllowEmptyStrings = true)]  public string   Error         { get; set; }
		[Required(AllowEmptyStrings = true)]  public string   Result        { get; set; }

        #region Arguments
        [InverseProperty("Log")]
		public ICollection<LogArgument> Arguments { get; set; } = new List<LogArgument>();
        #endregion Arguments
	}
}

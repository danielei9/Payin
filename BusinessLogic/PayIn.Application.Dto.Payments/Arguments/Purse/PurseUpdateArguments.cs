using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Purse
{
	public class PurseUpdateArguments : IArgumentsBase
	{
		                                                                                            public int Id { get; set; }
		[Display(Name = "resources.purse.name")]		[Required(AllowEmptyStrings = false)] 		public string Name { get; set; }
		[Display(Name = "resources.purse.validity")]    [Required]		                            public XpDate Validity { get; set; }
		[Display(Name = "resources.purse.expiration")]	[Required]		                            public XpDate Expiration { get; set; }

		#region Constructor
		public PurseUpdateArguments(int id, string name, XpDate validity, XpDate expiration)
		{
			Id = id;
			Name = name;
			Validity = validity;
			Expiration = expiration;
		}
		#endregion Constructor
	}
}
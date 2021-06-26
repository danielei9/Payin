using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Purse
{
	public class PurseCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.purse.name")] 		[Required]   public string Name { get; set; }		
		[Display(Name = "resources.purse.validity")]	[Required]   public XpDate Validity { get; set; }
		[Display(Name = "resources.purse.expiration")]	[Required]   public XpDate Expiration { get; set; }
		[DataSubType(DataSubType.Image)]		                     public byte[] Image { get; set; }

		#region Constructor
		public PurseCreateArguments(string name, XpDate validity, XpDate expiration, byte[] image)
		{
			Name = name;
			Validity = validity;
			Expiration = expiration;
			Image = image;
		}
		#endregion Constructor
	}
}

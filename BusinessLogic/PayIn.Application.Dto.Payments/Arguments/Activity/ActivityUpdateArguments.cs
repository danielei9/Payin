using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ActivityUpdateArguments : IArgumentsBase
	{
		public int Id { get; set; }

		[Display(Name = "resources.activity.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Display(Name = "resources.activity.start")]
		public XpDateTime Start { get; set; }

		[Display(Name = "resources.activity.end")]
		public XpDateTime End { get; set; }

		[Display(Name = "resources.activity.description")]
		public string Description { get; set; }


		#region Constructors
		public ActivityUpdateArguments(int id, XpDateTime start, XpDateTime end, string description)
		{
			Id = id;
			Name = Name;
			Start = start;
			End = end;
			Description = description;
		}
		#endregion Constructors
	}
}

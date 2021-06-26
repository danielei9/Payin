using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ActivityCreateArguments : IArgumentsBase
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

		public int EventId { get; set; }

		#region Constructors
		public ActivityCreateArguments (int id, XpDateTime start, XpDateTime end, string description, int eventId)
		{
			Id = id;
			Name = Name;
			Start = start;
			End = end;
			Description = description;
			EventId = eventId;
		}
		#endregion Constructors
	}
}

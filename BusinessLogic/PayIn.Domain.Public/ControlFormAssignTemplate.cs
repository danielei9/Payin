using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlFormAssignTemplate : Entity
	{
		[Precision(9, 6)] public decimal? Latitude { get; set; }
		[Precision(9, 6)] public decimal? Longitude { get; set; }

		#region Form
		public int FormId { get; set; }
		public ControlForm Form { get; set; }
		#endregion Form

		#region Check
		public int CheckId { get; set; }
		public ControlTemplateCheck Check { get; set; }
		#endregion Check
	}
}

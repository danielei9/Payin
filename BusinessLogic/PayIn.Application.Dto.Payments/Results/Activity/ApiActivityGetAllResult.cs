using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ApiActivityGetAllResult
	{
		public int Id { get; set; }
		/// <summary>
		/// Nombre de la actividad
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Día y hora cuando comienza la actividad
		/// </summary>
		public XpDateTime Start { get; set; }
		/// <summary>
		/// Día y hora cuando acaba la actividad
		/// </summary>
		public XpDateTime End { get; set; }
		/// <summary>
		/// Descripción de la actividad
		/// </summary>
		public string Description { get; set; }
	}
}

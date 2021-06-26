using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EventGetAllResult
	{
		public int Id							{ get; set; }
		/// <summary>
		/// Lugar dónde se celebra el evento
		/// </summary>
		public string Place						{ get; set; }
		/// <summary>
		/// Nombre del evento
		/// </summary>
		public string Name						{ get; set; }
		/// <summary>
		/// Día y hora cuando comienza el evento
		/// </summary>
		public XpDateTime EventStart			{ get; set; }
		/// <summary>
		/// Día y hora cuando acaba el evento
		/// </summary>
		public XpDateTime EventEnd				{ get; set; }
		/// <summary>
		/// 0 [Deleted], 1 [Active], 2 [Suspended]
		/// </summary>
		/// <remarks>Estado</remarks>
		public EventState State					{ get; set; }
		/// <summary>
		/// Dice si es visible en el portal de ventas
		/// </summary>
		public bool IsVisible					{ get; set; }
		/// <summary>
		/// Indica la recaudación total y real del evento
		/// </summary>
		public decimal TotalAmount				{ get; set; }
		public decimal TotalExtraPrice			{ get; set; }
	}
}
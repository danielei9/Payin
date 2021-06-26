using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;
using System.ComponentModel.DataAnnotations;
using PayIn.Common;

namespace PayIn.Domain.Payments
{
	public class EntranceType : Entity
	{
		[Required(AllowEmptyStrings = false)]		public string					Name				{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string					Description			{ get; set; }
													public decimal					Price				{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string					PhotoUrl			{ get; set; }
													public int						MaxEntrance			{ get; set; } // Este valor admite nulos pero se asigna a int.MaxValue
													public DateTime					SellStart			{ get; set; }
													public DateTime					SellEnd				{ get; set; }
													public DateTime					CheckInStart		{ get; set; }
													public DateTime					CheckInEnd			{ get; set; }
													public decimal					ExtraPrice			{ get; set; }
													public EntranceTypeState		State				{ get; set; }
													public int?						RangeMin			{ get; set; }
													public int?						RangeMax			{ get; set; }
													public bool						IsVisible			{ get; set; }
                                                    public int						MaxSendingCount		{ get; set; }
        [Required(AllowEmptyStrings = true)]        public string					ShortDescription	{ get; set; }
        [Required(AllowEmptyStrings = true)]        public string					Conditions			{ get; set; }
													public int?						NumDay				{ get; set; }
													public DateTime?				StartDay			{ get; set; }
													public DateTime?				EndDay				{ get; set; }
        [Required(AllowEmptyStrings = true)]        public string					Code				{ get; set; }
													public EntranceTypeVisibility	Visibility			{ get; set; }
        [Required(AllowEmptyStrings = true)]        public string                   BodyTemplate        { get; set; } = "";

        #region Event
        public int EventId { get; set; }
		[ForeignKey("EventId")]
		public Event Event { get; set; }
		#endregion Event

		#region Entrances
		[InverseProperty("EntranceType")]
		public ICollection<Entrance> Entrances { get; set; } = new List<Entrance>();
		#endregion Entrances

		#region TicketLine
		[InverseProperty("EntranceType")]
		public ICollection<TicketLine> TicketLines { get; set; } = new List<TicketLine>();
		#endregion TicketLine

		#region EntranceTypeForm
		[InverseProperty("EntranceType")]
		public ICollection<EntranceTypeForm> EntranceTypeForms { get; set; } = new List<EntranceTypeForm>();
		#endregion EntranceTypeForm

		#region CampaignLine
		public ICollection<CampaignLine> CampaignLines { get; set; } = new List<CampaignLine>();
		#endregion CampaignLine
	}
}
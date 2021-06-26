using PayIn.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceNotification : Entity
	{
		public DateTime          CreatedAt            { get; set; }
		public NotificationType  Type	              { get; set; }
		public NotificationState State	              { get; set; }
		public int?              ReferenceId          { get; set; }
		public string            ReferenceClass       { get; set; }
		public string            SenderLogin          { get; set; }
		public string            ReceiverLogin        { get; set; }
		public bool              IsRead               { get; set; } // Deprecado: ya está incluido en el state
        public string            Message              { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string            PhotoUrl             { get; set; } = "";
        [Precision(9, 6)]
        public decimal?          Longitude            { get; set; }
        [Precision(9, 6)]
        public decimal?          Latitude             { get; set; }

        #region Incidence
        public int? IncidenceId { get; set; }
        [ForeignKey(nameof(ServiceNotification.IncidenceId))]
        public ServiceIncidence Incidence { get; set; }
        #endregion Incidence
    }
}

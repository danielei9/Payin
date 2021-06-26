using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Entrance : Entity
	{
													public long Code				{ get; set; }
													public long? Uid				{ get; set; }
													public EntranceState State		{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string VatNumber			{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string UserName			{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string LastName			{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string Email				{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string Login				{ get; set; }
                                                    public int SendingCount         { get; set; }
													public DateTime Timestamp       { get; set; }

		#region Checks
		[InverseProperty("Entrance")]
		public ICollection<Check> Checks { get; set; } = new List<Check>();
		#endregion Checks

		#region EntranceType
		public int? EntranceTypeId { get; set; }
		[ForeignKey("EntranceTypeId")]
		public EntranceType EntranceType { get; set; }
		#endregion EntranceType

		#region Event
		public int EventId { get; set; }
		[ForeignKey("EventId")]
		public Event Event { get; set; }
		#endregion Event

		#region TicketLine
		public int? TicketLineId { get; set; }
		[ForeignKey("TicketLineId")]
		public TicketLine TicketLine { get; set; }
        #endregion TicketLine

        #region FormValues
        [InverseProperty("Entrance")]
        public ICollection<EntranceFormValue> FormValues { get; set; } = new List<EntranceFormValue>();
		#endregion FormValues

		#region Contact
		[InverseProperty("VisitorEntrance")]
		public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
		#endregion Contact

		#region Contructors
		public Entrance()
		{
		}
		public Entrance(long code, EntranceState state, int sendingCount, DateTime now,
			string vatNumber, string userName, string lastName, long? uid, string email, string login,
			int? entranceTypeId, int eventId, int? ticketLineId = null,
			ICollection<EntranceFormValue> formValues = null
		)
			: this()
		{
			Code = code;
			State = state;
			SendingCount = sendingCount;
			Timestamp = DateTime.UtcNow;
			VatNumber = vatNumber;
			UserName = userName;
			LastName = lastName;
			Uid = uid;
			Email = email;
			Login = login;
			EntranceTypeId = entranceTypeId;
			EventId = eventId;
			TicketLineId = ticketLineId;
			FormValues = formValues ?? new List<EntranceFormValue>();
		}
		public Entrance(long code, EntranceState state, int sendingCount, DateTime now,
			string vatNumber, string userName, string lastName, long? uid, string email, string login,
			EntranceType entranceType, TicketLine ticketLine,
			ICollection<EntranceFormValue> formValues = null
		)
			: this()
		{
			Code = code;
			State = state;
			SendingCount = sendingCount;
			Timestamp = DateTime.UtcNow;
			VatNumber = vatNumber;
			UserName = userName;
			LastName = lastName;
			Uid = uid;
			Email = email;
			Login = login;
			EntranceType = entranceType;
			EventId = entranceType.EventId;
			TicketLine = ticketLine;
			FormValues = formValues ?? new List<EntranceFormValue>();
		}
        #endregion Contructors

        #region GenerateBody
        /// <summary>
        /// Generate body to the entrance email.
        /// It has to has loadede: EntranceType.Event
        /// </summary>
        /// <returns></returns>
        public string GenerateBody()
        {
            var body = this.EntranceType.BodyTemplate;
            //var body = string.Empty;
            //using (var reader = new StreamReader(HostingEnvironment.MapPath("~/Views/Entrance/EmailTemplate.htm")))
            //    body = reader.ReadToEnd();
            if (body.IsNullOrEmpty())
                return body;

            body = body
                .Replace("{UserName}", this.UserName + " " + this.LastName)
                .Replace("{EventName}", this.EntranceType.Event.Name)
                .Replace("{EventPlace}", this.EntranceType.Event.Place)
                .Replace("{EventStart}", this.EntranceType.Event.EventStart.ToString())
                .Replace("{EventEnd}", this.EntranceType.Event.EventEnd.ToString())
                .Replace("{EntranceCode}", this.Code.ToString())
                .Replace("{EntranceQr}", "pay[in]/entrance:{{{1}code{1}:{0}}}".FormatString(this.Code, Convert.ToChar(34)));

            return body;
        }
        #endregion GenerateBody
    }
}

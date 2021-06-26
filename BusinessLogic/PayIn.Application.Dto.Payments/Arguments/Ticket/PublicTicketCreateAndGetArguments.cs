using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicTicketCreateAndGetArguments : ICreateArgumentsBase<Ticket>
	{
        /// <summary>
        /// Email de la persona a la que se le hace el ticket
        /// </summary>
		[Required] public string                                                    Email        { get; set; }
        /// <summary>
        /// Login de la persona a la que se le hace el ticket
        /// </summary>
		[Required] public string                                                    Login        { get; set; }
        /// <summary>
        /// Login externo de la persona a la que se le hace el ticket (sirve para integraciones)
        /// </summary>
		           public string                                                    ExternalLogin { get; set; }
        /// <summary>
        /// Referencia o cualquier valor que quiera guardarse junto al ticket
        /// </summary>
		           public string                                                    Reference    { get; set; }
        /// <summary>
        /// Fecha / hora del ticket
        /// </summary>
		[Required] public XpDateTime                                                Date         { get; set; }
        /// <summary>
        /// Empresa que genera el ticket
        /// </summary>
		[Required] public int                                                       ConcessionId { get; set; }
        /// <summary>
        /// Id del evento si el tiquet está asociado a un evento concreto
        /// </summary>
		           public int?                                                      EventId      { get; set; }
        /// <summary>
        /// Tipo de venta
        /// </summary>
		           public TicketType                                                Type         { get; set; }
        /// <summary>
        /// Lineas del ticket
        /// </summary>
		           public IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> Lines        { get; set; }
        
        #region Constructor
        public PublicTicketCreateAndGetArguments(string email, string login, string externalLogin, string reference, DateTime date, int concessionId, int? eventId, IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> lines, TicketType? type)
		{
            Email = email;
			Login = login;
            ExternalLogin = externalLogin ?? "";
			Reference    = reference ?? "";
			Date         = date;
			ConcessionId = concessionId;
			EventId = eventId;
			Lines  = lines ?? new List<MobileTicketCreateAndGetArguments_TicketLine>();
			Type = type ?? TicketType.Ticket;
		}
		#endregion Constructor
	}
}

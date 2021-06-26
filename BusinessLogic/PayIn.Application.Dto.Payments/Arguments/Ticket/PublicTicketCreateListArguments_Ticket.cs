using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class PublicTicketCreateListArguments_Ticket
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
        /// Referencia o cualquier valor que quiera guardarse junto al ticket
        /// </summary>
		           public string                                                    Reference    { get; set; }
        /// <summary>
        /// Fecha / hora del ticket
        /// </summary>
		[Required] public XpDateTime                                                Date         { get; set; }
        /// <summary>
        /// Lineas del ticket
        /// </summary>
		           public IEnumerable<PublicTicketCreateListArguments_TicketLine>   Lines        { get; set; }
    }
}

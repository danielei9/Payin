using Xp.Common.Dto.Arguments;
using System.ComponentModel.DataAnnotations;
using PayIn.Common;


namespace PayIn.Application.Dto.Payments.Arguments.TicketTemplate
{
	public class TicketTemplateCreateArguments : IArgumentsBase
	{
																						public int TicketId { get; set; }
		[Display(Name = "resources.ticketTemplate.name")]	                 [Required] public string Name { get; set; }
		[Display(Name = "resources.ticketTemplate.previousTextPosition")]	            public string PreviousTextPosition { get; set; }
		[Display(Name = "resources.ticketTemplate.backTextPosition")]                   public string BackTextPosition { get; set; }
		[Display(Name = "resources.ticketTemplate.regEx")]                   [Required] public string RegEx { get; set; }		
		[Display(Name = "resources.ticketTemplate.decimalCharDelimiter")]    [Required] public DecimalCharDelimiter DecimalCharDelimiter { get; set; }
		[Display(Name = "resources.ticketTemplate.amountPosition")]          [Required] public int AmountPosition { get; set; }
		[Display(Name = "resources.ticketTemplate.referencePosition")]                  public int? ReferencePosition { get; set; }
		[Display(Name = "resources.ticketTemplate.datePosition")]                       public int? DatePosition { get; set; }
		[Display(Name = "resources.ticketTemplate.dateFormat")]              [Required] public string DateFormat { get; set; }
		[Display(Name = "resources.ticketTemplate.titlePosition")]                      public int? TitlePosition { get; set; }
		[Display(Name = "resources.ticketTemplate.workerPosition")]                     public int? WorkerPosition { get; set; }
		[Display(Name = "resources.ticketTemplate.isGeneric")]                          public bool IsGeneric { get; set; }
	
        #region Constructors
		public TicketTemplateCreateArguments(int ticketId, string name, string previousTextPosition, string backTextPosition, string regEx, int concessionId, DecimalCharDelimiter decimalCharDelimiter, int amountPosition, int? referencePosition, int? datePosition, string dateFormat, int? titlePosition, int? workerPosition, bool isGeneric)
		{
			TicketId = ticketId;
			Name = name;
            PreviousTextPosition = previousTextPosition;
			BackTextPosition = backTextPosition;
            RegEx = regEx;
			DecimalCharDelimiter = decimalCharDelimiter;
            AmountPosition = amountPosition;
            ReferencePosition = referencePosition;
            DatePosition = datePosition;
            DateFormat = dateFormat;
            TitlePosition = titlePosition;
            WorkerPosition = workerPosition;
			IsGeneric = isGeneric;
        }
		#endregion Constructors
	}
}

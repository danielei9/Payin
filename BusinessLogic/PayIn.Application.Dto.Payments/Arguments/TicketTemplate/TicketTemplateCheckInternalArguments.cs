using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.TicketTemplate
{
	public class TicketTemplateCheckInternalArguments : IArgumentsBase
	{
		public string Text { get; set; }
        public string RegEx { get; set; }
		public DecimalCharDelimiter DecimalCharDelimiter { get; set; }
        public int? AmountPosition { get; set; }
        public int? ReferencePosition { get; set; }
        public int? DatePosition { get; set; }
        public string DateFormat { get; set; }
        public int? TitlePosition { get; set; }
        public int? WorkerPosition { get; set; }

        #region Constructors
		public TicketTemplateCheckInternalArguments(string text, string regEx, DecimalCharDelimiter decimalCharDelimiter, int? amountPosition, int? referencePosition, int? datePosition, string dateFormat, int? titlePosition, int? workerPosition)
		{
			Text = text;
            RegEx = regEx;
			DecimalCharDelimiter = decimalCharDelimiter;
            AmountPosition = amountPosition;
            ReferencePosition = referencePosition;
            DatePosition = datePosition;
            DateFormat = dateFormat;
            TitlePosition = titlePosition;
            WorkerPosition = workerPosition;
        }
		#endregion Constructors
	}
}

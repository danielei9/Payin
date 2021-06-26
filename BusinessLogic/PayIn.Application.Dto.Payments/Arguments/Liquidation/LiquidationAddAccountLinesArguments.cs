using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class LiquidationAddAccountLinesArguments : IArgumentsBase
    {
        public int Id { get; set; }
        public IEnumerable<LiquidationAddAccountLinesArguments_Line> Lines { get; set; }

        #region Constructures
        public LiquidationAddAccountLinesArguments(IEnumerable<LiquidationAddAccountLinesArguments_Line> lines)
			: base()
		{
            Lines = lines;
        }
		#endregion Constructures
	}
}

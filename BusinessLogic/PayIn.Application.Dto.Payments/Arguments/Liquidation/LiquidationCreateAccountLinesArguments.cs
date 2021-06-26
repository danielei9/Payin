using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class LiquidationCreateAccountLinesArguments : IArgumentsBase
    {
        public IEnumerable<LiquidationCreateAccountLinesArguments_Line> Lines { get; set; }

		#region Constructures
		public LiquidationCreateAccountLinesArguments(IEnumerable<LiquidationCreateAccountLinesArguments_Line> lines)
			: base()
		{
            Lines = lines;
        }
		#endregion Constructures
	}
}

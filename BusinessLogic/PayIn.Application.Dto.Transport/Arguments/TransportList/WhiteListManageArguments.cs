using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportList
{
	public class WhiteListManageArguments : IArgumentsBase
	{
		[Required]
					public string CardId { get; set; }
					public DateTime ParameterValue { get; set; }
					public MifareOperationResultArguments[] Script { get; set; }


		#region Constructors
		public WhiteListManageArguments(string cardId, MifareOperationResultArguments[] script, DateTime parameterValue)
		{
			CardId = cardId;
			Script = script;
			ParameterValue = parameterValue;
		}
		#endregion Constructors
	}
}

using PayIn.Common;
using PayIn.Domain.Transport;
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
	public class GreyListManageArguments : IArgumentsBase
	{
		[Required]
		public TransportCardType CardType { get; set; }
		[Required]
		public string CardId { get; set; }
		public DateTime ParameterValue { get; set; }
		public MifareOperationResultArguments[] Script { get; set; }
		public int OperationNumber { get; set; }
		public PayIn.Domain.Transport.GreyList.ActionType Action { get; set; }
		public string AffectedCamp { get; set; }
		public string NewValue { get; set; }
		public int EquipmentType { get; set; }


		#region Constructors
		public GreyListManageArguments(TransportCardType cardType, string cardId, MifareOperationResultArguments[] script, DateTime parameterValue, int operationNumber, PayIn.Domain.Transport.GreyList.ActionType action, string affectedCamp, string newValue, int equipmentType)
		{
			CardType = cardType;
			CardId = cardId;
			Script = script;
			ParameterValue = parameterValue;
			OperationNumber = operationNumber;
			Action = action;
			AffectedCamp = affectedCamp;
			NewValue = newValue;
			EquipmentType = equipmentType;
		}
		#endregion Constructors
	}
}

using System.ComponentModel.DataAnnotations;
using System;
using Xp.Common.Dto.Arguments;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation.Internal
{
	public class TransportOperationSearchInternalArguments : IArgumentsBase
	{
		[Required] public string Cards { get; set; }
		public long? Uids
		{
			get
			{
				return Cards.FromHexadecimal().ToInt32();
			}
		}
		// Campos que se quieren guardar sin ser argumentos
		public Dictionary<string, string> Script { get; set; }

		#region Constructors
		public TransportOperationSearchInternalArguments(string cardId)
		{
			Cards = cardId;
		}
		#endregion Constructors
	}
}

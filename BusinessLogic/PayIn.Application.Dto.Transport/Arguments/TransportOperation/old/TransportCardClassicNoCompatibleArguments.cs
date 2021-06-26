using PayIn.Domain.Transport.MifareClassic.Operations;
using System.ComponentModel.DataAnnotations;
using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCard
{
	public class TransportCardClassicNoCompatibleArguments : IArgumentsBase
	{
		[Required] public string Cards { get; set; }
		public long? Uids
		{
			get
			{
				return Cards.FromHexadecimal().ToInt32().Value;
			}
		}

		#region Constructors
		public TransportCardClassicNoCompatibleArguments(string cardId)
		{
			Cards = cardId;
		}
		#endregion Constructors
	}
}

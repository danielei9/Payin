using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportSystem
{
	public class TransportSystemGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CardType { get; set; }
		public int State { get; set; }
	}
}

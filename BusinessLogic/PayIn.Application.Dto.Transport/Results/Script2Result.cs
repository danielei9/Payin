using System.Collections.Generic;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;
using Xp.Domain.Transport.MifareClassic.Operaons;

namespace Xp.Application.Results
{
    public class Script2Result
    {
        public MifareAutenticateOperation Condition { get;set; }
        public CardSystem CardSystem { get; set; }
        public string Keys { get; set; }
		public IEnumerable<IMifareOperation> Script { get; set; }
	}
}

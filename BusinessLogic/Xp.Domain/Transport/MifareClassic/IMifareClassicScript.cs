using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Xp.Domain.Transport.MifareClassic
{
	public interface IMifareClassicScript<TCard>
		where TCard : MifareClassicCard
	{
		TCard Card { get; set; }
		List<IMifareRWOperation> Request { get; set; }
		List<IMifareRWOperation> Response { get; set; }

		void Read(List<IMifareRWOperation> list, Expression<Func<TCard, object>> expression);
		void Write(List<IMifareRWOperation> list, Expression<Func<TCard, object>> expression);
		void Write(List<IMifareRWOperation> list, byte sector, byte block, string data, int? from = null, int? to = null);
		//void Sum(List<IMifareRWOperation> list, byte sector, byte block, string data, int? from = null, int? to = null);
		void Check(List<IMifareRWOperation> list, byte sector, byte block, string data, int? from = null, int? to = null);
		Task<IEnumerable<IMifareOperation>> GetRequestAsync();
		Task<IEnumerable<IMifareOperation>> GetResponseAsync();
	}
}
using PayIn.Application.Transport.Scripts.Mobilis;
using PayIn.Domain.Transport.Eige.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Transport.Services
{
	public class TescService
	{
		#region GetTitleActiveAsync
		public async Task<bool> GetTitleActive1Async(long uid, ITransportCardGetTitleCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1]))
			))
				return false;

			return await GetTitleActiveAsync(script.Card.Tesc.Identificador1.Value);
		}
		public async Task<bool> GetTitleActive2Async(long uid, ITransportCardGetTitleCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[2]))
			))
				return false;

			return await GetTitleActiveAsync(script.Card.Tesc.Identificador2.Value);
		}
		private async Task<bool> GetTitleActiveAsync(int identificador)
		{
			return await Task.Run(() =>
			{
				return identificador == "524D".FromHexadecimal().ToInt16();
			});
		}
		#endregion GetTitleActiveAsync

		#region GetTitleExhaustedDate2Async
		public async Task<DateTime?> GetTitleExhaustedDate1Async(long uid, ITransportCardGetExhaustedDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[script.Card.Tesc.Sector1.Value].Blocks[1]))
			))
				return null;

			return
				(await GetTitleExhaustedDateAsync(script.Card.Tesc.Validez1.Value));
		}
		public async Task<DateTime?> GetTitleExhaustedDate2Async(long uid, ITransportCardGetExhaustedDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[script.Card.Tesc.Sector2.Value].Blocks[1]))
			))
				return null;

			return
				(await GetTitleExhaustedDateAsync(script.Card.Tesc.Validez2.Value));
		}
		public async Task<DateTime?> GetTitleExhaustedDateAsync(DateTime? validez)
		{
			return await Task.Run(() => {
				return validez;
			});
		}
		#endregion GetTitleExhaustedDate2Async
	}
}

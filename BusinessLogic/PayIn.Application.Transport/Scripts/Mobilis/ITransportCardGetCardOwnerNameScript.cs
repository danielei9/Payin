using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetCardOwnerNameScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class ITransportCardGetCardOwnerNameScriptExtension
	{
		public static void AddRequest(this ITransportCardGetCardOwnerNameScript that)
		{
			that.Read(that.Request, x => x.Tarjeta.CodigoEntorno);
			that.Read(that.Request, x => x.Tarjeta.EmpresaPropietaria);
		}
	}
}

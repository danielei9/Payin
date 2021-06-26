using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetCargaTypeNameScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetCargaTypeNameScriptExtension
	{
		public static void AddRequest(this ITransportCardGetCargaTypeNameScript that)
		{
			that.Read(that.Request, x => x.Carga.TipoOperacion1_Operacion);
			that.Read(that.Request, x => x.Carga.TipoOperacion2_Operacion);
		}
	}
}

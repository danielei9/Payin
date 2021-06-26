using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetUserNameScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetUserNameScriptExtension
	{
		public static void AddRequest(this ITransportCardGetUserNameScript that)
		{
			that.Read(that.Request, x => x.Usuario.Nombre);
			that.Read(that.Request, x => x.Usuario.Nombre2);
			that.Read(that.Request, x => x.Usuario.ExtranjeroSinDocumentacion);
			that.Read(that.Request, x => x.Usuario.TipoIdentificacion2);
		}
	}
}

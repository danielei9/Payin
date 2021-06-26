using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetUserDniScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetUserDniScriptExtension
	{
		public static void AddRequest(this ITransportCardGetUserDniScript that)
		{
			that.Read(that.Request, x => x.Usuario.ExtranjeroSinDocumentacion);
			that.Read(that.Request, x => x.Usuario.TipoIdentificacion2);
			that.Read(that.Request, x => x.Usuario.Dni);
			that.Read(that.Request, x => x.Usuario.Dni_Numero);
			that.Read(that.Request, x => x.Usuario.Dni_Letra);
			that.Read(that.Request, x => x.Usuario.Cif_Letra);
			that.Read(that.Request, x => x.Usuario.Cif_Numero);
			that.Read(that.Request, x => x.Usuario.Cif_Letra2);
			that.Read(that.Request, x => x.Usuario.Nombre2);
		}
	}
}

using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeValidacion : MifareClassicElement
	{
		[MifareClassicMemory("VEO",     1, 1,  41,  45)] public EigeInt8 EmpresaOperadora { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VTV",     1, 1,  46,  49)] public EigeBytes TipoValidacion { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  46,  46)] public GenericEnum<EigeTipoValidacion_SentidoEnum> TipoValidacion_Sentido { get { return Get<GenericEnum<EigeTipoValidacion_SentidoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  47,  47)] public GenericEnum<EigeTipoValidacion_TransporteEnum> TipoValidacion_Transporte { get { return Get<GenericEnum<EigeTipoValidacion_TransporteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  48,  48)] public GenericEnum<EigeTipoValidacion_TransbordoEnum> TipoValidacion_Transbordo { get { return Get<GenericEnum<EigeTipoValidacion_TransbordoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  49,  49)] public GenericEnum<EigeTipoValidacion_ViajeroEnum> TipoValidacion_Viajero { get { return Get<GenericEnum<EigeTipoValidacion_ViajeroEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VLEPS",   1, 1,  50,  62)] public EigeInt16 VLEPS { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  50,  57)] public EigeInt8 VLEPS_Ferrocarril_Estacion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  58,  59)] public EigeInt8 VLEPS_Ferrocarril_Vestibulo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  50,  56)] public EigeInt8 VLEPS_Bus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  57,  62)] public EigeInt8 VLEPS_Bus_Convoy { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  50,  56)] public EigeInt8 VLEPS_Autobus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  57,  61)] public EigeInt8 VLEPS_Autobus_Parada { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        1, 1,  60,  60)] public EigeInt8 VLEPS_Autobus_Sentido { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VFH",     1, 1,  63,  87)] public EigeDateTime FechaValidacion { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VNPV",    1, 1,  88,  91)] public EigeInt8 NumeroPersonasViaje { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VMTIA",   1, 1,  92,  93)] public EigeInt8 MaxTransbordosInternos { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VNPT",    1, 1,  94,  97)] public EigeInt8 NumeroPersonasTrasbordo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VCTE",    1, 1,  98,  99)] public EigeInt8 ContadorTransbordosExternos { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VCTI",    1, 1, 100, 101)] public EigeInt8 ContadorTransbordosInternos { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VCVS",    1, 1, 102, 105)] public EigeInt8 ContadorViajerosSalida { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VTRT",    1, 1, 106, 113)] public EigeInt8 TiempoRestanteTransbordo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VBLN",    1, 1, 114, 114)] public EigeBool ListaNegra { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VBLB",    1, 1, 115, 115)] public EigeBool ListaGris { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VBS",     1, 1, 116, 116)] public EigeBool ListaBlanca { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VFD",     1, 1, 117, 117)] public EigeBool FuncionamientoDegradado { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TV2CTA",  4, 0, 110, 110)] public EigeBool Titulo2EnAmpliacion { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("VFDA",    4, 0, 111, 111)] public EigeBool Degradado { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TL",      4, 0, 112, 115)] public EigeBytes LocalizadorListaBlanca { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("ATF2LIB2",4, 0, 116, 119)] public EigeBytes LocalizadorListaGris { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeValidacion(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}

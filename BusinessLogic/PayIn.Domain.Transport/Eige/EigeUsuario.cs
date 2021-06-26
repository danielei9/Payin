using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeUsuario : MifareClassicElement
	{
		[MifareClassicMemory("UN",    2, 1,  1,  112)] public EigeString Nombre { get { return Get<EigeString>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UNA2",  3, 2,  1,  120)] public EigeString Apellidos { get { return Get<EigeString>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UBE",   2, 2,  1,    1)] public EigeBool ExtranjeroSinDocumentacion { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UBDD",  2, 2,  2,    2)] public GenericEnum<EigeSexoEnum> Sexo { get { return Get<GenericEnum<EigeSexoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UN2",   2, 2,  3,   42)] public EigeString Nombre2 { get { return Get<EigeString>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",      2, 2,  3,   10)] public EigeString Cif_Letra2 { get { return Get<EigeString>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UAS",   2, 2,  43,  44)] public EigeBytes ApoyoSensorial { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UDLN",  2, 2,  45,  45)] public EigeBool DesbloqueoListaNegra { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UBN",   2, 2,  46,  46)] public EigeBool TipoIdentificacion2 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UBTP",  2, 2,  47,  47)] public EigeBool TarjetaPruebas { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UBTV",  2, 2,  48,  48)] public EigeBool Vinculado { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UBTA",  2, 2,  49,  49)] public EigeBool Averia { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UFC",   2, 2,  51,  59)] public EigeMesDia Cumpleanyos { get { return Get<EigeMesDia>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UCV",   1, 0,  33,  56)] public EigeInt24 CodigoViajero { get { return Get<EigeInt24>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UCNV",  1, 0,  57,  76)] public EigeBytes CodigoNoViajero { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UCPIN", 2, 2,  60,  79)] public EigeString Pin { get { return Get<EigeString>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UD",    2, 2,  80, 119)] public EigeString Dni { get { return Get<EigeString>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",      2, 2,  80, 111)] public EigeBcd Dni_Numero { get { return Get<EigeBcd>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",      2, 2, 112, 119)] public EigeString Dni_Letra { get { return Get<EigeString>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",      2, 2,  80,  87)] public EigeString Cif_Letra { get { return Get<EigeString>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",      2, 2,  88, 119)] public EigeBcd Cif_Numero { get { return Get<EigeBcd>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UDB",   2, 2, 120, 120)] public EigeBool TieneDomiciliacionBancaria { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UAN",   2, 1, 113, 119)] public EigeBytes AnyoNacimiento { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("UIDI",  1, 0, 117, 118)] public GenericEnum<EigeIdiomaEnum> Idioma { get { return Get<GenericEnum<EigeIdiomaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("T8H",   1, 0, 119, 119)] public EigeBool Tiene8Historicos { get { return Get<EigeBool>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeUsuario(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}

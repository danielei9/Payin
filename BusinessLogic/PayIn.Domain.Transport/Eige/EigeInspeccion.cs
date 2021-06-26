using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeInspeccion : MifareClassicElement
	{
		[MifareClassicMemory("CUCNI", 8, 0,  1,   4)] public EigeInt8 ContadorInspecciones { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUII",  8, 0,  5,   8)] public EigeInt8 ContadorInspeccionesConIncidencia { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUPU",  8, 0,  09, 10)] public EigeInt8 PosicionUltimaInspeccion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Inspección 1
		[MifareClassicMemory("CU1E",  8, 0,  11,  15)] public EigeBytes EmpresaInspeccion1 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV1L", 8, 0,  16,  28)] public EigeBytes LugarInspeccion1 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV1F", 8, 0,  29,  53)] public EigeDateTime FechaHora1 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV1M", 8, 0,  54,  61)] public EigeBytes Incidencia1 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		// Inspección 2
		[MifareClassicMemory("CU2E",  8, 0,  11,  15)] public EigeBytes EmpresaInspeccion2 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV2L", 8, 0,  16,  28)] public EigeBytes LugarInspeccion2 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV2F", 8, 0,  29,  53)] public EigeDateTime FechaHora2 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV2M", 8, 0,  54,  61)] public EigeBytes Incidencia2 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		// Inspección 3
		[MifareClassicMemory("CU3E",  8, 0,  11,  15)] public EigeBytes EmpresaInspeccion3 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV3L", 8, 0,  16,  28)] public EigeBytes LugarInspeccion3 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV3F", 8, 0,  29,  53)] public EigeDateTime FechaHora3 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV3M", 8, 0,  54,  61)] public EigeBytes Incidencia3 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		// Inspección 4
		[MifareClassicMemory("CU4E",  8, 0,  11,  15)] public EigeBytes EmpresaInspeccion4 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV4L", 8, 0,  16,  28)] public EigeBytes LugarInspeccion4 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV4F", 8, 0,  29,  53)] public EigeDateTime FechaHora4 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("CUV4M", 8, 0,  54,  61)] public EigeBytes Incidencia4 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeInspeccion(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}

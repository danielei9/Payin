using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using PayIn.Domain.Transport.Tesc.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Tesc
{
	public class EigeTesc : MifareClassicElement
	{
		[MifareClassicMemory("TESCID1",   0, 1,   0,  15)] public EigeInt16 Identificador1 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCS1",    0, 1,  16,  23)] public EigeInt8 Sector1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCVE1",  15, 0,   0,  15)] public EigeInt16 Version1 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCEN1",  15, 0,  16,  31)] public EigeInt16 Entidad1 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCEM1",  15, 0,  32,  47)] public EigeInt16 Emisor1 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCU1",   15, 0,  48, 111)] public EigeInt64 Usuario1 { get { return Get<EigeInt64>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCVA1",  15, 0, 112, 127)] public TescDate Validez1 { get { return Get<TescDate>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCMAC1", 15, 1,   0,  67)] public EigeBytes Mac1 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }

		[MifareClassicMemory("TESCID2",   0, 2,   0,  15)] public EigeInt16 Identificador2 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCS2",    0, 2,  16,  23)] public EigeInt8 Sector2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCVE2",  15, 0,   0,  15)] public EigeInt16 Version2 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCEN2",  15, 0,  16,  31)] public EigeInt16 Entidad2 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCEM2",  15, 0,  32,  47)] public EigeInt16 Emisor2 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCU2",   15, 0,  48, 111)] public EigeInt64 Usuario2 { get { return Get<EigeInt64>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCVA2",  15, 0, 112, 127)] public TescDate Validez2 { get { return Get<TescDate>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TESCMAC2", 15, 1,   0,  67)] public EigeBytes Mac2 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeTesc(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}

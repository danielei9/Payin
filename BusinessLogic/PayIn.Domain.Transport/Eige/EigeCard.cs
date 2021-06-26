using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Tesc;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeCard : MifareClassicCard
	{
		//public static string MobileKey = "MobileKey_Eige";
		public static int PayinExpendedor = 0;
		public static TimeSpan BlackListResolvedElapsed = TimeSpan.FromDays(0);
		// GreyListResolvedElapsed debe restar 4 días - se deja a 0 para las pruebas de Homologación
#if HOMO || TEST || DEBUG
		public static TimeSpan GreyListResolvedElapsed = TimeSpan.FromHours(0);
#else
		public static TimeSpan GreyListResolvedElapsed = TimeSpan.FromDays(4);
#endif

		public EigeTarjeta Tarjeta { get; private set; }
		public EigeProduccion Produccion { get; private set; }
		public EigeUsuario Usuario { get; private set; }
		public EigeEmision Emision { get; private set; }
		public EigePersonalizacion Personalizacion { get; private set; }
		public EigeCarga Carga { get; private set; }
		public EigeTitulo Titulo { get; private set; }
		public EigeTituloTuiN TituloTuiN { get; private set; }
		public EigeValidacion Validacion { get; private set; }
		public EigeHistorico Historico { get; private set; }
		public EigeInspeccion Inspeccion { get; private set; }
		public EigeTesc Tesc { get; private set; }

		#region Constructors
		public EigeCard(string userName, IMifareClassicHsmService hsm)
			: base(hsm)
		{
			Tarjeta = new EigeTarjeta(this);
			Produccion = new EigeProduccion(this);
			Usuario = new EigeUsuario(this);
			Emision = new EigeEmision(this);
			Personalizacion = new EigePersonalizacion(this);
			Carga = new EigeCarga(this);
			Titulo = new EigeTitulo(this);
			TituloTuiN = new EigeTituloTuiN(this);
			Validacion = new EigeValidacion(this);
			Historico = new EigeHistorico(this);
			Inspeccion = new EigeInspeccion(this);
			Tesc = new EigeTesc(this);
		}
		#endregion Constructors

		#region EsJoven
		public bool EsJoven()
		{
			return
				Tarjeta.Tipo.Value == EigeTipoTarjetaEnum.ViajeroPersonalizado && (
					Tarjeta.Subtipo.Value == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConDni ||
					Tarjeta.Subtipo.Value == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConOtroId);
		}
		#endregion EsJoven

		#region IsTuiN
		public bool IsTuiN()
		{
			return ((Titulo.CodigoTitulo1.Value >= 1271) && (Titulo.CodigoTitulo1.Value <= 1277));
		}
		#endregion IsTuiN

		#region IsTesc
		public bool IsTesc1()
		{
			return (Tesc.Identificador1.Value == "524D".FromHexadecimal().ToInt16());
		}
		public bool IsTesc2()
		{
			return (Tesc.Identificador2.Value == "524D".FromHexadecimal().ToInt16());
		}
		#endregion IsTesc

		#region EsBono1
		public bool EsBono1()
		{
			return true;
		}
		#endregion EsBono1

		#region EsBono2
		public bool EsBono2()
		{
			return false;
		}
		#endregion EsBono2

		#region CheckAsync
		public override async Task<bool> CheckAsync(long uid, MifareClassicBlock block)
		{
			return await Task.Run(() =>
			{
				var result = true;

				if ((block.Value == null) || (block.Value.Length != 16))
					return true;
				else if
					((block.Sector.Number == 0) && (block.Number == 0))
					result = true;
				else if (
					((block.Sector.Number == 5) && (block.Number == 0)) ||
					((block.Sector.Number == 5) && (block.Number == 1))
				)
					if ((Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Bonus) == EigeTitulosActivosEnum.Bonus)
						result = valueCheck(block.Value);
				else if (
					((block.Sector.Number == 6) && (block.Number == 0)) ||
					((block.Sector.Number == 6) && (block.Number == 1))
				)
					if ((Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Monedero) == EigeTitulosActivosEnum.Monedero)
						result = valueCheck(block.Value);
				else if (
					(block.Sector.Number == 9)
				)

				if (IsTuiN())
					result = crcCheckTuin(uid, block.Value);
				else
					result = crcCheck(block.Value);

				//if (IsTesc1())
				//	result = crcCheckTesc1(uid, block.Value);
				//if (IsTesc2())
				//	result = crcCheckTesc2(uid, block.Value);

				return result;
			});
		}
		#endregion CheckAsync

		#region GetValueWithCrcAsync
		public override async Task<byte[]> GetValueWithCrcAsync(long uid, MifareClassicBlock block)
		{
			return await Task.Run(() =>
			{
				var result = block.Value;

				if ((block.Value == null) || (block.Value.Length != 16)) { }
				else if ((block.Sector.Number == 0) && (block.Number == 0)) { }
				else if (
					((block.Sector.Number == 5) && (block.Number == 0)) ||
					((block.Sector.Number == 5) && (block.Number == 1)) ||
					((block.Sector.Number == 6) && (block.Number == 0)) ||
					((block.Sector.Number == 6) && (block.Number == 1))
				)
				{
					// bytes negados
					result[4] = (byte)(result[0] ^ 0xFF);
					result[5] = (byte)(result[1] ^ 0xFF);
					result[6] = (byte)(result[2] ^ 0xFF);
					result[7] = (byte)(result[3] ^ 0xFF);
					// bytes positivos
					result[8] = result[0];
					result[9] = result[1];
					result[10] = result[2];
					result[11] = result[3];
					// bytes positivos
					result[13] = (byte)(result[12] ^ 0xFF);
					result[14] = result[12];
					result[15] = (byte)(result[12] ^ 0xFF);
				}
				else if (
					(block.Sector.Number == 9)
				)
					result[15] = crcCalculateTuin(uid, result);
				else
					result[15] = crcCalculate(result);

				return result;
			});
		}
		#endregion GetValueWithCrcAsync

		#region SetCrc
		public override void SetCrc(byte sectorNumber, byte blockNumber)
		{
				var block = Sectors[sectorNumber].Blocks[blockNumber];

				if ((block.Value == null) || (block.Value.Length != 16)) { }
				else if ((block.Sector.Number == 0) && (block.Number == 0)) { }
				else if (
					((block.Sector.Number == 5) && (block.Number == 0)) ||
					((block.Sector.Number == 5) && (block.Number == 1)) ||
					((block.Sector.Number == 6) && (block.Number == 0)) ||
					((block.Sector.Number == 6) && (block.Number == 1))
				)
				{
					// bytes negados
					block.Value[4] = (byte)(block.Value[0] ^ 0xFF);
					block.Value[5] = (byte)(block.Value[1] ^ 0xFF);
					block.Value[6] = (byte)(block.Value[2] ^ 0xFF);
					block.Value[7] = (byte)(block.Value[3] ^ 0xFF);
					// bytes positivos
					block.Value[8] = block.Value[0];
					block.Value[9] = block.Value[1];
					block.Value[10] = block.Value[2];
					block.Value[11] = block.Value[3];
					// bytes positivos
					block.Value[13] = (byte)(block.Value[12] ^ 0xFF);
					block.Value[14] = block.Value[12];
					block.Value[15] = (byte)(block.Value[12] ^ 0xFF);
				}
				else if (
					(IsTuiN()) &&
					(block.Sector.Number == 9)
				)
					block.Value[15] = crcCalculateTuin(Uid.ToInt32().Value, block.Value);
				else
					block.Value[15] = crcCalculate(block.Value);
		}
		#endregion SetCrc

		#region CheckImportantAsync
		public override async Task<bool> CheckImportantAsync(MifareClassicBlock block)
		{
			return await Task.Run(() =>
			{
				var result = true;

				if ((block.Value == null) || (block.Value.Length != 16))
					return true;
				else if
					((block.Number == 0) && (block.Sector.Number == 0))
					result = true;
				else if (
					((block.Sector.Number == 5) && (block.Number == 0) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Bonus) == EigeTitulosActivosEnum.Bonus)) ||
					((block.Sector.Number == 5) && (block.Number == 1) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Bonus) == EigeTitulosActivosEnum.Bonus)) ||
					((block.Sector.Number == 6) && (block.Number == 0) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Monedero) == EigeTitulosActivosEnum.Monedero)) ||
					((block.Sector.Number == 6) && (block.Number == 1) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Monedero) == EigeTitulosActivosEnum.Monedero))
				)
					result = valueCheck(block.Value);
				else if (
					((block.Sector.Number == 0) && (block.Number == 1)) ||
					((block.Sector.Number == 1) && (block.Number == 0)) ||
					((block.Sector.Number == 1) && (block.Number == 1)) ||
					((block.Sector.Number == 1) && (block.Number == 2)) ||
					((block.Sector.Number == 3) && (block.Number == 1)) ||
					((block.Sector.Number == 3) && (block.Number == 0) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Titulo1) == EigeTitulosActivosEnum.Titulo1)) ||
					((block.Sector.Number == 4) && (block.Number == 0) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Titulo2) == EigeTitulosActivosEnum.Titulo2)) ||
					((block.Sector.Number == 4) && (block.Number == 1) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Titulo2) == EigeTitulosActivosEnum.Titulo2))
				)
					result = crcCheck(block.Value);

				if (
					((block.Sector.Number == 5) && (block.Number == 0) && !Enumerable.SequenceEqual(block.Value, block.Sector.Blocks[1].Value) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Bonus) == EigeTitulosActivosEnum.Bonus)) ||
					((block.Sector.Number == 5) && (block.Number == 1) && !Enumerable.SequenceEqual(block.Value, block.Sector.Blocks[0].Value) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Bonus) == EigeTitulosActivosEnum.Bonus)) ||
					((block.Sector.Number == 6) && (block.Number == 0) && !Enumerable.SequenceEqual(block.Value, block.Sector.Blocks[1].Value) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Monedero) == EigeTitulosActivosEnum.Monedero)) ||
					((block.Sector.Number == 6) && (block.Number == 1) && !Enumerable.SequenceEqual(block.Value, block.Sector.Blocks[0].Value) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Monedero) == EigeTitulosActivosEnum.Monedero)) ||
					((block.Sector.Number == 1) && (block.Number == 1) && !Enumerable.SequenceEqual(block.Value, block.Sector.Blocks[2].Value)) ||
					((block.Sector.Number == 1) && (block.Number == 2) && !Enumerable.SequenceEqual(block.Value, block.Sector.Blocks[1].Value)) ||
					((block.Sector.Number == 4) && (block.Number == 0) && !Enumerable.SequenceEqual(block.Value, block.Sector.Blocks[1].Value) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Titulo2) == EigeTitulosActivosEnum.Titulo2)) ||
					((block.Sector.Number == 4) && (block.Number == 1) && !Enumerable.SequenceEqual(block.Value, block.Sector.Blocks[0].Value) && ((((EigeCard)block.Sector.Card).Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Titulo2) == EigeTitulosActivosEnum.Titulo2))
				)
					result = false;

				return result;
			});
		}
		#endregion CheckImportantAsync

		#region crcCheck
		public static bool crcCheck(byte[] val)
		{
			var result =
				(val == null) ||
				(val.Length != 16) ||
				(crcCalculate(val) == val[15]);
			return result;
		}
		#endregion crcCheck

		#region crcCalculate
		private static readonly ushort[] crcTable = {
			 0x0000,
			0x1189,
			0x2312,
			0x329b,
			0x4624,
			0x57ad,
			0x6536,
			0x74bf,
			 0x8c48,
			0x9dc1,
			0xaf5a,
			0xbed3,
			0xca6c,
			0xdbe5,
			0xe97e,
			0xf8f7,
			 0x1081,
			0x0108,
			0x3393,
			0x221a,
			0x56a5,
			0x472c,
			0x75b7,
			0x643e,
			 0x9cc9,
			0x8d40,
			0xbfdb,
			0xae52,
			0xdaed,
			0xcb64,
			0xf9ff,
			0xe876,
			 0x2102,
			0x308b,
			0x0210,
			0x1399,
			0x6726,
			0x76af,
			0x4434,
			0x55bd,
			 0xad4a,
			0xbcc3,
			0x8e58,
			0x9fd1,
			0xeb6e,
			0xfae7,
			0xc87c,
			0xd9f5,
			 0x3183,
			0x200a,
			0x1291,
			0x0318,
			0x77a7,
			0x662e,
			0x54b5,
			0x453c,
			 0xbdcb,
			0xac42,
			0x9ed9,
			0x8f50,
			0xfbef,
			0xea66,
			0xd8fd,
			0xc974,
			 0x4204,
			0x538d,
			0x6116,
			0x709f,
			0x0420,
			0x15a9,
			0x2732,
			0x36bb,
			 0xce4c,
			0xdfc5,
			0xed5e,
			0xfcd7,
			0x8868,
			0x99e1,
			0xab7a,
			0xbaf3,
			 0x5285,
			0x430c,
			0x7197,
			0x601e,
			0x14a1,
			0x0528,
			0x37b3,
			0x263a,
			 0xdecd,
			0xcf44,
			0xfddf,
			0xec56,
			0x98e9,
			0x8960,
			0xbbfb,
			0xaa72,
			 0x6306,
			0x728f,
			0x4014,
			0x519d,
			0x2522,
			0x34ab,
			0x0630,
			0x17b9,
			 0xef4e,
			0xfec7,
			0xcc5c,
			0xddd5,
			0xa96a,
			0xb8e3,
			0x8a78,
			0x9bf1,
			 0x7387,
			0x620e,
			0x5095,
			0x411c,
			0x35a3,
			0x242a,
			0x16b1,
			0x0738,
			 0xffcf,
			0xee46,
			0xdcdd,
			0xcd54,
			0xb9eb,
			0xa862,
			0x9af9,
			0x8b70,
			 0x8408,
			0x9581,
			0xa71a,
			0xb693,
			0xc22c,
			0xd3a5,
			0xe13e,
			0xf0b7,
			 0x0840,
			0x19c9,
			0x2b52,
			0x3adb,
			0x4e64,
			0x5fed,
			0x6d76,
			0x7cff,
			 0x9489,
			0x8500,
			0xb79b,
			0xa612,
			0xd2ad,
			0xc324,
			0xf1bf,
			0xe036,
			 0x18c1,
			0x0948,
			0x3bd3,
			0x2a5a,
			0x5ee5,
			0x4f6c,
			0x7df7,
			0x6c7e,
			 0xa50a,
			0xb483,
			0x8618,
			0x9791,
			0xe32e,
			0xf2a7,
			0xc03c,
			0xd1b5,
			 0x2942,
			0x38cb,
			0x0a50,
			0x1bd9,
			0x6f66,
			0x7eef,
			0x4c74,
			0x5dfd,
			 0xb58b,
			0xa402,
			0x9699,
			0x8710,
			0xf3af,
			0xe226,
			0xd0bd,
			0xc134,
			 0x39c3,
			0x284a,
			0x1ad1,
			0x0b58,
			0x7fe7,
			0x6e6e,
			0x5cf5,
			0x4d7c,
			 0xc60c,
			0xd785,
			0xe51e,
			0xf497,
			0x8028,
			0x91a1,
			0xa33a,
			0xb2b3,
			 0x4a44,
			0x5bcd,
			0x6956,
			0x78df,
			0x0c60,
			0x1de9,
			0x2f72,
			0x3efb,
			 0xd68d,
			0xc704,
			0xf59f,
			0xe416,
			0x90a9,
			0x8120,
			0xb3bb,
			0xa232,
			 0x5ac5,
			0x4b4c,
			0x79d7,
			0x685e,
			0x1ce1,
			0x0d68,
			0x3ff3,
			0x2e7a,
			 0xe70e,
			0xf687,
			0xc41c,
			0xd595,
			0xa12a,
			0xb0a3,
			0x8238,
			0x93b1,
			 0x6b46,
			0x7acf,
			0x4854,
			0x59dd,
			0x2d62,
			0x3ceb,
			0x0e70,
			0x1ff9,
			 0xf78f,
			0xe606,
			0xd49d,
			0xc514,
			0xb1ab,
			0xa022,
			0x92b9,
			0x8330,
			 0x7bc7,
			0x6a4e,
			0x58d5,
			0x495c,
			0x3de3,
			0x2c6a,
			0x1ef1,
			0x0f78
		};
		public static byte crcCalculate(byte[] val)
		{
			var crcval = 0xFFFF;
			for (var i = 0; i < 15; i++)
				crcval = (crcval >> 8) ^ crcTable[(crcval ^ val[i]) & 0xff];

			var crc = (byte)(((crcval & 0xFF00) >> 8) ^ (crcval & 0xFF));

			return crc;
		}
		#endregion crcCalculate

		#region crcCalculateTuin
		private bool crcCheckTuin(long uid, byte[] val)
		{
			return
				(val == null) ||
				(val.Length != 16) ||
				(crcCalculateTuin(uid, val) == val[15]);
		}
		private byte crcCalculateTuin(long uid, byte[] val)
		{
			var crc = crcCalculate(val);
			foreach (var byte_ in uid.ToString("X").FromHexadecimal())
				crc ^= byte_;

			return crc;
		}
		#endregion crcCalculateTuin

		// #region crcCheckTesc
		// private bool crcCheckTesc1(long uid, byte[] val)
		// {
		// 	return
		// 		(val == null) ||
		// 		(val.Length != 16) ||
		// 		(crcCalculateTesc(uid, val).ToHexadecimal() == Tesc.Mac1.Value.ToHexadecimal());
		// }
		// private bool crcCheckTesc2(long uid, byte[] val)
		// {
		// 	return
		// 		(val == null) ||
		// 		(val.Length != 16) ||
		// 		(crcCalculateTesc(uid, val).ToHexadecimal() == Tesc.Mac2.Value.ToHexadecimal());
		// }
		// #endregion crcCheckTesc

		// #region crcCalculateTesc
		// private byte[] crcCalculateTesc(long uid, byte[] val)
		// {
		// 	var UidBytes = uid.ToString("X").FromHexadecimal();

		// 	// Get data
		// 	var data = new byte[24];
		// 	Array.Copy(UidBytes, 0, data, 0, 4);
		// 	Array.Copy(val, 0, data, 4, 16);
		// 	data[20] = 0x80;
		// 	data[21] = 0x00;
		// 	data[22] = 0x00;
		// 	data[23] = 0x00;

		// 	var kmac = new byte[]
		// 	{
		// 		0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
		// 		0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
		// 	};

		// 	var des = MACTripleDES.Create();
		// 	des.Initialize();
		// 	des.Key = kmac;
		// 	var result = des.ComputeHash(data);

		// 	return result;
		// }
		// #endregion crcCalculateTesc

		// #region diversifyKey
		// private byte[] diversifyKey(long uid, byte[] val)
		// {
		// 	var UidBytes = uid.ToString("X").FromHexadecimal();

		// 	// Get data
		// 	var data = new byte[8];
		// 	data[0] = 0x63; // c
		// 	data[1] = 0x73; // s
		// 	Array.Copy(UidBytes, 0, data, 2, 4);
		// 	data[6] = 0x65; // e
		// 	data[7] = 0x54; // T

		// 	var des = MACTripleDES.Create();
		// 	des.Initialize();
		// 	des.Key = new byte[] { 0x54, 0x45, 0x53, 0x43, 0x31, 0x36 }; // TESC16
		// 	var ciphered = des.ComputeHash(data);

		// 	var result = new byte[6];
		// 	Array.Copy(ciphered, 0, result, 0, 6);

		// 	return result;
		// }
		// #endregion diversifyKey

		#region valueCheck
		private bool valueCheck(byte[] val)
		{
			return
				(val == null) ||
				(val.Length != 16) ||
				(
					// bytes negados
					val[0] == (val[4] ^ 0xFF) &&
					val[1] == (val[5] ^ 0xFF) &&
					val[2] == (val[6] ^ 0xFF) &&
					val[3] == (val[7] ^ 0xFF) &&
					// bytes positivos
					val[0] == val[8] &&
					val[1] == val[9] &&
					val[2] == val[10] &&
					val[3] == val[11] &&
					// bytes positivos
					val[13] == (val[12] ^ 0xFF) &&
					val[14] == val[12] &&
					val[15] == (val[12] ^ 0xFF)
				);
		}
		#endregion valueCheck

		#region BlockBackup
		public override void BlockBackup(byte sectorNumber, byte blockNumber)
		{
				var block = Sectors[sectorNumber].Blocks[blockNumber];
				
				if (block.Value.Length != 16)
					return;
				
				if (block.OldValue == null)
					return;
				
				//Slot 1
				if ((sectorNumber == 1) && (blockNumber == 1))
					Sectors[1].Blocks[2].Set((byte[])block.Value.Clone());
				//Slot 2
				else if ((sectorNumber == 4) && (blockNumber == 0))
					Sectors[4].Blocks[1].Set((byte[])block.Value.Clone());
				//Tuin
				else if ((IsTuiN()) && (sectorNumber == 9) && (blockNumber == 0))
					Sectors[9].Blocks[1].Set((byte[])block.Value.Clone());
		}
		#endregion BlockBackup
	}
}
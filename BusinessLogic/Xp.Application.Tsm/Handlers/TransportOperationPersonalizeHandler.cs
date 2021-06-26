using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Hsm;
using Xp.Application.Tsm.GlobalPlatform.Commands;
using Xp.Application.Tsm.GlobalPlatform.Constants;
using Xp.Application.Tsm.GlobalPlatform.Utilities;
using Xp.Application.Tsm.Mifare4Mobile.Commands;

namespace PayIn.Application.Public.Handlers
{
	public class TransportOperationPersonalizeHandler :
		IServiceBaseHandler<TransportOperationPersonalizeArguments>
	{
		// Select
		private byte[] SdAid = "A0 00 00 03 96 4D 34 4D A4 00 81 DB 69 01 00 01".FromHexadecimal();
		private byte[] Aid = "A0 00 00 03 96 4D 34 4D 24 00 81 DB 69 02 00 01".FromHexadecimal();

		// InitializeUpdate
		protected const byte KeyId = 0x00;
		protected const ScpMode Scp = ScpMode.SCP_03_6D;
		protected const SecLevel2 SecurityLevel = SecLevel2.C_ENC | SecLevel2.C_MAC | SecLevel2.R_ENC | SecLevel2.R_MAC;
		//protected List<ISCKey> Keys { get; set; } = new List<ISCKey> {
		//	new SCGPKey(0x48, 0x01, KeyType.AES_CBC, "60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F".FromHexadecimal()),
		//	new SCGPKey(0x48, 0x02, KeyType.AES_CBC, "60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F".FromHexadecimal()),
		//	new SCGPKey(0x48, 0x03, KeyType.AES_CBC, "60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F".FromHexadecimal())
		//};

		private IMifare4MobileHsm Hsm { get; set; }

		#region Constructors
		public TransportOperationPersonalizeHandler(
			IMifare4MobileHsm hsm
		)
		{
			if (hsm == null) throw new ArgumentNullException("hsm");

			Hsm = hsm;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportOperationPersonalizeArguments arguments)
		{
			//var state = new TsmSelectState { Script = await GetPersonalizeScriptAsync() };
			//result = (await SelectHandler.CreateAsync(arguments, state)).Data.FirstOrDefault();

			var result = new ResultBase<string>()
			{
				Data = new List<string>
				{
					Select(arguments).ToHexadecimal(),
					InitializeUpdate(arguments).ToHexadecimal(),
					//ExternalAuthenticate(arguments).ToHexadecimalWithMac_ExternalAuthenticate(state.Session, Scp),
					//InstallForPerso(arguments).ToHexadecimalWithMac_ExternalAuthenticate(state.Session, Scp),
				}
			};
			return result;
		}
		#endregion ExecuteAsync

		#region GetPersonalizeScriptAsync
		private async Task<List<byte[]>> GetPersonalizeScriptAsync()
		{
			return new List<byte[]>
			{
				(await (
					new M4mUpdateSmMetadataCommand()
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mAddAndUpdateMdacCommand()
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 0)
					{
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "FF078069".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 1)
					{
						//Block0 = "3110C0D20000000000200000000040D8".FromHexadecimal(),
						Block0 = "3110C4D20000000000200000000040D8".FromHexadecimal(),
						Block1 = "010000000A000000000000000000004C".FromHexadecimal(),
						Block2 = "010000000A000000000000000000004C".FromHexadecimal(),
						AccessConditions = "7E178869".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 2)
					{
						Block0 = "0000000018A83083559A00842D010070".FromHexadecimal(),
						Block1 = "20202020202020202020202020200048".FromHexadecimal(),
						Block2 = "808080808000030000000000000018A4".FromHexadecimal(),
						AccessConditions = "78778869".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 3)
					{
						Block0 = "01C200A0074000000020FCDFDF04004D".FromHexadecimal(),
						Block1 = "0A77E82A0D000000000000000000008D".FromHexadecimal(),
						Block2 = "2020202020202020202020202020206B".FromHexadecimal(),
						AccessConditions = "78778869".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 4)
					{
						Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "7F078869".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 5)
					{
						Block0 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
						Block1 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "7F078869".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 6)
					{
						Block0 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
						Block1 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "4C378B69".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 7)
					{
						Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "7F078869".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 8)
					{
						Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "78778869".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 9)
					{
						Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "FF078069".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 10)
					{
						Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "FF078069".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 11)
					{
						Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "FF078069".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 12)
					{
						Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "FF078069".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 13)
					{
						Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "FF078069".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 14)
					{
						Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "FF078069".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mUpdateMifareClassicSectorCommand(Hsm, 15)
					{
						Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
						Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
						AccessConditions = "FF078069".FromHexadecimal()
					}
				).ToHexadecimalAsync()).FromHexadecimal()
			};
		}
		#endregion GetPersonalizeScriptAsync

		#region Select
		private ApduCommand Select(TransportOperationPersonalizeArguments arguments)
		{
			Debug.WriteLine("CardRandomId: " + arguments.CardRandomId);

			// Put RandomId in AID
			if (!arguments.CardRandomId.IsNullOrEmpty())
			{
				Aid[Aid.Length - 1] = (arguments.CardRandomId.FromHexadecimal())[0];
				SdAid[SdAid.Length - 1] = (arguments.CardRandomId.FromHexadecimal())[0];
			}

			Debug.WriteLine("AID: " + SdAid.ToHexadecimal());
			Debug.WriteLine("SD AID: " + SdAid.ToHexadecimal());

			var command = new ApduCommand(
				TsmApduCla.COMMAND_ISO,
				TsmApduIns.SELECT,
				TsmApduP1.SELECT_ByName,
				TsmApduP2.SELECT_First,
				SdAid);

			return command;
		}
		#endregion Select

		#region InitializeUpdate
		public ApduCommand InitializeUpdate(TransportOperationPersonalizeArguments arguments)
		{
			var hostChallenge = RandomGenerator.generateRandom(8);

			var command = new ApduCommand(
				TsmApduCla.COMMAND_GP, // 0x80
				TsmApduIns.INITIALIZE_UPDATE, // 0x50
				TsmApduP1.INITIALIZE_UPDATE, // 0x48,
				KeyId,
				hostChallenge
			);
			return command;
		}
		#endregion InitializeUpdate

		#region ExternalAuthenticate
		public ApduCommand ExternalAuthenticate(TransportOperationPersonalizeArguments arguments)
		{
			var command = new ApduCommand(
				TsmApduCla.GET_CHALLENGE, // 0x84
				TsmApduIns.EXTERNAL_AUTHENTICATE, // 0x82
				(byte) SecurityLevel,
				0x00,
				"".FromHexadecimal() //cryptograms.HostCrypto
			);

			return command;
		}
		#endregion ExternalAuthenticate

		#region InstallForPerso
		public ApduCommand InstallForPerso(TransportOperationPersonalizeArguments arguments)
		{
			var result = new byte[6 + Aid.Length];
			var i = 0;
			result[i++] = 0x00;
			result[i++] = 0x00;
			result[i++] = (byte)Aid.Length;
			result.Copy(Aid, 0, Aid.Length, i);
			i = i + Aid.Length;
			result[i++] = 0x00;
			result[i++] = 0x00;
			result[i++] = 0x00;

			var command = new ApduCommand(
				(SecurityLevel == 0) ?
					TsmApduCla.COMMAND_GP : // 0x80
					TsmApduCla.GET_CHALLENGE, // 0x84
				TsmApduIns.INSTALL, // 0xE6
				TsmApduP1.INSTALL_ForPerso, // 0x20
				TsmApduP2.PERSO, // 0x00
				result,
				(SecurityLevel != 0) ?
					(byte)(result.Length + 8) :
					(byte)(result.Length)
			);

			return command;
		}
		#endregion InstallForPerso
	}
}
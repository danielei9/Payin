using PayIn.Application.Tsm.Services.CommChannel;
using System;
using Xp.Application.Tsm.GlobalPlatform;
using Xp.Application.Tsm.GlobalPlatform.Commands;
using Xp.Application.Tsm.GlobalPlatform.Config;
using Xp.Application.Tsm.GlobalPlatform.SmartCardIo;

namespace PayIn.Application.Tsm.Services.OpalExtensions
{
	public class M4MCommands : GP2xCommands, ICommands
	{
		public M4MCommands()
		{
		}
		public M4MCommands(HttpCardChannel channel)
		{
		}
		public ResponseAPDU InstallForPerso(byte[] aid)
		{
			/*if (aid == null)
				throw new ArgumentException("aid must be not null");
			
			byte headerSize = (byte)5; // CLA + INS + P1 + P2 + LC
			byte dataSize = (byte)(6 + aid.Length); 

			if (SecMode != SecLevel.NO_SECURITY_LEVEL) {
				dataSize = (byte) (dataSize + 8); // add space for the C-MAC
			}

			byte[] installForPerso = new byte[(headerSize + (byte)(dataSize & 0xFF))];

			installForPerso[(int)ISO7816.OFFSET_CLA] = (byte) ((SecMode == SecLevel.NO_SECURITY_LEVEL) ? 0x80 : 0x84);
			installForPerso[(int)ISO7816.OFFSET_INS] = 0xE6; // (INS) INSTALL command
			installForPerso[(int)ISO7816.OFFSET_P1] = 0x20; // (P1) For perso
			installForPerso[(int)ISO7816.OFFSET_P2] = 0x00; // (P2)
			installForPerso[(int)ISO7816.OFFSET_LC] = dataSize; // (LC) data length
			installForPerso[5] = 0x00;
			installForPerso[6] = 0x00;
			installForPerso[7] = (byte)(aid.Length & 0xFF);
			installForPerso.Copy(aid, 0, aid.Length, 8); // put the AID into installForPersoComm
			int i = 8 + aid.Length; // next index of installForPersoComm to deal with

			installForPerso[i++] = 0x00;
			installForPerso[i++] = 0x00;
			installForPerso[i++] = 0x00;

			if (SecMode == SecLevel.C_MAC) {
				byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
				dataCmac.Copy(installForPerso, 0, dataCmac.Length); // data used to generate C-MAC
				byte[] cmac = GenerateMac(dataCmac); // generate C-MAC
				installForPerso.Copy(cmac, 0, cmac.Length, dataCmac.Length); // put C-MAC into installForPersoComm
			}

			if (
				(SecMode == SecLevel.C_ENC_AND_MAC) &&
				(Scp != SCPMode.SCP_03_65) &&
				(Scp != SCPMode.SCP_03_6D) &&
				(Scp != SCPMode.SCP_03_05) &&
				(Scp != SCPMode.SCP_03_0D) &&
				(Scp != SCPMode.SCP_03_2D) &&
				(Scp != SCPMode.SCP_03_25)
			) {
				byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
				dataCmac.Copy(installForPerso, 0, dataCmac.Length); // data used to generate C-MAC
				byte[] cmac = GenerateMac(dataCmac); // generate C-MAC
				installForPerso.Copy(cmac, 0, cmac.Length, dataCmac.Length); // put C-MAC into installForPersoComm
				installForPerso = EncryptCommand(installForPerso);
			}

			if (
				(SecMode == SecLevel.C_ENC_AND_MAC) ||
				(SecMode == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) ||
				(SecMode == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)
			)
			{
				if (
					Scp == SCPMode.SCP_03_65 ||
					Scp == SCPMode.SCP_03_6D ||
					Scp == SCPMode.SCP_03_05 ||
					Scp == SCPMode.SCP_03_0D ||
					Scp == SCPMode.SCP_03_2D ||
					Scp == SCPMode.SCP_03_25
				)
				{
					byte[] dataToEnc = new byte[headerSize + dataSize - 8]; // data to encrypt without 8 bytes of CMAC
					dataToEnc.Copy(installForPerso, 0, dataToEnc.Length); // data used to generate C-MAC

					byte[] encInstallForPersoComm = EncryptCommand(dataToEnc);
					byte[] cmac = GenerateMac(encInstallForPersoComm); // generate C-MAC
					installForPerso = new byte[encInstallForPersoComm.Length + 8];
					installForPerso.Copy(encInstallForPersoComm, 0, encInstallForPersoComm.Length);
					installForPerso.Copy(cmac, 0, cmac.Length, encInstallForPersoComm.Length);
				}
			}

			ApduCommand cmdInstallforperso = new ApduCommand(installForPerso);
			ResponseAPDU resp = CardChannel.Transmit(cmdInstallforperso);

			if (resp.Sw != (int)ISO7816.SW_NO_ERROR)
				throw new ApplicationException("Error in INSTALL FOR PERSO : " + resp.Sw.ToHexadecimal());

			//this.compudeAndVerifyRMac(resp.getBytes());

			if (SecMode == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)
			{
				byte[] plainCommand = DecryptCardResponseData(resp.Bytes);
				if (plainCommand != null)
				{
					//logger.debug("plain text response is " + Conversion.arrayToHex(plainCommand));
				}
			}

			// increment the value of counter icv for CENC
			if (
				SecMode == SecLevel.C_ENC_AND_MAC ||
				SecMode == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC ||
				SecMode == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC
			)
				CENC_Counter++;

			return resp;*/
			return null;
		}
		public ResponseAPDU StoreData(byte[] data, bool lastBlock, byte blockNumber)
		{
			/*
			if (data == null)
				throw new ArgumentException("data must be not null");
			
			var headerSize = (byte)5; // CLA + INS + P1 + P2 + LC
			var dataSize = (byte)data.Length;
			if (SecMode != SecLevel.NO_SECURITY_LEVEL)
				dataSize = (byte)(dataSize + 8); // add space for the C-MAC

			var storeData = new byte[headerSize + (dataSize & 0xFF)];
			storeData[(int)ISO7816.OFFSET_CLA] = (byte) (SecMode == SecLevel.NO_SECURITY_LEVEL ? 0x80 : 0x84);
			storeData[(int)ISO7816.OFFSET_INS] = 0xE2; // (INS) STORE DATA command
			storeData[(int)ISO7816.OFFSET_P1] = lastBlock == true ? (byte) 0x81 : (byte) 0x01; // (P1) Last block?
			storeData[(int)ISO7816.OFFSET_P2] = blockNumber; // (P2) Block Number
			storeData[(int)ISO7816.OFFSET_LC] = dataSize; // (LC) data length
			storeData.Copy(data, 0, data.Length, 5); // put the DATA into storeDataComm

			var i = 5 + data.Length; // next index of storeDataComm to deal with
			if (SecMode == SecLevel.C_MAC)
			{
				var dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
				dataCmac.Copy(storeData, 0, dataCmac.Length); // data used to generate C-MAC
				var cmac = GenerateMac(dataCmac); // generate C-MAC
				storeData.Copy(cmac, 0, cmac.Length, dataCmac.Length); // put C-MAC into storeDataComm
			}

			if (
				(SecMode == SecLevel.C_ENC_AND_MAC) &&
				(Scp != SCPMode.SCP_03_65) &&
				(Scp != SCPMode.SCP_03_6D) &&
				(Scp != SCPMode.SCP_03_05) &&
				(Scp != SCPMode.SCP_03_0D) &&
				(Scp != SCPMode.SCP_03_2D) &&
				(Scp != SCPMode.SCP_03_25)
			)
			{
				var dataCmac = new byte[headerSize + dataSize - 8]; // data used  to generate C-MAC
				dataCmac.Copy(storeData, 0, dataCmac.Length); // data used to generate C-MAC
				var cmac = GenerateMac(dataCmac); // generate C-MAC
				storeData.Copy(cmac, 0, cmac.Length, dataCmac.Length); // put C-MAC into storeDataComm
				storeData = EncryptCommand(storeData);
			}

			if (
				(SecMode == SecLevel.C_ENC_AND_MAC) ||
				(SecMode == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) ||
				(SecMode == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)
			)
			{
				if (
					(Scp == SCPMode.SCP_03_65) ||
					(Scp == SCPMode.SCP_03_6D) ||
					(Scp == SCPMode.SCP_03_05) ||
					(Scp == SCPMode.SCP_03_0D) ||
					(Scp == SCPMode.SCP_03_2D) ||
					(Scp == SCPMode.SCP_03_25)
				)
				{
					var dataToEnc = new byte[headerSize + dataSize - 8]; // data to encrypt without 8 bytes of CMAC
					dataToEnc.Copy(storeData, 0, dataToEnc.Length); // data used to generate C-MAC

					var encStoreDataComm = EncryptCommand(dataToEnc);
					var cmac = GenerateMac(encStoreDataComm); // generate C-MAC

					storeData = new byte[encStoreDataComm.Length + 8];
					storeData.Copy(encStoreDataComm, 0, encStoreDataComm.Length);
					storeData.Copy(cmac, 0, cmac.Length, encStoreDataComm.Length);
				}
			}

			var cmdStoreData = new ApduCommand(storeData);
			var resp = CardChannel.Transmit(cmdStoreData);

			if (resp.Sw != (int)ISO7816.SW_NO_ERROR)
				throw new ApplicationException("Error in STORE DATA : " + resp.Sw.ToHexadecimal());

			if (SecMode == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC) {
				byte[] plainCommand = DecryptCardResponseData(resp.Bytes);
				if (plainCommand != null)
				{
				}
			}

			// increment the value of counter icv for CENC
			if (
				(SecMode == SecLevel.C_ENC_AND_MAC) ||
				(SecMode == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) ||
				(SecMode == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)
			)
				CENC_Counter++;

			return resp;
			*/
			return null;
		}
	}
}

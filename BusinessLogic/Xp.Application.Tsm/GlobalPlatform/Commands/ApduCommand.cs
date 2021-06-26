using System;
using System.Diagnostics;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Tsm.GlobalPlatform.Commands;
using Xp.Common;

namespace Xp.Application.Tsm.GlobalPlatform.Commands
{
	public class ApduCommand
	{
		public byte Cla { get; set; }
		public byte Ins { get; set; }
		public byte P1 { get; set; }
		public byte P2 { get; set; }
		public byte[] Data { get; set; }
		public byte Le { get; set; }

		#region Command
		public byte[] Command
		{
			get
			{
				var command = new byte[Math.Min(Le, Data.Length) + 5];

				command[0] = Cla;
				command[1] = Ins;
				command[2] = P1;
				command[3] = P2;
				command[4] = Le;
				command.Copy(Data, 0, Math.Min(Le, Data.Length), 5);

				return command;
			}
		}
		#endregion Command

		#region Constructors
		public ApduCommand(byte cla, byte ins, byte p1, byte p2, byte[] data, byte? le = null)
		{
			Cla = cla;
			Ins = ins;
			P1 = p1;
			P2 = p2;
			Data = data;
			Le = le ?? (byte)data.Length;
		}
		#endregion Constructors

		#region ToHexadecimal
		public string ToHexadecimal()
		{
			return Command.ToHexadecimal();
		}
		#endregion ToHexadecimal

		#region ToHexadecimalWithMac_ExternalAuthenticate
		public string ToHexadecimalWithMac_ExternalAuthenticate(TsmSession session, ScpMode scp)
		{
			//if (arguments.SecMode == SecLevel.NO_SECURITY_LEVEL)
			//	return Command.ToHexadecimal();

			//var dataWithPadding = (byte[])null;

			//if (data.Length % 8 != 0)
			//{
			//	// We need a PADDING
			//	var nbBytes = 8 - (data.Length % 8);
			//	dataWithPadding = new byte[data.Length + nbBytes];
			//	dataWithPadding.Copy(data);
			//	dataWithPadding.Copy(GP2xCommands.PADDING, 0, nbBytes, data.Length);
			//}
			//else
			//{
			//	dataWithPadding = new byte[data.Length + 8];
			//	dataWithPadding.Copy(data, 0, data.Length);
			//	dataWithPadding.Copy(GP2xCommands.PADDING, 0, GP2xCommands.PADDING.Length, data.Length);
			//}

			//var res = new byte[8];
			//var ivSpec = new IvParameterSpec(Icv);

			if (
				(scp == ScpMode.SCP_UNDEFINED) || // TODO: Undefined SCPMode Here ?
				(scp == ScpMode.SCP_01_05) ||
				(scp == ScpMode.SCP_01_15)
			)
			{
				//var cryptogram = XpSecurity.Cypher_DESede_CBC_NoPadding(session.SessMac, dataWithPadding, Icv);
				////var myCipher = Cipher.GetInstance("DESede/CBC/NoPadding");
				////myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessMac, "DESede"), ivSpec);
				////var cryptogram = myCipher.DoFinal(dataWithPadding);
				//res.Copy(cryptogram, cryptogram.Length - 8, 8);

				//switch (scp)
				//{
				//	case ScpMode.SCP_01_05:
				//		Icv = res; // update ICV with new C-MAC
				//		break;
				//	case ScpMode.SCP_01_15: // update ICV with new ENCRYPTED C-MAC
				//		Icv = XpSecurity.Cypher_DESede_CBC_NoPadding(session.SessMac, res);
				//		//Cipher myCipher2 = Cipher.GetInstance("DESede/ECB/NoPadding");
				//		//myCipher2.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessMac, "DESede"));
				//		//Icv = myCipher2.DoFinal(res);
				//		break;
				//}
				throw new ApplicationException("SCP {0} not allowed.".FormatString(scp));
			}
			else if (
				(scp == ScpMode.SCP_02_15) ||
				(scp == ScpMode.SCP_02_04) ||
				(scp == ScpMode.SCP_02_05) ||
				(scp == ScpMode.SCP_02_14) ||
				(scp == ScpMode.SCP_02_45) ||
				(scp == ScpMode.SCP_02_55)
			)
			{
				//SecretKeySpec desSingleKey = new SecretKeySpec(session.SessMac, 0, 8, "DES");
				//Cipher singleDesCipher;
				//singleDesCipher = Cipher.GetInstance("DES/CBC/NoPadding", "SunJCE");

				//// Calculate the first n - 1 block.
				//var noOfBlocks = dataWithPadding.Length / 8;
				//var ivForNextBlock = Icv;
				//var startIndex = 0;
				//for (var i = 0; i < (noOfBlocks - 1); i++)
				//{
				//	singleDesCipher.Init(Cipher.ENCRYPT_MODE, desSingleKey, ivSpec);
				//	ivForNextBlock = singleDesCipher.DoFinal(dataWithPadding, startIndex, 8);
				//	startIndex += 8;
				//	ivSpec = new IvParameterSpec(ivForNextBlock);
				//}

				//SecretKeySpec desKey = new SecretKeySpec(session.SessMac, "DESede");
				//Cipher myCipher;

				//myCipher = Cipher.GetInstance("DESede/CBC/NoPadding", "SunJCE");
				//var offset = dataWithPadding.Length - 8;

				//// Generate C-MAC. Use 8-LSB
				//// For the last block, you can use TripleDES EDE with ECB mode, now I select the CBC and
				//// use the last block of the previous encryption result as ICV.
				//myCipher.Init(Cipher.ENCRYPT_MODE, desKey, ivSpec);
				//res = myCipher.DoFinal(dataWithPadding, offset, 8);
				//if (
				//	(scp == ScpMode.SCP_02_04) ||
				//	(scp == ScpMode.SCP_02_05) ||
				//	(scp == ScpMode.SCP_02_45)
				//)
				//	Icv = res; // update ICV with new C-MAC //no ICV encryption
				//else if (
				//	(scp == ScpMode.SCP_02_15) ||
				//	(scp == ScpMode.SCP_02_14) ||
				//	(scp == ScpMode.SCP_02_55)
				//)
				//{
				//	// update ICV with new ENCRYPTED C-MAC
				//	ivSpec = new IvParameterSpec(new byte[8]);
				//	singleDesCipher.Init(Cipher.ENCRYPT_MODE, desSingleKey, ivSpec);
				//	Icv = singleDesCipher.DoFinal(res);
				//}
				throw new ApplicationException("SCP {0} not allowed.".FormatString(scp));
			}
			else if (
				(scp == ScpMode.SCP_03_65) ||
				(scp == ScpMode.SCP_03_6D) ||
				(scp == ScpMode.SCP_03_05) ||
				(scp == ScpMode.SCP_03_0D) ||
				(scp == ScpMode.SCP_03_2D) ||
				(scp == ScpMode.SCP_03_25)
			)
			{
				Le += 8;

				var dataToCalulateMac = new byte[Command.Length + 16];
				dataToCalulateMac.Copy(session.MacIcv);
				dataToCalulateMac.Copy(Command, 0, Command.Length, 16);
				var res = XpSecurity.Mac_AES(session.SessMac, dataToCalulateMac, macSizeInBits: 64);

				session.MacIcv = XpSecurity.Mac_AES(session.SessMac, dataToCalulateMac);
				
				return Command.ToHexadecimal() + res.ToHexadecimal();
			}

			return null;
		}
		#endregion ToHexadecimalWithMac_ExternalAuthenticate

		#region ToHexadecimalWithMac
		public string ToHexadecimalWithMac(TsmState state, SecLevel secLevel, ScpMode scp)
		{
			if (secLevel == SecLevel.C_MAC)
			{
			//	byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
			//	System.arraycopy(installForPerso, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
			//	byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
			//	System.arraycopy(cmac, 0, installForPerso, dataCmac.length, cmac.length); // put C-MAC into installForPersoComm
			}

			if (
				(secLevel == SecLevel.C_ENC_AND_MAC) &&
				(scp != ScpMode.SCP_03_65) &&
				(scp != ScpMode.SCP_03_6D) &&
				(scp != ScpMode.SCP_03_05) &&
				(scp != ScpMode.SCP_03_0D) &&
				(scp != ScpMode.SCP_03_2D) &&
				(scp != ScpMode.SCP_03_25))
			{
			//	byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
			//	System.arraycopy(installForPerso, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
			//	byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
			//	System.arraycopy(cmac, 0, installForPerso, dataCmac.length, cmac.length); // put C-MAC into installForPersoComm
			//	installForPerso = this.encryptCommand(installForPerso);
			}

			if (
				(
					(secLevel == SecLevel.C_ENC_AND_MAC) ||
					(secLevel == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) ||
					(secLevel == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)
				) && (
					(scp == ScpMode.SCP_03_65) ||
					(scp == ScpMode.SCP_03_6D) ||
					(scp == ScpMode.SCP_03_05) ||
					(scp == ScpMode.SCP_03_0D) ||
					(scp == ScpMode.SCP_03_2D) ||
					(scp == ScpMode.SCP_03_25)
				)
			)
			{
				var dataToEnc = Command; // data used to generate C-MAC
				var encInstallForPersoComm = GP2xCommands.EncryptCommand(dataToEnc, state.Session, scp);
				var cmac = GP2xCommands.GenerateMac_Command(encInstallForPersoComm, state, secLevel, scp);
				var result = encInstallForPersoComm.ToHexadecimal() + cmac.ToHexadecimal();

				return result;
			}

			return null;
		}
		#endregion ToHexadecimalWithMac

		#region Decrypt
		public static byte[] Decrypt(byte[] response, TsmState state, SecLevel secLevel, ScpMode scp)
		{
			var data = response.CloneArray(response.Length - 10);
			var mac = response.CloneArray(response.Length - data.Length - 2, data.Length);
			var status = response.CloneArray(2, response.Length - 2);

			GP2xCommands.ComputeAndVerifyRMac(data, mac, status, state, secLevel, scp);

			if (secLevel == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)
				data = GP2xCommands.DecryptResponse(data, state.Session, secLevel, scp);

			// increment the value of counter icv for CENC
			if (
				(secLevel == SecLevel.C_ENC_AND_MAC) ||
				(secLevel == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) ||
				(secLevel == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)
			)
				state.Session.CEncCounter++;

			var result = new byte[data.Length + status.Length];
			result.Copy(data);
			result.Copy(status, targetOffset: data.Length);

			return result;
		}
		#endregion Decrypt
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Tsm.GlobalPlatform.Config;
using Xp.Common;

namespace Xp.Application.Tsm.GlobalPlatform.Commands
{
    public class GP2xCommands : AbstractCommands
	{
		//public const byte CLA_COMMAND_ISO = 0x00; // 0000xxxx Clannel
		//public const byte CLA_COMMAND_GP = 0x80; // 0000xxxx Clannel

		//public const byte INS_SELECT = 0xA4;
		//public const byte INS_SELECT_P1_ByName = 0x04;
		//public const byte INS_SELECT_P2_First = 0x00;
		//public const byte INS_SELECT_P2_Next = 0x02;

		//public const byte INS_GET_CHALLENGE = 0x84;
		//public const byte INS_GET_CHALLENGE_P2 = 0x00;

		public const byte INS_GETDATA1 = 0xCA;
		public const byte INS_GETDATA2 = 0xCB;
		public const byte INS_DELETE = 0xE4;
		public const byte INS_INSTALL = 0xE6;
		public const byte INS_GETSTATUS = 0xF2;

		//public const byte INS_INITIALIZE_UPDATE = 0x50;
		//public const byte INS_EXTERNAL_AUTHENTICATE = 0x82;

		/// <summary>
		/// Default PADDING to encrypt data for SCP 02 and SCP 01
		/// </summary>
		public static byte[] PADDING = {
			0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
		};
		/// <summary>
		/// Default PADDING to encrypt data for SCP 03
		/// </summary>
		protected static byte[] SCP03_PADDING = {
			0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
		};
		/// <summary>
		/// SCP 01 constant used in @see{fr.xlim.ssd.opal.library.commands.GP2xCommands.initializeUpdate} Response command
		/// </summary>
		public static byte SCP01 = 0x01;
		/// <summary>
		/// SCP 02 constant used in @see{fr.xlim.ssd.opal.library.commands.GP2xCommands.initializeUpdate} Response command
		/// </summary>
		public static byte SCP02 = 0x02;
		/// <summary>
		/// SCP 03 constant used in @see{fr.xlim.ssd.opal.library.commands.GP2xCommands.initializeUpdate} Response command
		/// </summary>
		public static byte SCP03 = 0x03;
		/// <summary>
		/// SCP 02 constant used to obtain, in @see{fr.xlim.ssd.opal.library.commands.GP2xCommands.generateSessionKeys}, the C-Mac session key
		/// </summary>
		protected static byte[] SCP02_DERIVATION4CMAC = { 0x01, 0x01 };
		/// <summary>
		/// SCP 02 constant used to obtain, in @see{fr.xlim.ssd.opal.library.commands.GP2xCommands.generateSessionKeys}, the R-Mac session key
		/// </summary>
		protected static byte[] SCP02_DERIVATION4RMAC = { 0x01, 0x02 };
		/// <summary>
		/// SCP 02 constant used to obtain, in @see{fr.xlim.ssd.opal.library.commands.GP2xCommands.generateSessionKeys}, the encryption session key
		/// </summary>
		protected static byte[] SCP02_DERIVATION4ENCKEY = { 0x01, 0x82 };
		/// <summary>
		/// SCP 02 constant used to obtain, in @see{fr.xlim.ssd.opal.library.commands.GP2xCommands.generateSessionKeys}, the data encryption session key
		/// </summary>
		protected static byte[] SCP02_DERIVATION4DATAENC = { 0x01, 0x81 };
		/// <summary>
		/// SCP 03 constant used to obtain, in @see{fr.xlim.ssd.opal.library.commands.GP2xCommands.generateSessionKeys}, the C-Mac session key
		/// </summary>
		//protected static byte SCP03_DERIVATION4CMAC = 0x06;
		/// <summary>
		/// SCP 03 constant used to obtain, in @see{fr.xlim.ssd.opal.library.commands.GP2xCommands.generateSessionKeys}, the encryption session key
		/// </summary>
		//protected static byte SCP03_DERIVATION4DATAENC = 0x04;
		/// <summary>
		/// SCP 03 constant used to obtain, in @see{fr.xlim.ssd.opal.library.commands.GP2xCommands.generateSessionKeys}, the R-Mac session key
		/// </summary>
		//protected static byte SCP03_DERIVATION4RMAC = 0x07;
		/// <summary>
		/// SCP 03 constant used to obtain the card cryptogram
		/// </summary>
		//protected static byte SCP03_DERIVATION4CardCryptogram = 0x00;
		/// <summary>
		/// SCP 03 constant used to obtain the host cryptogram
		/// </summary>
		//protected static byte SCP03_DERIVATION4HostCryptogram = 0x01;
		/// <summary>
		/// Static Keys
		/// </summary>
		protected List<ISCKey> Keys = new List<ISCKey>();
		private static byte[] iv_zero = "00 00 00 00 00 00 00 00".FromHexadecimal();
		private static byte[] iv_zero_scp03 = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00".FromHexadecimal();
		/// <summary>
		/// used to calculate Encrypted counter ICV for C-ENC
		/// </summary>
		//protected static int CENC_Counter = 01;
		/// <summary>
		/// used to calculate Encrypted counter ICV for R-ENC
		/// </summary>
		//protected int RENC_counter = 01;
		/// <summary>
		/// Counter ICV padding for R-ENC computing
		/// </summary>
		protected static byte[] SCP03_R_ENC_COUNTER_ICV_PADDING = "80 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00".FromHexadecimal();
		/// <summary>
		/// Counter ICV padding for R-ENC computing
		/// </summary>
		protected static byte[] SCP03_C_ENC_COUNTER_ICV_PADDING = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00".FromHexadecimal();
		/// <summary>
		/// Seed key bytes
		/// </summary>
		protected byte[] sdKeyBytes = "60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F".FromHexadecimal();
		/// <summary>
		/// Default constructor
		/// </summary>
		public GP2xCommands()
		{
			SCGPKey defKey1 = new SCGPKey(0x48, 0x01, KeyType.AES_CBC, sdKeyBytes);
			SCGPKey defKey2 = new SCGPKey(0x48, 0x02, KeyType.AES_CBC, sdKeyBytes);
			SCGPKey defKey3 = new SCGPKey(0x48, 0x03, KeyType.AES_CBC, sdKeyBytes);

			SCGPKey[] keys = { defKey1, defKey2, defKey3 };
			
			SetOffCardKeys(keys);
		}

		//public ISCKey GetKey(byte keySetVersion, byte keyId)
		//{
		//	return Keys
		//		.Where(x => x.Version == keySetVersion && x.Id == keyId)
		//		.FirstOrDefault();
		//}
		public ISCKey SetOffCardKey(ISCKey key)
		{
			var currKey = Keys
				.Where(x =>
					(x.Version == key.Version) &&
					(x.Id == key.Id)
				)
				.FirstOrDefault();
			if (currKey != null)
				return currKey;

			Keys.Add(key);
			return key;
		}
		public void SetOffCardKeys(ISCKey[] keys)
		{
			foreach (ISCKey key in keys)
				SetOffCardKey(key);
		}
		public ISCKey DeleteOffCardKey(byte keySetVersion, byte keyId)
		{
			var keys = Keys
				.Where(x =>
					(x.Version == keySetVersion) &&
					(x.Id == keyId)
				)
				.ToArray();

			foreach (ISCKey currKey in keys)
			{
				Keys.Remove(currKey);
				return currKey;
			}
			return null;
		}
		
		/* (non-Javadoc)
		 * @see fr.xlim.ssd.opal.commands.Commands#getStatus(fr.xlim.ssd.opal.Constant.FileType, fr.xlim.ssd.opal.Constant.GetStatusResponseMode, byte[])
		 * /

		@Override
			public ResponseAPDU[] getStatus(GetStatusFileType fileType, GetStatusResponseMode responseMode, byte[] searchQualifier) throws CardException
		{

			logger.debug("=> Get Status begin");

			logger.debug("+ file type is " + fileType);
			logger.debug("+ response mode is " + responseMode);
			logger.debug("+ Search Qualifier is " + (searchQualifier != null ? Conversion.arrayToHex(searchQualifier) : "null"));
			logger.debug("+ SecLevel is " + this.secMode);

				if (fileType == null) {
				throw new IllegalArgumentException("fileType must be not null");
			}

				if (responseMode == null) {
				throw new IllegalArgumentException("responseMode must be not null");
			}

			// TODO: check searchQualifier size ?
			List<ResponseAPDU> responsesList = new LinkedList<ResponseAPDU>();

				byte[] getStatusCmd = null;
		byte dataSize = (byte)0; // '0xD0' + Key Identifier + '0xD2' + Key Version Number + C-MAC
		byte headerSize = (byte)5; // CLA + INS + P1 + P2 + LC

				if (searchQualifier == null) {
					searchQualifier = new byte[2];
					searchQualifier[0] = (byte) 0x4F;
					searchQualifier[1] = (byte) 0x00;
					logger.debug("* Search Qualifier equals " + Conversion.arrayToHex(searchQualifier));
				}

				if (this.secMode == SecLevel.NO_SECURITY_LEVEL) {
			dataSize = (byte)(searchQualifier.length); // searchQualifier
			getStatusCmd = new byte[headerSize + dataSize];
			getStatusCmd[ISO7816.OFFSET_CLA.getValue()] = (byte)0x80;
		} else {
			dataSize = (byte)(searchQualifier.length + 8); // searchQualifier + C-MAC
			getStatusCmd = new byte[headerSize + dataSize];
			getStatusCmd[ISO7816.OFFSET_CLA.getValue()] = (byte)0x84;
		}

		getStatusCmd [ISO7816.OFFSET_INS.getValue()] = (byte) 0xF2;
		getStatusCmd [ISO7816.OFFSET_P1.getValue()] = fileType.getValue();
		getStatusCmd [ISO7816.OFFSET_P2.getValue()] = responseMode.getValue();
		getStatusCmd [ISO7816.OFFSET_LC.getValue()] = dataSize;

		logger.debug("* Get Status command is " + Conversion.arrayToHex(getStatusCmd));

		System.arraycopy(searchQualifier, 0, getStatusCmd, 5, searchQualifier.length);

				if (this.secMode != SecLevel.NO_SECURITY_LEVEL) {
			byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC

			logger.debug("* Data used to generate Mac value is " + Conversion.arrayToHex(dataCmac));

			System.arraycopy(getStatusCmd, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
			byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
			System.arraycopy(cmac, 0, getStatusCmd, dataCmac.length, cmac.length); // put C-MAC into getStatusCmd

			logger.debug("* Get Status command with CMac is " + Conversion.arrayToHex(getStatusCmd));
		}

				byte[]
		uncipheredgetStatusCmd = getStatusCmd.clone();

				if (this.secMode == SecLevel.C_ENC_AND_MAC) {
			getStatusCmd = this.encryptCommand(getStatusCmd);
			logger.debug("* Encrypt get Status command is " + Conversion.arrayToHex(getStatusCmd));
		}

		CommandAPDU cmdGetstatus = new CommandAPDU(getStatusCmd);
		ResponseAPDU resp = this.getCardChannel().transmit(cmdGetstatus);

		logger.debug("GET STATUS command "
						+ "(-> " + Conversion.arrayToHex(cmdGetstatus.getBytes()) + ") "
						+ "(<- " + Conversion.arrayToHex(resp.getBytes()) + ")");

				responsesList.add(resp);

				while (resp.getSW() == ISO7816.SW_MORE_DATA_AVAILABLE.getValue()) {
					uncipheredgetStatusCmd[ISO7816.OFFSET_P2.getValue()] = (byte) (responseMode.getValue() + (byte) 0x01); // Get next occurrence(s)
					if (this.secMode != SecLevel.NO_SECURITY_LEVEL) {
			byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
			System.arraycopy(uncipheredgetStatusCmd, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
			byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
			System.arraycopy(cmac, 0, uncipheredgetStatusCmd, dataCmac.length, cmac.length); // put C-MAC into getStatusCmd
		}
		getStatusCmd = uncipheredgetStatusCmd;
					if (this.secMode == SecLevel.C_ENC_AND_MAC) {
			getStatusCmd = this.encryptCommand(uncipheredgetStatusCmd);
		}
		cmdGetstatus = new CommandAPDU(getStatusCmd);
		resp = this.getCardChannel().transmit(cmdGetstatus);

		logger.debug("GET STATUS command "
							+ "(-> " + Conversion.arrayToHex(cmdGetstatus.getBytes()) + ") "
							+ "(<- " + Conversion.arrayToHex(resp.getBytes()) + ")");

		// TODO: no check at responses ?
		responsesList.add(resp);
		}

				if (resp.getSW() != ISO7816.SW_NO_ERROR.getValue()) {
					throw new CardException("Error in Get Status : " + Integer.toHexString(resp.getSW()));
				}

				ResponseAPDU[] r = new ResponseAPDU[responsesList.size()];

		logger.debug("=> Get Status end");

				return responsesList.toArray(r);
			}
			 */
		public static byte[] EncryptCommand(byte[] data, TsmSession session, ScpMode scp)
		{
			var datas = (byte[])null;
			var encryptedCmd = (byte[])null;
	
			if (
				(scp == ScpMode.SCP_01_05) ||
				(scp == ScpMode.SCP_01_15)
			)
			{
				//var dataLength = command.Length - 4 - 8; // command without (CLA, INS, P1, P2) AND C-MAC
				//if (dataLength % 8 == 0)
				//{
				//	// don't need a PADDING
				//	datas = new byte[dataLength];
				//	datas.Copy(command, 4, dataLength); // copies LC + DATAFIELD from command
				//	datas[0] = (byte)(datas.Length - 1); // update the "pseudo" LC with the length of the original clear text
				//}
				//else
				//{
				//	// need a PADDING
				//	var nbBytes = 8 - (dataLength % 8); // bytes needed for the PADDING
				//	datas = new byte[dataLength + nbBytes];
				//	datas.Copy(command, 4, dataLength); // copies LC + DATAFIELD from command
				//	datas[0] = (byte)(datas.Length - 1 - nbBytes); // update the "pseudo" LC with the length of the original clear text
				//	datas.Copy(PADDING, 0, nbBytes, dataLength); // add necessary PADDING
				//}

				//var ivSpec = new IvParameterSpec("00 00 00 00 00 00 00 00".FromHexadecimal());
				//var myCipher = Cipher.GetInstance("DESede/CBC/NoPadding");
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessEnc, "DESede"), ivSpec);

				//byte[] res = myCipher.DoFinal(datas);
				//encryptedCmd = new byte[5 + res.Length + 8];
				//encryptedCmd.Copy(command, 0, 5);
				//encryptedCmd.Copy(res, 0, res.Length, 5);
				//encryptedCmd.Copy(command, command.Length - 8, 8, res.Length + 5);
				//encryptedCmd[4] = (byte)(encryptedCmd.Length - 5);
				throw new ApplicationException("SCP {0} not allowed.".FormatString(scp));
			}

			if (
				(scp == ScpMode.SCP_02_04) ||
				(scp == ScpMode.SCP_02_05) ||
				(scp == ScpMode.SCP_02_0A) ||
				(scp == ScpMode.SCP_02_0B) ||
				(scp == ScpMode.SCP_02_14) ||
				(scp == ScpMode.SCP_02_15) ||
				(scp == ScpMode.SCP_02_1A) ||
				(scp == ScpMode.SCP_02_1B) ||
				(scp == ScpMode.SCP_02_45) ||
				(scp == ScpMode.SCP_02_54) ||
				(scp == ScpMode.SCP_02_55)
			)
			{
				//var dataLength = command.Length - 5 - 8; // command without (CLA, INS, P1, P2) AND C-MAC

				//if (dataLength % 8 == 0)
				//{
				//	// don't need a PADDING
				//	datas = new byte[dataLength + 8];
				//	datas.Copy(command, 5, dataLength); // copies LC + DATAFIELD from command
				//	datas.Copy(PADDING, 0, 8, dataLength);
				//	command[4] = (byte)(dataLength + 8);
				//}
				//else
				//{
				//	// need a PADDING
				//	var nbBytes = 8 - (dataLength % 8); // bytes needed for the PADDING
				//	datas = new byte[dataLength + nbBytes];
				//	datas.Copy(command, 5, dataLength); // copies LC + DATAFIELD from command
				//	datas.Copy(PADDING, 0, nbBytes, dataLength); // add necessary PADDING
				//}

				//var ivSpec = new IvParameterSpec(iv_zero);
				//var myCipher = Cipher.GetInstance("DESede/CBC/NoPadding", "SunJCE");
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessEnc, "DESede"), ivSpec);

				//var res = myCipher.DoFinal(datas);
				//encryptedCmd = new byte[5 + res.Length + 8];
				//encryptedCmd.Copy(command, 0, 5);
				//encryptedCmd.Copy(res, 0, res.Length, 5);
				//encryptedCmd.Copy(command, command.Length - 8, 8, res.Length + 5);
				//encryptedCmd[4] = (byte)(datas.Length + 8);
				throw new ApplicationException("SCP {0} not allowed.".FormatString(scp));
			}

			if (
				(scp == ScpMode.SCP_03_65) ||
				(scp == ScpMode.SCP_03_05) ||
				(scp == ScpMode.SCP_03_25)
			)
			{
				var dataLength = data.Length - 5; // command without (CLA, INS, P1, P2) AND C-MAC

				if (dataLength % 16 == 0)
				{
					// don't need a PADDING
					datas = new byte[dataLength + 16];
					datas.Copy(data, 5, dataLength); // copies LC + DATAFIELD from command
					datas.Copy(SCP03_PADDING, 0, 16, dataLength);
					data[4] = (byte)(dataLength + 8);
				}
				else
				{
					// need a PADDING
					int nbBytes = 16 - (dataLength % 16); // bytes needed for the PADDING
					datas = new byte[dataLength + nbBytes];
					datas.Copy(data, 5, dataLength); // copies LC + DATAFIELD from command
					datas.Copy(SCP03_PADDING, 0, nbBytes, dataLength); // add necessary PADDING
				}

				var res = XpSecurity.Cypher_AES_CBC_NoPadding(session.SessEnc, datas, iv_zero_scp03);

				encryptedCmd = new byte[5 + res.Length];
				encryptedCmd.Copy(data, 0, 5);
				encryptedCmd.Copy(res, 0, res.Length, 5);
				encryptedCmd[4] = (byte)(datas.Length + 8);
			}

			if (
				(scp == ScpMode.SCP_03_6D) ||
				(scp == ScpMode.SCP_03_0D) ||
				(scp == ScpMode.SCP_03_2D)
			)
			{
				// compute the counter icv
				var hexaCounter = session.CEncCounter.ToHexadecimal();
				if ((hexaCounter.Length % 2) == 1)
					hexaCounter = "0" + hexaCounter;
				var byteCounter = hexaCounter.FromHexadecimal();

				var icvCEnc = iv_zero_scp03.CloneArray();
				icvCEnc.Copy(byteCounter, targetOffset: 16 - byteCounter.Length);
				icvCEnc = XpSecurity.Cypher_AES_CBC_NoPadding(session.SessEnc, icvCEnc, SCP03_C_ENC_COUNTER_ICV_PADDING);

				var dataLength = data.Length - 5; // command without (CLA, INS, P1, P2, LC) AND C-MAC
				if (dataLength % 16 == 0)
				{
					// don't need a PADDING
					datas = new byte[dataLength + 16];
					datas.Copy(data, 5, dataLength); // copies LC + DATAFIELD from command
					datas.Copy(SCP03_PADDING, 0, 16, dataLength);
					data[4] = (byte)(dataLength + 8);
				}
				else
				{
					// need a PADDING
					var nbBytes = 16 - (dataLength % 16); // bytes needed for the PADDING
					datas = new byte[dataLength + nbBytes];
					datas.Copy(data, 5, dataLength); // copies LC + DATAFIELD from command
					datas.Copy(SCP03_PADDING, 0, nbBytes, dataLength); // add necessary PADDING
				}

				var res = XpSecurity.Cypher_AES_CBC_NoPadding(session.SessEnc, datas, icvCEnc);

				encryptedCmd = new byte[5 + res.Length];
				encryptedCmd.Copy(data, 0, 5);
				encryptedCmd.Copy(res, 0, res.Length, 5);
				encryptedCmd[4] = (byte)((res.Length + 8) & 0xFF);
			}

			return encryptedCmd;
		}
		/// <summary>
		/// ICV Initialization. All values set to 0
		/// </summary>
		/* *
		 * Initialization ICV to mac ovec AID of selected application, this function is used in
		 * SCP 02_1A and SCP 02_0A and SCP 02_1B which use the implicit initiation mode
		 * /
		protected void initIcvToMacOverAid(byte[] aid)
		{

			logger.info("==> init ICV to mac over AID");
			logger.info("* SCP 02 Protocol (" + this.scp + ") used");
			logger.info("* IV is " + Conversion.arrayToHex(iv_zero));

			IvParameterSpec ivSpec = new IvParameterSpec(iv_zero);
			byte[] dataWithPadding;

			byte[] res;

			if (aid.length % 8 != 0)
			{ // We need a PADDING

				logger.debug("- Data needs PADDING!");

				int nbBytes = 8 - (aid.length % 8);
				dataWithPadding = new byte[aid.length + nbBytes];
				System.arraycopy(aid, 0, dataWithPadding, 0, aid.length);
				System.arraycopy(GP2xCommands.PADDING, 0, dataWithPadding, aid.length, nbBytes);
			}
			else
			{
				dataWithPadding = new byte[aid.length + 8];
				System.arraycopy(aid, 0, dataWithPadding, 0, aid.length);
				System.arraycopy(GP2xCommands.PADDING, 0, dataWithPadding, aid.length, GP2xCommands.PADDING.length);
			}

			logger.debug("* data with PADDING: " + Conversion.arrayToHex(dataWithPadding));

			try
			{

				SecretKeySpec desSingleKey = new SecretKeySpec(this.sessMac, 0, 8, "DES");
				Cipher singleDesCipher;
				singleDesCipher = Cipher.getInstance("DES/CBC/NoPadding", "SunJCE");

				// Calculate the first n - 1 block.
				int noOfBlocks = dataWithPadding.length / 8;
				byte ivForNextBlock[] = this.icv;
				int startIndex = 0;
				for (int i = 0; i < (noOfBlocks - 1); i++)
				{
					singleDesCipher.init(Cipher.ENCRYPT_MODE, desSingleKey, ivSpec);
					ivForNextBlock = singleDesCipher.doFinal(dataWithPadding, startIndex, 8);
					startIndex += 8;
					ivSpec = new IvParameterSpec(ivForNextBlock);
					logger.debug("* Calculated cryptogram is for Bolck " + i + " " + Conversion.arrayToHex(ivForNextBlock));
				}


				SecretKeySpec desKey = new SecretKeySpec(this.sessMac, "DESede");
				Cipher myCipher;

				myCipher = Cipher.getInstance("DESede/CBC/NoPadding", "SunJCE");
				int offset = dataWithPadding.length - 8;

				// Generate C-MAC. Use 8-LSB
				// For the last block, you can use TripleDES EDE with ECB mode, now I select the CBC and
				// use the last block of the previous encryption result as ICV.
				//ivSpec = new IvParameterSpec(ivForLastBlock);
				myCipher.init(Cipher.ENCRYPT_MODE, desKey, ivSpec);
				res = myCipher.doFinal(dataWithPadding, offset, 8);
				logger.info("New ICV is " + Conversion.arrayToHex(res));
			}
			catch (NoSuchProviderException ex)
			{
				java.util.logging.Logger.getLogger(GP2xCommands.class.getName()).log(Level.SEVERE, null, ex);
				} catch (NoSuchAlgorithmException e) {
					throw new UnsupportedOperationException("Cannot find algorithm", e);
				} catch (NoSuchPaddingException e) {
					throw new UnsupportedOperationException("No such PADDING problem", e);
				} catch (InvalidKeyException e) {
					throw new UnsupportedOperationException("Key problem", e);
				} catch (IllegalBlockSizeException e) {
					throw new UnsupportedOperationException("Block size problem", e);
				} catch (BadPaddingException e) {
					throw new UnsupportedOperationException("Bad PADDING problem", e);
				} catch (InvalidAlgorithmParameterException e) {
					throw new UnsupportedOperationException("Invalid Algorithm parameter", e);
				}
			}
			 */
		//protected byte[] InitializeUpdate2_CalculateDerivationData(ScpMode scp, byte[] hostChallenge, byte[] cardChallenge, byte[] sequenceCounter)
		//{
		//	var derivationData = (byte[])null;

		//	if (
		//		(scp == ScpMode.SCP_UNDEFINED) ||
		//		(scp == ScpMode.SCP_01_05) ||
		//		(scp == ScpMode.SCP_01_15)
		//	)
		//	{
		//		// SCP 01_*
		//		derivationData = new byte[16];
		//		derivationData.Copy(hostChallenge, 0, 4, 4);
		//		derivationData.Copy(hostChallenge, 4, 4, 12);
		//		derivationData.Copy(cardChallenge, 0, 4, 8);
		//		derivationData.Copy(cardChallenge, 4, 4);
		//		return derivationData;
		//	}
		//	else if (
		//		(scp == ScpMode.SCP_02_15) ||
		//		(scp == ScpMode.SCP_02_04) ||
		//		(scp == ScpMode.SCP_02_05) ||
		//		(scp == ScpMode.SCP_02_14) ||
		//		(scp == ScpMode.SCP_02_0A) ||
		//		(scp == ScpMode.SCP_02_45) ||
		//		(scp == ScpMode.SCP_02_55)
		//	)
		//	{
		//		// SCP 02_*
		//		derivationData = new byte[16];
		//		derivationData.Copy(sequenceCounter, 0, 2, 2);
		//		return derivationData;
		//	}
		//	else if (
		//		(scp == ScpMode.SCP_03_65) ||
		//		(scp == ScpMode.SCP_03_6D) ||
		//		(scp == ScpMode.SCP_03_05) ||
		//		(scp == ScpMode.SCP_03_0D) ||
		//		(scp == ScpMode.SCP_03_2D) ||
		//		(scp == ScpMode.SCP_03_25)
		//	)
		//	{
		//		/*
		//		 * Derivation data in SCP 03 mode
		//		 *
		//		 * -0-----------------------10--11---12--13--14--15--16------------23-24------------31-
		//		 * | label (11 byte of 00)    | dc | 00 |  L   | i  | Host Challenge | Card Challenge |
		//		 * ------------------------------------------------------------------------------------
		//		 * Definition of the derivation constant (dc):
		//		 * - 00 : derivation data to calculate card cryptogram
		//		 * - 01 : derivation data to calculate host cryptogram
		//		 * - 04 : derivation of S-ENC
		//		 * - 06 : derivation of S-MAC
		//		 * - 07 : derivation of S-RMAC
		//		 */

		//		derivationData = new byte[32];
		//		var label = new byte[11];

		//		derivationData.Copy(label);
		//		derivationData.Copy(hostChallenge, 0, hostChallenge.Length, 16);
		//		derivationData.Copy(cardChallenge, 0, hostChallenge.Length, 24);
		//	}
		//	return derivationData;
		//}
		//protected TsmSession InitializeUpdate2_GenerateSessionKeys(TsmPersonalizeArguments arguments, InitializeUpdate2Arguments parameters, byte[] derivationData)
		//{
		//	var keyId = arguments.State.KeyId;
		//	if (keyId == 0)
		//		keyId = 1;

		//	var key = GetKey(parameters.KeyVersNumRec, keyId);
		//	if (key == null)
		//		throw new ApplicationException("Selected key not found in local repository (keySetVersion: " + (parameters.KeyVersNumRec & 0xff) + ", keyId: " + keyId + ")");

		//	if (
		//		(parameters.Scp == ScpMode.SCP_UNDEFINED) ||
		//		(parameters.Scp == ScpMode.SCP_01_05) ||
		//		(parameters.Scp == ScpMode.SCP_01_15)
		//	)
		//	{
		//		//if (key is ISCDerivableKey)
		//		//{
		//		//	var keysFromDerivableKey = ((ISCDerivableKey)key).DeriveKey(parameters.KeyDivData);
		//		//	result.KEnc = keysFromDerivableKey[0];
		//		//	result.KMac = keysFromDerivableKey[1];
		//		//	result.KKek = keysFromDerivableKey[2];
		//		//}
		//		//else
		//		//{
		//		//	result.KEnc = (SCGPKey)key;
		//		//	result.KMac = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
		//		//	if (result.KMac == null)
		//		//		throw new ApplicationException("Selected MAC Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
		//		//	result.KKek = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
		//		//	if (result.KKek == null)
		//		//		throw new ApplicationException("Selected KEK Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
		//		//}
		//		//// TODO: SCPMode.SCP_UNDEFINED Here ?

		//		//var sessEnc = new byte[24];
		//		//var sessMac = new byte[24];
		//		//var sessKek = new byte[24];

		//		//var myCipher = Cipher.GetInstance("DESede/ECB/NoPadding");

		//		///* Calculating session encryption key */
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKenc.Value, "DESede"));
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessEnc.Copy(session, 0, 16);
		//		//sessEnc.Copy(session, 0, 8, 16);

		//		///* Calculating session mac key */
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKmac.Value, "DESede"));
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessMac.Copy(session, 0, 16);
		//		//sessMac.Copy(session, 0, 8, 16);

		//		///* Calculating session data encryption key */
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKkek.Value, "DESede"));
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessMac.Copy(session, 0, 16);
		//		//sessMac.Copy(session, 0, 8, 16);

		//		//return new TsmSession
		//		//{
		//		//	SessEnc = sessEnc,
		//		//	SessMac = sessMac,
		//		//	SessKek = sessKek
		//		//};
		//		throw new ApplicationException("SCP {0} not allowed.".FormatString(parameters.Scp));
		//	}
		//	else if (
		//		parameters.Scp == ScpMode.SCP_02_15 ||
		//		parameters.Scp == ScpMode.SCP_02_45 ||
		//		parameters.Scp == ScpMode.SCP_02_05 ||
		//		parameters.Scp == ScpMode.SCP_02_55
		//	)
		//	{
		//		//if (key is ISCDerivableKey)
		//		//{
		//		//	SCGPKey[] keysFromDerivableKey = ((ISCDerivableKey)key).DeriveKey(parameters.KeyDivData);
		//		//	result.KEnc = keysFromDerivableKey[0];
		//		//	result.KMac = keysFromDerivableKey[1];
		//		//	result.KKek = keysFromDerivableKey[2];
		//		//}
		//		//else
		//		//{
		//		//	result.KEnc = (SCGPKey)key;
		//		//	result.KMac = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
		//		//	if (result.KMac == null)
		//		//		throw new ApplicationException("Selected MAC Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
		//		//	result.KKek = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
		//		//	if (result.KKek == null)
		//		//		throw new ApplicationException("Selected KEK Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
		//		//}

		//		//var sessEnc = new byte[24];
		//		//var sessMac = new byte[24];
		//		//var sessRMac = new byte[24];
		//		//var sessKek = new byte[24];

		//		//if (staticKenc.Value.Length == 16)
		//		//{
		//		//	var temp = (byte[])staticKenc.Value.CloneArray();
		//		//	var newStaticKenc = new byte[24];
		//		//	newStaticKenc.Copy(temp, 0, temp.Length);
		//		//	newStaticKenc.Copy(temp, 0, 8, 16);
		//		//	staticKenc = new SCGPKey(staticKenc.Version, staticKenc.Id, KeyType.DES_CBC, newStaticKenc);
		//		//}
		//		//if (staticKmac.Value.Length == 16)
		//		//{
		//		//	var temp = (byte[])staticKmac.Value.CloneArray();
		//		//	var newStaticKmac = new byte[24];
		//		//	newStaticKmac.Copy(temp, 0, temp.Length);
		//		//	newStaticKmac.Copy(temp, 0, 8, 16);
		//		//	staticKmac = new SCGPKey(staticKmac.Version, staticKmac.Id, KeyType.DES_CBC, newStaticKmac);
		//		//}
		//		//if (staticKkek.Value.Length == 16)
		//		//{
		//		//	var temp = (byte[])staticKkek.Value.CloneArray();
		//		//	var newStaticKkek = new byte[24];
		//		//	newStaticKkek.Copy(temp, 0, temp.Length);
		//		//	newStaticKkek.Copy(temp, 0, 8, 16);
		//		//	staticKkek = new SCGPKey(staticKkek.Version, staticKkek.Id, KeyType.DES_CBC, newStaticKkek);
		//		//}

		//		//var myCipher = Cipher.GetInstance("DESede/CBC/NoPadding");
		//		//var ivSpec = new IvParameterSpec(Icv);

		//		//// Calculing Encryption Session Keys
		//		//derivationData.Copy(SCP02_DERIVATION4ENCKEY, 0, 2);
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKenc.Value, "DESede"), ivSpec);
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessEnc.Copy(session, 0, 16);
		//		//sessEnc.Copy(session, 0, 8, 16);

		//		//// Calculing C_Mac Session Keys
		//		//derivationData.Copy(SCP02_DERIVATION4CMAC, 0, 2);
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKmac.Value, "DESede"), ivSpec);
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessMac.Copy(session, 0, 16);
		//		//sessMac.Copy(session, 0, 8, 16);

		//		//// Calculing R_Mac Session Keys
		//		//derivationData.Copy(SCP02_DERIVATION4RMAC, 0, 2);
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKmac.Value, "DESede"), ivSpec);
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessRMac.Copy(session, 0, 16);
		//		//sessRMac.Copy(session, 0, 8, 16);

		//		//// Calculing Data Encryption Session Keys
		//		//derivationData.Copy(SCP02_DERIVATION4DATAENC, 0, 2);
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKkek.Value, "DESede"), ivSpec);
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessKek.Copy(session, 0, 16);
		//		//sessKek.Copy(session, 0, 8, 16);

		//		//return new TsmSession
		//		//{
		//		//	SessEnc = sessEnc,
		//		//	SessMac = sessMac,
		//		//	SessRMac = sessRMac,
		//		//	SessKek = sessKek
		//		//};
		//		throw new ApplicationException("SCP {0} not allowed.".FormatString(parameters.Scp));
		//	}
		//	else if (
		//		(parameters.Scp == ScpMode.SCP_02_04) ||
		//		(parameters.Scp == ScpMode.SCP_02_14)
		//	)
		//	{
		//		//if (key is ISCDerivableKey)
		//		//{
		//		//	SCGPKey[] keysFromDerivableKey = ((ISCDerivableKey)key).DeriveKey(parameters.KeyDivData);
		//		//	result.KEnc = keysFromDerivableKey[0];
		//		//	result.KMac = keysFromDerivableKey[0];
		//		//	result.KKek = keysFromDerivableKey[0];
		//		//}
		//		//else
		//		//{
		//		//	result.KEnc = (SCGPKey)key;
		//		//	result.KMac = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
		//		//	if (result.KMac == null)
		//		//		throw new ApplicationException("Selected MAC Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
		//		//	result.KKek = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
		//		//	if (result.KKek == null)
		//		//		throw new ApplicationException("Selected KEK Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
		//		//}

		//		//var sessEnc = new byte[24];
		//		//var sessMac = new byte[24];
		//		//var sessRMac = new byte[24];
		//		//var sessKek = new byte[24];

		//		//if (staticKenc.Value.Length == 16)
		//		//{
		//		//	var temp = (byte[])staticKenc.Value.CloneArray();
		//		//	var newStaticKenc = new byte[24];
		//		//	newStaticKenc.Copy(temp, 0, temp.Length);
		//		//	newStaticKenc.Copy(temp, 0, 8, 16);
		//		//	staticKenc = new SCGPKey(staticKenc.Version, staticKenc.Id, KeyType.DES_CBC, newStaticKenc);
		//		//}
		//		//if (staticKmac.Value.Length == 16)
		//		//{
		//		//	var temp = (byte[])staticKmac.Value.CloneArray();
		//		//	var newStaticKmac = new byte[24];
		//		//	newStaticKmac.Copy(temp, 0, temp.Length);
		//		//	newStaticKmac.Copy(temp, 0, 8, 16);
		//		//	staticKmac = new SCGPKey(staticKmac.Version, staticKmac.Id, KeyType.DES_CBC, newStaticKmac);
		//		//}
		//		//if (staticKkek.Value.Length == 16)
		//		//{
		//		//	var temp = (byte[])staticKkek.Value.CloneArray();
		//		//	var newStaticKkek = new byte[24];
		//		//	newStaticKkek.Copy(temp, 0, temp.Length);
		//		//	newStaticKkek.Copy(temp, 0, 8, 16);
		//		//	staticKkek = new SCGPKey(staticKkek.Version, staticKkek.Id, KeyType.DES_CBC, newStaticKkek);
		//		//}

		//		//var myCipher = Cipher.GetInstance("DESede/CBC/NoPadding");
		//		//var ivSpec = new IvParameterSpec(Icv);

		//		//// Calculing Encryption Session Keys
		//		//derivationData.Copy(SCP02_DERIVATION4ENCKEY, 0, 2);
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKenc.Value, "DESede"), ivSpec);
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessEnc.Copy(session, 0, 16);
		//		//sessEnc.Copy(session, 0, 8, 16);

		//		//// Calculing C_Mac Session Keys
		//		//derivationData.Copy(SCP02_DERIVATION4CMAC, 0, 2);
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKmac.Value, "DESede"), ivSpec);
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessMac.Copy(session, 0, 16);
		//		//sessMac.Copy(session, 0, 8, 16);

		//		//// Calculing R_Mac Session Keys
		//		//derivationData.Copy(SCP02_DERIVATION4RMAC, 0, 2);
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKmac.Value, "DESede"), ivSpec);
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessRMac.Copy(session, 0, 16);
		//		//sessRMac.Copy(session, 0, 8, 16);

		//		//// Calculing Data Encryption Session Keys
		//		//derivationData.Copy(SCP02_DERIVATION4DATAENC, 0, 2);
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKkek.Value, "DESede"), ivSpec);
		//		//session = myCipher.DoFinal(derivationData);
		//		//sessKek.Copy(session, 0, 16);
		//		//sessKek.Copy(session, 0, 8, 16);

		//		//return new TsmSession
		//		//{
		//		//	SessEnc = sessEnc,
		//		//	SessMac = sessMac,
		//		//	SessRMac = sessRMac,
		//		//	SessKek = sessKek
		//		//};
		//		throw new ApplicationException("SCP {0} not allowed.".FormatString(parameters.Scp));
		//	}
		//	else if (
		//		(parameters.Scp == ScpMode.SCP_03_65) ||
		//		(parameters.Scp == ScpMode.SCP_03_6D) ||
		//		(parameters.Scp == ScpMode.SCP_03_05) ||
		//		(parameters.Scp == ScpMode.SCP_03_0D) ||
		//		(parameters.Scp == ScpMode.SCP_03_2D) ||
		//		(parameters.Scp == ScpMode.SCP_03_25)
		//	)
		//	{
		//		//InitIcv(parameters.Scp);

		//		// Get Secure Channel Keys
		//		var kEnc = (SCGPKey)null;
		//		var kMac = (SCGPKey)null;
		//		var kKek = (SCGPKey)null;

		//		if (key is ISCDerivableKey)
		//		{
		//			var keysFromDerivableKey = ((ISCDerivableKey)key).DeriveKey(parameters.KeyDivData);
		//			kEnc = keysFromDerivableKey[0];
		//			kMac = keysFromDerivableKey[0];
		//			kKek = keysFromDerivableKey[0];
		//		}
		//		else
		//		{
		//			kEnc = (SCGPKey)key;
		//			kMac = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
		//			if (kMac == null)
		//				throw new ApplicationException("Selected MAC Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
		//			kKek = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
		//			if (kKek == null)
		//				throw new ApplicationException("Selected KEK Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
		//		}

		//		// Get Secure Channel Session
		//		var sessEnc = new byte[16];
		//		var sessMac = new byte[16];
		//		var sessRMac = new byte[16];

		//		//var skeySpec = new SecretKeySpec(staticKenc.Value, "AES");
		//		//var cipher = Cipher.GetInstance("AES/CBC/NoPadding");

		//		// Calculing Encryption Session Keys
		//		derivationData[11] = SCP03_DERIVATION4DATAENC;
		//		derivationData[13] = 0x00;
		//		derivationData[14] = 0x80;
		//		derivationData[15] = 0x01;

		//		var res = XpSecurity.Mac_AES(kEnc.Value, derivationData);
		//		//var mac = new CMac();
		//		//sessEnc = mac.Calculate(staticKenc.Value, derivationData);

		//		// Calculing C_Mac Session Keys
		//		derivationData[11] = SCP03_DERIVATION4CMAC;
		//		derivationData[13] = 0x00;
		//		derivationData[14] = 0x80;
		//		derivationData[15] = 0x01;

		//		sessMac = XpSecurity.Mac_AES(kMac.Value, derivationData);
		//		//sessMac = mac.Calculate(staticKmac.Value, derivationData);
		//		//mac.Init(new KeyParameter(staticKmac.Value));
		//		//mac.Update(derivationData, 0, derivationData.Length);
		//		//mac.DoFinal(ref sessMac, 0);

		//		// Calculing R_Mac Session Keys
		//		derivationData[11] = SCP03_DERIVATION4RMAC;
		//		derivationData[13] = 0x00;
		//		derivationData[14] = 0x80;
		//		derivationData[15] = 0x01;

		//		sessRMac = XpSecurity.Mac_AES(kMac.Value, derivationData);
		//		//sessRMac = mac.Calculate(staticKmac.Value, derivationData);
		//		//mac.Init(new KeyParameter(staticKmac.Value));
		//		//mac.Update(derivationData, 0, derivationData.Length);
		//		//mac.DoFinal(ref sessRMac, 0);

		//		return new TsmSession
		//		{
		//			SessEnc = sessEnc,
		//			SessMac = sessMac,
		//			SessRMac = sessRMac
		//		};
		//	}
		//	return null;
		//}
		//protected TsmCryptograms InitializeUpdate2_CalculateCryptograms(ScpMode scp, byte[] sequenceCounter, byte[] hostChallenge, byte[] cardChallenge, byte[] derivationData, TsmSession session)
		//{
		//	var hostCrypto = (byte[])null;
		//	var cardCrypto = (byte[])null;

		//	var data = new byte[24];
		//	//var myCipher = Cipher.GetInstance("DESede/CBC/NoPadding");
		//	//var ivSpec = new IvParameterSpec(Icv);

		//	if (
		//		(scp == ScpMode.SCP_UNDEFINED) ||
		//		(scp == ScpMode.SCP_01_05) ||
		//		(scp == ScpMode.SCP_01_15)
		//	)
		//	{
		//		///* Calculing Cryptogram */
		//		//data.Copy(hostChallenge, 0, 8);
		//		//data.Copy(cardChallenge, 0, 8, 8);
		//		//data.Copy(PADDING, 0, 8, 16);

		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessEnc, "DESede"), ivSpec);
		//		//var cardcryptogram = myCipher.DoFinal(data);
		//		//cardCrypto = new byte[8];
		//		//cardCrypto.Copy(cardcryptogram, 16, 8);

		//		//data.Copy(cardChallenge, 0, 8);
		//		//data.Copy(hostChallenge, 0, 8, 8);
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessEnc, "DESede"), ivSpec);
		//		//var hostcryptogram = myCipher.DoFinal(data);
		//		//hostCrypto = new byte[8];
		//		//hostCrypto.Copy(hostcryptogram, 16, 8);

		//		//return new TsmCryptograms
		//		//{
		//		//	CardCrypto = cardCrypto,
		//		//	HostCrypto = hostCrypto
		//		//};
		//		throw new ApplicationException("SCP {0} not allowed.".FormatString(scp));
		//	}
		//	else if (
		//		(scp == ScpMode.SCP_02_15) ||
		//		(scp == ScpMode.SCP_02_04) ||
		//		(scp == ScpMode.SCP_02_05) ||
		//		(scp == ScpMode.SCP_02_14) ||
		//		(scp == ScpMode.SCP_02_45) ||
		//		(scp == ScpMode.SCP_02_55)
		//	)
		//	{
		//		///* Calculing Card Cryptogram */
		//		//data.Copy(hostChallenge, 0, 8);
		//		//data.Copy(sequenceCounter, 0, 2, 8);
		//		//data.Copy(cardChallenge, 0, 6, 10);
		//		//data.Copy(PADDING, 0, PADDING.Length, 16);

		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessEnc, "DESede"), ivSpec);
		//		//var cardcryptogram = myCipher.DoFinal(data);
		//		//cardCrypto = new byte[8];
		//		//cardCrypto.Copy(cardcryptogram, 16, 8);

		//		///* Calculing Host Cryptogram */
		//		//data.Copy(sequenceCounter, 0, 2);
		//		//data.Copy(cardChallenge, 0, 6, 2);
		//		//data.Copy(hostChallenge, 0, 8, 8);
		//		//data.Copy(PADDING, 0, 16, PADDING.Length);
		//		//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessEnc, "DESede"), ivSpec);
		//		//var hostcryptogram = myCipher.DoFinal(data);
		//		//hostCrypto = new byte[8];
		//		//hostCrypto.Copy(hostcryptogram, 16, 8);

		//		//return new TsmCryptograms
		//		//{
		//		//	CardCrypto = cardCrypto,
		//		//	HostCrypto = hostCrypto
		//		//};
		//		throw new ApplicationException("SCP {0} not allowed.".FormatString(scp));
		//	}
		//	else if (
		//		(scp == ScpMode.SCP_03_65) ||
		//		(scp == ScpMode.SCP_03_6D) ||
		//		(scp == ScpMode.SCP_03_05) ||
		//		(scp == ScpMode.SCP_03_0D) ||
		//		(scp == ScpMode.SCP_03_2D) ||
		//		(scp == ScpMode.SCP_03_25)
		//	)
		//	{
		//		/* Calculing Card Cryptogram */
		//		derivationData[11] = SCP03_DERIVATION4CardCryptogram;
		//		derivationData[13] = 0x00;
		//		derivationData[14] = 0x40;
		//		derivationData[15] = 0x01;

		//		//var mac = new CMac(new AESEngine(), 8 * 8);
		//		//var mac = new CMac();

		//		//cardCrypto = mac.Calculate(session.SessMac, derivationData, 64);
		//		cardCrypto = XpSecurity.Mac_AES(session.SessMac, derivationData, null, 64);
		//		//cardCrypto = new byte[8];
		//		//mac.Init(new KeyParameter(session.SessMac));
		//		//mac.Update(derivationData, 0, derivationData.Length);
		//		//mac.DoFinal(ref cardCrypto, 0);

		//		/* Calculing Card Cryptogram */
		//		derivationData[11] = SCP03_DERIVATION4HostCryptogram;
		//		derivationData[13] = 0x00;
		//		derivationData[14] = 0x40;
		//		derivationData[15] = 0x01;

		//		//cardCrypto = mac.Calculate(session.SessMac, derivationData, 64);
		//		cardCrypto = XpSecurity.Mac_AES(session.SessMac, derivationData, null, 64);
		//		//hostCrypto = new byte[8];
		//		//mac.Init(new KeyParameter(session.SessMac));
		//		//mac.Update(derivationData, 0, derivationData.Length);
		//		//mac.DoFinal(ref hostCrypto, 0);

		//		return new TsmCryptograms
		//		{
		//			CardCrypto = cardCrypto,
		//			HostCrypto = hostCrypto
		//		};
		//	}

		//	return null;
		//}
		//protected byte[] PseudoRandomGenerationCardChallenge(byte[] aid, TsmSession session)
		//{
		//	var ivSpec = new IvParameterSpec(iv_zero);
		//	var dataWithPadding = (byte[])null;
		//	if (aid.Length % 8 != 0)
		//	{
		//		// We need a PADDING
		//		int nbBytes = 8 - (aid.Length % 8);
		//		dataWithPadding = new byte[aid.Length + nbBytes];
		//		dataWithPadding.Copy(aid, 0, aid.Length);
		//		dataWithPadding.Copy(PADDING, 0, nbBytes, aid.Length);
		//	}
		//	else
		//	{
		//		dataWithPadding = new byte[aid.Length + 8];
		//		dataWithPadding.Copy(aid, 0, aid.Length);
		//		dataWithPadding.Copy(PADDING, 0, PADDING.Length, aid.Length);
		//	}

		//	var desSingleKey = new SecretKeySpec(session.SessMac, 0, 8, "DES");
		//	var singleDesCipher = Cipher.GetInstance("DES/CBC/NoPadding", "SunJCE");

		//	// Calculate the first n - 1 block.
		//	var noOfBlocks = dataWithPadding.Length / 8;
		//	var ivForNextBlock = Icv;
		//	int startIndex = 0;
		//	for (int i = 0; i<(noOfBlocks - 1); i++)
		//	{
		//		singleDesCipher.Init(Cipher.ENCRYPT_MODE, desSingleKey, ivSpec);
		//		ivForNextBlock = singleDesCipher.DoFinal(dataWithPadding, startIndex, 8);
		//		startIndex += 8;
		//		ivSpec = new IvParameterSpec(ivForNextBlock);
		//	}

		//	var desKey = new SecretKeySpec(session.SessMac, "DESede");
		//	var myCipher = Cipher.GetInstance("DESede/CBC/NoPadding", "SunJCE");
		//	var offset = dataWithPadding.Length - 8;

		//	// Generate C-MAC. Use 8-LSB
		//	// For the last block, you can use TripleDES EDE with ECB mode, now I select the CBC and
		//	// use the last block of the previous encryption result as ICV.
		//	//ivSpec = new IvParameterSpec(ivForLastBlock);
		//	myCipher.Init(Cipher.ENCRYPT_MODE, desKey, ivSpec);
		//	var res = myCipher.DoFinal(dataWithPadding, offset, 8);

		//	// the CardChallenge is the six MSB of the result(MAC Over AID)
		//	var computedCardChallenge = new byte[6];
		//	computedCardChallenge.Copy(res, 0, 6);

		//	return computedCardChallenge;
		//}
		/*
		@Override
		public ResponseAPDU deleteOnCardObj(byte[] aid, boolean cascade) throws CardException
		{

		logger.debug("=> Delete On Card Object begin");

		logger.debug("+ " + (aid != null ? "AID to delete is " + Conversion.arrayToHex(aid) : "There is not AID"));
		logger.debug("+ Cascade mode ? " + cascade);
		logger.debug("+ Security mode is " + this.secMode);

		if (aid == null) {
		throw new IllegalArgumentException("aid must be not null");
		}

		byte dataSize = (byte) (2 + aid.length); // params +  AID

		if (this.getSecMode() != SecLevel.NO_SECURITY_LEVEL) {
		dataSize = (byte)(dataSize + 8); // add space for the C-MAC
		}

		byte headerSize = (byte) 5; // CLA + INS + P1 + P2 + LC

		byte[]
		deleteComm = new byte[headerSize + dataSize];

		deleteComm[ISO7816.OFFSET_CLA.getValue()] = (byte) ((this.getSecMode() == SecLevel.NO_SECURITY_LEVEL) ? 0x80 : 0x84); // (CLA) command class (GlobalPlatform command + secure messaging with GlobalPlatform format)
		deleteComm [ISO7816.OFFSET_INS.getValue()] = (byte) 0xE4; // (INS) DELETE command
		deleteComm [ISO7816.OFFSET_P1.getValue()] = (byte) 0x00; // (P1) the only and the last DELETE command
		deleteComm [ISO7816.OFFSET_P2.getValue()] = (cascade ? (byte) 0x80 : (byte) 0x00); // (P2) 0x00 : only delete the specified object
																				   //      0x80 : delete object and related objects
		deleteComm [ISO7816.OFFSET_LC.getValue()] = dataSize;   // (LC) data length

		deleteComm [ISO7816.OFFSET_CDATA.getValue()] = (byte) 0x4F; // the object being deleted is specified by its AID
		deleteComm [ISO7816.OFFSET_CDATA.getValue() + 1] = (byte) aid.length; // AID length
		System.arraycopy(aid, 0, deleteComm, ISO7816.OFFSET_CDATA.getValue() + 2, aid.length); // put the AID into deleteComm

		logger.debug("* Delete Command is " + Conversion.arrayToHex(deleteComm));


		if (this.getSecMode() == SecLevel.C_MAC) {
		byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
		System.arraycopy(deleteComm, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, deleteComm, dataCmac.length, cmac.length); // put C-MAC into installForLoadComm

		logger.debug("* delete Command which CMac is " + Conversion.arrayToHex(deleteComm));
		}

		if (((this.getSecMode() == SecLevel.C_ENC_AND_MAC) || (this.getSecMode() == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) || (this.getSecMode() == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC))
				&& (this.scp != SCPMode.SCP_03_65)
				&& (this.scp != SCPMode.SCP_03_6D)
				&& (this.scp != SCPMode.SCP_03_05)
				&& (this.scp != SCPMode.SCP_03_0D)
				&& (this.scp != SCPMode.SCP_03_2D)
				&& (this.scp != SCPMode.SCP_03_25)) {
		byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
		System.arraycopy(deleteComm, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, deleteComm, dataCmac.length, cmac.length); // put C-MAC into installForLoadComm

		logger.debug("* delete Command which CMac is " + Conversion.arrayToHex(deleteComm));
		deleteComm = this.encryptCommand(deleteComm);
		logger.debug("* Encrypted delete Command is " + Conversion.arrayToHex(deleteComm));
		}

		if ((this.getSecMode() == SecLevel.C_ENC_AND_MAC) || (this.getSecMode() == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) || (this.getSecMode() == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)) {
		if ((this.scp == SCPMode.SCP_03_65)
			|| (this.scp == SCPMode.SCP_03_6D)
			|| (this.scp == SCPMode.SCP_03_05)
			|| (this.scp == SCPMode.SCP_03_0D)
			|| (this.scp == SCPMode.SCP_03_2D)
			|| (this.scp == SCPMode.SCP_03_25))
		{
		byte[] dataToEnc = new byte[headerSize + dataSize - 8]; // data to encrypt without 8 bytes of CMAC
		System.arraycopy(deleteComm, 0, dataToEnc, 0, dataToEnc.length); // data used to generate C-MAC

		byte[] encDeleteComm = this.encryptCommand(dataToEnc);
		logger.debug("* Encrypted Delete Command is " + Conversion.arrayToHex(encDeleteComm));
		byte[] cmac = this.generateMac(encDeleteComm); // generate C-MAC
		logger.debug("* CMac " + Conversion.arrayToHex(cmac));
		deleteComm = new byte[encDeleteComm.length + 8];
		System.arraycopy(encDeleteComm, 0, deleteComm, 0, encDeleteComm.length);
		System.arraycopy(cmac, 0, deleteComm, encDeleteComm.length, cmac.length);
		logger.debug("Final install for load Command : " + Conversion.arrayToHex(deleteComm));
		}
		}

		CommandAPDU cmdDelete = new CommandAPDU(deleteComm);
		ResponseAPDU resp = this.getCardChannel().transmit(cmdDelete);

		logger.debug("DELETE OBJECT command "
				+ "(-> " + Conversion.arrayToHex(cmdDelete.getBytes()) + ") "
				+ "( < - " + Conversion.arrayToHex(resp.getBytes()) + ")");

		if (resp.getSW() != ISO7816.SW_NO_ERROR.getValue()) {
			throw new CardException("Error in DELETE OBJECT : " + Integer.toHexString(resp.getSW()));
		}


		this.compudeAndVerifyRMac(resp.getBytes());

		if ((this.getSecMode() == secMode.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC) && (this.scp == SCPMode.SCP_03_65)) {
		this.decryptCardResponseData(resp.getBytes());
		}

		// increment the value of counter icv for CENC
		if (this.getSecMode() == secMode.C_ENC_AND_MAC
				|| this.getSecMode() == secMode.C_ENC_AND_C_MAC_AND_R_MAC
				|| this.getSecMode() == secMode.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC) {
		CENC_Counter++;
		}


		logger.debug("=> Delete On Card Object End");

		return resp;
		}

		/* (non-Javadoc)
		* @see fr.xlim.ssd.opal.library.commands.Commands#deleteOnCardKey(byte, byte)
		* /

		@Override
		public ResponseAPDU deleteOnCardKey(byte keySetVersion, byte keyId) throws CardException
		{

		logger.debug("=> Delete On Card Key begin");

		logger.debug("+ Key Set Version to delete is " + Integer.toHexString((int) (keySetVersion) & 0xFF));
		logger.debug("+ Key Id to delete is " + Integer.toHexString((int) (keyId) & 0xFF));
		logger.debug("+ SecLevel is " + this.secMode);

		byte dataSize = (byte) 4; // '0xD0' + Key Identifier + '0xD2' + Key Version Number

		if (this.getSecMode() != SecLevel.NO_SECURITY_LEVEL) {
		dataSize = (byte)(dataSize + 8); // add space for the C-MAC
		}

		byte headerSize = (byte) 5; // CLA + INS + P1 + P2 + LC

		byte[]
		deleteComm = new byte[headerSize + dataSize];

		deleteComm[0] = (byte) ((this.getSecMode() == SecLevel.NO_SECURITY_LEVEL) ? 0x80 : 0x84); // (CLA) command class (GlobalPlatform command + secure messaging with GlobalPlatform format)
		deleteComm [1] = (byte) 0xE4; // (INS) DELETE command
		deleteComm [2] = (byte) 0x00; // (P1) the only and the last DELETE command
		deleteComm [3] = (byte) 0x00; // (P2) 0x00 : only delete the specified key
		deleteComm [4] = dataSize;   // (LC) data length

		deleteComm [5] = (byte) 0xD0; // Key Identifier
		deleteComm [6] = keyId;
		deleteComm [7] = (byte) 0xD2; // Key Version Number
		deleteComm [8] = keySetVersion;

		logger.debug("* Delete Command is " + Conversion.arrayToHex(deleteComm));

		if (this.getSecMode() != SecLevel.NO_SECURITY_LEVEL) {
		byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
		System.arraycopy(deleteComm, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, deleteComm, dataCmac.length, cmac.length); // put C-MAC into deleteComm

		logger.debug("* Delete Command whith CMAC is " + Conversion.arrayToHex(deleteComm));
		}

		if (this.getSecMode() == SecLevel.C_ENC_AND_MAC) {
		deleteComm = this.encryptCommand(deleteComm);

		logger.debug("* Encrypted Delete Command is " + Conversion.arrayToHex(deleteComm));
		}

		CommandAPDU cmdDelete = new CommandAPDU(deleteComm);
		ResponseAPDU resp = this.getCardChannel().transmit(cmdDelete);

		logger.debug("DELETE KEY command "
				+ "(-> " + Conversion.arrayToHex(cmdDelete.getBytes()) + ") "
				+ "(<- " + Conversion.arrayToHex(resp.getBytes()) + ")");

		if (resp.getSW() != 0x9000) {
			throw new CardException("Error in DELETE KEY : " + Integer.toHexString(resp.getSW()));
		}

		logger.debug("=> Delete On Card Key End");

		return resp;
		}

		/* (non-Javadoc)
		* @see fr.xlim.ssd.opal.library.commands.Commands#installForLoad(byte[], byte[], byte[])
		* /

		@Override
		public ResponseAPDU installForLoad(byte[] packageAid, byte[] securityDomainAid, byte[] params) throws CardException
		{

		logger.debug("=> Install for load begin");

		logger.debug("+ " + (packageAid != null ? "Package AID to install is " + Conversion.arrayToHex(packageAid) : "There is not Package AID"));
		logger.debug("+ " + (securityDomainAid != null ? "Security Domain AID is " + Conversion.arrayToHex(securityDomainAid) : "There is not Security Domain AID"));
		logger.debug("+ " + (params != null ? "Parameters is " + Conversion.arrayToHex(params) : "There is not parameter"));
		logger.debug("+ SecLevel is " + this.secMode);

		if (packageAid == null) {
		throw new IllegalArgumentException("packageAid must be not null");
		}
		if (securityDomainAid == null) {
		throw new IllegalArgumentException("securityDomainAid must be not null");
		}

		int paramLength = ((params != null) ? params.length : 0);
		byte[]
		paramLengthEncoded = null;
		if (paramLength < 128) {
		paramLengthEncoded = new byte[1];
		paramLengthEncoded[0] = (byte)paramLength;
		} else if (paramLength <= 255) {
		paramLengthEncoded = new byte[2];
		paramLengthEncoded[0] = (byte)0x81;
		paramLengthEncoded[1] = (byte)paramLength;
		} else {
		throw new IllegalArgumentException("params must size must be <= 255");
		}

		logger.debug("* Parameters Length is " + paramLength + " (0x" + Integer.toHexString(paramLength) + ")");
		logger.debug("* Parameters Length Encoded is " + Conversion.arrayToHex(paramLengthEncoded));

		int secDomLength = securityDomainAid.length;

		byte headerSize = (byte) 5; // CLA + INS + P1 + P2 + LC
		byte dataSize = (byte) (1 + packageAid.length // Length of Load File AID +  Load File AID
				+ 1 + secDomLength // + Length of Security Domain AID + Security Domain AID
				+ 1 // + Length of Load File Data BlockHash (0x00)
				+ paramLengthEncoded.length + paramLength // + Length of Load Parameters field + Load Parameters field
				+ 1);                                          // + Length of Load Token (0x00)

		if (this.getSecMode() != SecLevel.NO_SECURITY_LEVEL) {
		dataSize = (byte)(dataSize + 8); // add space for the C-MAC
		}

		byte[]
		installForLoadComm = new byte[(headerSize + (short)(dataSize & 0xFF))];

		installForLoadComm[ISO7816.OFFSET_CLA.getValue()] = (byte) ((this.getSecMode() == SecLevel.NO_SECURITY_LEVEL) ? 0x80 : 0x84); // (CLA) command class (GlobalPlatform command + secure messaging with GlobalPlatform format)
		installForLoadComm [ISO7816.OFFSET_INS.getValue()] = (byte) 0xE6; // (INS) INSTALL command
		installForLoadComm [ISO7816.OFFSET_P1.getValue()] = (byte) 0x02; // (P1) For load
		installForLoadComm [ISO7816.OFFSET_P2.getValue()] = (byte) 0x00; // (P2) no information provided
		installForLoadComm [ISO7816.OFFSET_LC.getValue()] = dataSize; // (LC) data length

		installForLoadComm [ISO7816.OFFSET_CDATA.getValue()] = (byte) packageAid.length; // AID length

		int i = 6; // next index of installForLoadComm to deal with

		System.arraycopy(packageAid, 0, installForLoadComm, i, packageAid.length); // put the AID into installForLoadComm
		i += packageAid.length;

		installForLoadComm [i] = (byte) secDomLength; // length of security domain
		i++;

		System.arraycopy(securityDomainAid, 0, installForLoadComm, i, secDomLength);
		i += secDomLength;

		installForLoadComm [i] = (byte) 0x00; // length of load file data block
		i++;

		System.arraycopy(paramLengthEncoded, 0, installForLoadComm, i, paramLengthEncoded.length);
		i += paramLengthEncoded.length;

		if (params != null) {
		System.arraycopy(params, 0, installForLoadComm, i, paramLength);
		i += paramLength;
		}

		installForLoadComm [i] = (byte) 0x00; // length of load token

		logger.debug("* Install For Load Command is " + Conversion.arrayToHex(installForLoadComm));

		if (this.getSecMode() == SecLevel.C_MAC) {
		byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
		System.arraycopy(installForLoadComm, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, installForLoadComm, dataCmac.length, cmac.length); // put C-MAC into installForLoadComm

		logger.debug("* Install For Load Command whith CMAC is " + Conversion.arrayToHex(installForLoadComm));
		}

		if ((this.getSecMode() == SecLevel.C_ENC_AND_MAC)
				&& (this.scp != SCPMode.SCP_03_65)
				&& (this.scp != SCPMode.SCP_03_6D)
				&& (this.scp != SCPMode.SCP_03_05)
				&& (this.scp != SCPMode.SCP_03_0D)
				&& (this.scp != SCPMode.SCP_03_2D)
				&& (this.scp != SCPMode.SCP_03_25)) {
		byte[] dataCmac = new byte[headerSize + dataSize - 8]; // data used to generate C-MAC
		System.arraycopy(installForLoadComm, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, installForLoadComm, dataCmac.length, cmac.length); // put C-MAC into installForLoadComm

		logger.debug("* Install For Load Command whith CMAC is " + Conversion.arrayToHex(installForLoadComm));
		installForLoadComm = this.encryptCommand(installForLoadComm);
		logger.debug("* Encrypted Install For Load Command is " + Conversion.arrayToHex(installForLoadComm));
		}

		if (((this.getSecMode() == SecLevel.C_ENC_AND_MAC) || (this.getSecMode() == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) || (this.getSecMode() == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC))) {
		if (this.scp == SCPMode.SCP_03_65
			|| this.scp == SCPMode.SCP_03_6D
			|| this.scp == SCPMode.SCP_03_05
			|| this.scp == SCPMode.SCP_03_0D
			|| this.scp == SCPMode.SCP_03_2D
			|| this.scp == SCPMode.SCP_03_25)
		{
		byte[] dataToEnc = new byte[headerSize + dataSize - 8]; // data to encrypt without 8 bytes of CMAC
		System.arraycopy(installForLoadComm, 0, dataToEnc, 0, dataToEnc.length); // data used to generate C-MAC

		byte[] encInstallForLoadComm = this.encryptCommand(dataToEnc);
		logger.debug("* Encrypted Install For Load Command is " + Conversion.arrayToHex(encInstallForLoadComm));
		byte[] cmac = this.generateMac(encInstallForLoadComm); // generate C-MAC
		logger.debug("* CMac " + Conversion.arrayToHex(cmac));
		installForLoadComm = new byte[encInstallForLoadComm.length + 8];
		System.arraycopy(encInstallForLoadComm, 0, installForLoadComm, 0, encInstallForLoadComm.length);
		System.arraycopy(cmac, 0, installForLoadComm, encInstallForLoadComm.length, cmac.length);
		logger.debug("Final install for load Command : " + Conversion.arrayToHex(installForLoadComm));
		}

		}

		CommandAPDU cmdInstallforload = new CommandAPDU(installForLoadComm);
		ResponseAPDU resp = this.getCardChannel().transmit(cmdInstallforload);

		logger.debug("INSTALL FOR LOAD command "
				+ "(-> " + Conversion.arrayToHex(cmdInstallforload.getBytes()) + ") "
				+ "(<- " + Conversion.arrayToHex(resp.getBytes()) + ")");

		if (resp.getSW() != ISO7816.SW_NO_ERROR.getValue()) {
			throw new CardException("Error in INSTALL FOR LOAD : " + Integer.toHexString(resp.getSW()));
		}


		this.compudeAndVerifyRMac(resp.getBytes());

		if (this.getSecMode() == secMode.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC) {
		byte[] plainCommand = this.decryptCardResponseData(resp.getBytes());
		if (plainCommand != null)
		{
		logger.debug("plain text response is " + Conversion.arrayToHex(plainCommand));
		}

		}

		// increment the value of counter icv for CENC
		if (this.getSecMode() == secMode.C_ENC_AND_MAC
				|| this.getSecMode() == secMode.C_ENC_AND_C_MAC_AND_R_MAC
				|| this.getSecMode() == secMode.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC) {
		CENC_Counter++;
		}

		logger.debug("=> Install For Load Command End");

		return resp;
		}

		/* (non-Javadoc)
		* @see fr.xlim.ssd.opal.library.commands.Commands#load(java.io.File)
		* /

		@Override
		public ResponseAPDU[] load(byte[] capFile) throws CardException
		{
		logger.debug("=> Load Command without maxDataLenght");
		return this.load(capFile, (byte) 0xF0);
		}

		/* (non-Javadoc)
		* @see fr.xlim.ssd.opal.library.commands.Commands#load(java.io.File, byte)
		* /

		@Override
		public ResponseAPDU[] load(byte[] capFile, byte maxDataLength) throws CardException
		{

		logger.debug("=> Load Command Begin");

		logger.debug("+ Cap File size to load is " + capFile.length);
		logger.debug("+ Max Data Length is " + (short) (maxDataLength & 0xFF)
				+ "(0x" + Integer.toHexString((int) (maxDataLength & 0xFF)) + ")");
		logger.debug("+ SecLevel is " + this.secMode);

		List<ResponseAPDU> responses = new LinkedList<ResponseAPDU>();
		int capFileRemainLen = capFile.length;
		ByteBuffer buffer = null;

		buffer = ByteBuffer.wrap(capFile);
		buffer.order(ByteOrder.LITTLE_ENDIAN);

		int cMacLen = 0;                // Size of C-MAC (unused by default)
		int headerLen = 5;
		int dataBlockSize = maxDataLength & 0x0ff;            // Size of a block

		if (this.getSecMode() != SecLevel.NO_SECURITY_LEVEL) {
		cMacLen = 8;                // we need to update cMacLen...
		dataBlockSize -= cMacLen;
		logger.debug("* SecLevel != NO_SECURITY_LEVEL => new dataBlockSize is " + dataBlockSize);
		}

		if ((this.getSecMode() == SecLevel.C_ENC_AND_MAC) && (dataBlockSize >= 0xF0)) { // check valid data length...
		dataBlockSize -= 1;
		logger.debug("* SecLevel != C_ENC_AND_MAC => dataBlockSize >= 0xF0 so I decrease it!");
		}

		byte[]
		ber = null;

		logger.debug("* Cap File Remain Length is " + capFileRemainLen);

		if (capFileRemainLen < 128) {
		ber = new byte[2];
		ber[0] = (byte)0xC4;
		ber[1] = (byte)capFileRemainLen;
		} else if (capFileRemainLen < 256) {
		ber = new byte[3];
		ber[0] = (byte)0xC4;
		ber[1] = (byte)0x81;
		ber[2] = (byte)capFileRemainLen;
		} else if (capFileRemainLen < 65536) {
		ber = new byte[4];
		ber[0] = (byte)0xC4;
		ber[1] = (byte)0x82;
		ber[2] = (byte)(capFileRemainLen / 256);
		ber[3] = (byte)(capFileRemainLen % 256);
		}  else {
		throw new IllegalStateException("capFileRemainLen is >= 65536");
		}

		logger.debug("* ber is " + Conversion.arrayToHex(ber));

		int dataSizeInFirstCommand = dataBlockSize - ber.length;

		// number of subsequent blocks to send
		int nbBlock = 0;

		if (capFileRemainLen <= dataSizeInFirstCommand) {
		nbBlock = 1;
		} else if ((capFileRemainLen - dataSizeInFirstCommand) % dataBlockSize == 0) {
		nbBlock = ((capFileRemainLen - dataSizeInFirstCommand) / dataBlockSize) + 1;
		} else {
		nbBlock = ((capFileRemainLen - dataSizeInFirstCommand) / dataBlockSize) + 2;
		}

		logger.debug("* number of block is " + nbBlock);

		byte[]
		cmd = null;

		for (int i = 0; i < nbBlock; i++) {

		if (i == nbBlock - 1)
		{ // only or last block to be sent
		if (i == 0)
		{ // only block to be sent
			cmd = new byte[headerLen + capFileRemainLen + ber.length + cMacLen];
			cmd[4] = (byte)(capFileRemainLen + ber.length + cMacLen);
			System.arraycopy(ber, 0, cmd, 5, ber.length);
			buffer.get(cmd, 5 + ber.length, capFileRemainLen);
		}
		else
		{ //last block to be sent
			cmd = new byte[headerLen + capFileRemainLen + cMacLen];
			cmd[4] = (byte)(capFileRemainLen + cMacLen);
			buffer.get(cmd, 5, capFileRemainLen);
		}
		}
		else
		{ // 2 blocks at least
		cmd = new byte[headerLen + dataBlockSize + cMacLen];
		cmd[4] = (byte)(dataBlockSize + cMacLen);
		if (i == 0)
		{ // first block
			System.arraycopy(ber, 0, cmd, 5, ber.length);
			buffer.get(cmd, 5 + ber.length, dataSizeInFirstCommand);
			capFileRemainLen -= dataSizeInFirstCommand;
		}
		else
		{ // block i
			buffer.get(cmd, 5, dataBlockSize);
			capFileRemainLen -= dataBlockSize;
		}
		}
		cmd[ISO7816.OFFSET_CLA.getValue()] = (byte)((this.getSecMode() == SecLevel.NO_SECURITY_LEVEL) ? 0x80 : 0x84);
		cmd[ISO7816.OFFSET_INS.getValue()] = (byte)0xE8;
		cmd[ISO7816.OFFSET_P1.getValue()] = (byte)((i == nbBlock - 1) ? 0x80 : 0x00);
		cmd[ISO7816.OFFSET_P2.getValue()] = (byte)i;

		logger.debug("* Load Command is " + Conversion.arrayToHex(cmd));


		if (this.getSecMode() == SecLevel.C_MAC)
		{
		byte[] dataCmac = new byte[cmd.length - 8]; // data used to generate C-MAC
		System.arraycopy(cmd, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, cmd, dataCmac.length, cmac.length); // put C-MAC into installForLoadComm

		logger.debug("* Load Command whith CMAC is " + Conversion.arrayToHex(cmd));
		}

		if ((this.getSecMode() == SecLevel.C_ENC_AND_MAC)
			&& (this.scp != SCPMode.SCP_03_65)
			&& (this.scp != SCPMode.SCP_03_6D)
			&& (this.scp != SCPMode.SCP_03_05)
			&& (this.scp != SCPMode.SCP_03_0D)
			&& (this.scp != SCPMode.SCP_03_2D)
			&& (this.scp != SCPMode.SCP_03_25))
		{
		byte[] dataCmac = new byte[cmd.length - 8]; // data used to generate C-MAC
		System.arraycopy(cmd, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, cmd, dataCmac.length, cmac.length); // put C-MAC into installForLoadComm

		logger.debug("* Load Command whith CMAC is " + Conversion.arrayToHex(cmd));
		cmd = this.encryptCommand(cmd);
		logger.debug("* Encrypted Load Command is " + Conversion.arrayToHex(cmd));
		}

		if (((this.getSecMode() == SecLevel.C_ENC_AND_MAC) || (this.getSecMode() == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) || (this.getSecMode() == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)))
		{
		if (this.scp == SCPMode.SCP_03_65
				|| scp == SCPMode.SCP_03_6D
				|| scp == SCPMode.SCP_03_05
				|| scp == SCPMode.SCP_03_0D
				|| scp == SCPMode.SCP_03_2D
				|| scp == SCPMode.SCP_03_25)
		{
			byte[] dataToEnc = new byte[cmd.length - 8]; // data to encrypt without 8 bytes of CMAC
			System.arraycopy(cmd, 0, dataToEnc, 0, dataToEnc.length); // data used to generate C-MAC

			byte[] encInstallForLoadComm = this.encryptCommand(dataToEnc);
			logger.debug("* Encrypted Load Command is " + Conversion.arrayToHex(encInstallForLoadComm));
			byte[] cmac = this.generateMac(encInstallForLoadComm); // generate C-MAC
			logger.debug("* CMac " + Conversion.arrayToHex(cmac));
			cmd = new byte[encInstallForLoadComm.length + 8];
			System.arraycopy(encInstallForLoadComm, 0, cmd, 0, encInstallForLoadComm.length);
			System.arraycopy(cmac, 0, cmd, encInstallForLoadComm.length, cmac.length);
			logger.debug("Final load Command : " + Conversion.arrayToHex(cmd));
		}
		}

		CommandAPDU cmdLoad = new CommandAPDU(cmd);
		ResponseAPDU resp = null;
		resp = this.getCardChannel().transmit(cmdLoad);

		logger.debug("LOAD command "
			+ "(-> " + Conversion.arrayToHex(cmdLoad.getBytes()) + ") "
			+ "(<- " + Conversion.arrayToHex(resp.getBytes()) + ")");

		responses.add(resp);
		if (resp.getSW() != ISO7816.SW_NO_ERROR.getValue())
		{
		throw new CardException("Error in LOAD : " + Integer.toHexString(resp.getSW()));
		}
		this.compudeAndVerifyRMac(resp.getBytes());
		if ((this.getSecMode() == secMode.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC) && (this.scp == SCPMode.SCP_03_65))
		{
		this.decryptCardResponseData(resp.getBytes());
		}
		if (this.getSecMode() == secMode.C_ENC_AND_MAC
			|| this.getSecMode() == secMode.C_ENC_AND_C_MAC_AND_R_MAC
			|| this.getSecMode() == secMode.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)
		{
		CENC_Counter++;
		}

		}
		ResponseAPDU []
		r = new ResponseAPDU[responses.size()];

		// increment the value of counter icv for CENC


		logger.debug("=> Load Command End");

		return responses.toArray(r);
		}

		/* (non-Javadoc)
		* @see fr.xlim.ssd.opal.library.commands.Commands#installForInstallAndMakeSelectable(byte[], byte[], byte[], byte[], byte[])
		* /

		@Override
		public ResponseAPDU installForInstallAndMakeSelectable(byte[] loadFileAID,
														   byte[] moduleAID, byte[] applicationAID, byte[] privileges, byte[] parameters)

			throws CardException
		{

		logger.debug("=> Install For Install And Make Selectable Begin");

		logger.debug("+ " + (loadFileAID != null ? "Load File AID is " + Conversion.arrayToHex(loadFileAID) : "There is not Load File AID"));
		logger.debug("+ " + (moduleAID != null ? "Module AID is " + Conversion.arrayToHex(moduleAID) : "There is not Module AID"));
		logger.debug("+ " + (applicationAID != null ? "Application AID is " + Conversion.arrayToHex(applicationAID) : "There is not Application AID"));
		logger.debug("+ " + (privileges != null ? "Privileges AID is " + Conversion.arrayToHex(privileges) : "There is not privileges"));
		logger.debug("+ " + (parameters != null ? "Parameters is " + Conversion.arrayToHex(parameters) : "There is not parameters"));

		if (loadFileAID == null) {
		throw new IllegalArgumentException("loadFileAID must be not null");
		}

		if (moduleAID == null) {
		throw new IllegalArgumentException("moduleAID must be not null");
		}

		if (privileges == null) {
		throw new IllegalArgumentException("privileges must be not null");
		}

		byte[] params = parameters;

		if (params == null) {
			params = new byte[2];
			params[0] = (byte) 0xC9;
			params[1] = (byte) 0x00;

		logger.debug("* New parameters are " + Conversion.arrayToHex(params));
		}

		if (applicationAID == null) {
			applicationAID = moduleAID.clone();
			logger.debug("* New application AID is " + Conversion.arrayToHex(applicationAID));
		}

		int paramLength = params.length;

		logger.debug("* Parameters Length is " + paramLength);

		byte[] paramLengthEncoded = null;
		if (params.length< 128) {
			paramLengthEncoded = new byte[1];
			paramLengthEncoded[0] = (byte) paramLength;
		} else {
			paramLengthEncoded = new byte[2];
			paramLengthEncoded[0] = (byte) 0x81;
			paramLengthEncoded[1] = (byte) paramLength;
		}

		logger.debug("* Parameters Length Encoded is " + Conversion.arrayToHex(paramLengthEncoded));

		int cMacLen = this.getSecMode() != SecLevel.NO_SECURITY_LEVEL ? 8 : 0;
		byte headerSize = (byte)5;     // CLA + INS + P1 + P2 + LC
		byte dataSize = (byte)(1 + loadFileAID.length // Length of Executable Load File AID + Executable Load File AID
		+ 1 + moduleAID.length // Length of Executable Module AID + Executable Module AID
		+ 1 + applicationAID.length // Length of Application AID + Application AID
		+ 1 + privileges.length // Length of Privileges + Privileges
		+ paramLengthEncoded.length + paramLength // Length of Install Parameters field + Install Parameters field
		+ 1 // Length of Install Token (0)
		+ cMacLen);                                         // C-MAC Length

		byte[] installForInstallComm = new byte[headerSize + dataSize];

		installForInstallComm[ISO7816.OFFSET_CLA.getValue()] = (byte) ((this.getSecMode() == SecLevel.NO_SECURITY_LEVEL) ? 0x80 : 0x84); // (CLA) command class (GlobalPlatform command + secure messaging with GlobalPlatform format)
		installForInstallComm [ISO7816.OFFSET_INS.getValue()] = (byte) 0xE6;    // (INS) INSTALL command
		installForInstallComm [ISO7816.OFFSET_P1.getValue()] = (byte) 0x0C;     // (P1) For install
		installForInstallComm [ISO7816.OFFSET_P2.getValue()] = (byte) 0x00;    // (P2) no information provided
		installForInstallComm [ISO7816.OFFSET_LC.getValue()] = dataSize;    // (LC) data length

		/* ------------ BEGIN -- Install for install Data Field -------------- * /

		int i = 5; // next index of installForLoadComm to deal with

		// Load file AID
		installForInstallComm [i] = (byte) loadFileAID.length; // AID length
		i++;
		System.arraycopy(loadFileAID, 0, installForInstallComm, i, loadFileAID.length); // put the AID into installForLoadComm
		i += loadFileAID.length;

		// Module AID
		installForInstallComm [i] = (byte) moduleAID.length;
		i++;
		System.arraycopy(moduleAID, 0, installForInstallComm, i, moduleAID.length);
		i += moduleAID.length;

		// Application AID
		installForInstallComm [i] = (byte) applicationAID.length;
		i++;
		System.arraycopy(applicationAID, 0, installForInstallComm, i, applicationAID.length);
		i += applicationAID.length;

		// Privileges
		installForInstallComm [i] = (byte) privileges.length;
		i++;
		System.arraycopy(privileges, 0, installForInstallComm, i, privileges.length);
		i += privileges.length;

		// Install Parameters
		System.arraycopy(paramLengthEncoded, 0, installForInstallComm, i, paramLengthEncoded.length);
		i += paramLengthEncoded.length;
		System.arraycopy(params, 0, installForInstallComm, i, paramLength);
		i += paramLength;

		// Install token length
		installForInstallComm [i] = (byte) 0x00;
		i++;

		/* ------------ END -- Install for install Data Field -------------- * /

		logger.debug("* Install For Install Command is " + Conversion.arrayToHex(installForInstallComm));


		if (this.getSecMode() == SecLevel.C_MAC) {
		byte[] dataCmac = new byte[installForInstallComm.length - 8]; // data used to generate C-MAC
		System.arraycopy(installForInstallComm, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, installForInstallComm, dataCmac.length, cmac.length); // put C-MAC into installForLoadComm

		logger.debug("* Install For Install Command whith mac is " + Conversion.arrayToHex(installForInstallComm));
		}

		if ((this.getSecMode() == SecLevel.C_ENC_AND_MAC)
				&& (this.scp != SCPMode.SCP_03_65)
				&& (this.scp != SCPMode.SCP_03_6D)
				&& (this.scp != SCPMode.SCP_03_05)
				&& (this.scp != SCPMode.SCP_03_0D)
				&& (this.scp != SCPMode.SCP_03_2D)
				&& (this.scp != SCPMode.SCP_03_25)) {
		byte[] dataCmac = new byte[installForInstallComm.length - 8]; // data used to generate C-MAC
		System.arraycopy(installForInstallComm, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, installForInstallComm, dataCmac.length, cmac.length); // put C-MAC into installForLoadComm

		logger.debug("* Install For Install Command whith mac is " + Conversion.arrayToHex(installForInstallComm));
		installForInstallComm = this.encryptCommand(installForInstallComm);
		logger.debug("* Encrypted Install For Install Command is " + Conversion.arrayToHex(installForInstallComm));
		}

		if ((this.getSecMode() == SecLevel.C_ENC_AND_MAC)) {
		if (scp == SCPMode.SCP_03_65
			|| scp == SCPMode.SCP_03_6D
			|| scp == SCPMode.SCP_03_05
			|| scp == SCPMode.SCP_03_0D
			|| scp == SCPMode.SCP_03_2D
			|| scp == SCPMode.SCP_03_25)
		{
		byte[] dataToEnc = new byte[installForInstallComm.length - 8]; // data to encrypt without 8 bytes of CMAC
		System.arraycopy(installForInstallComm, 0, dataToEnc, 0, dataToEnc.length); // data used to generate C-MAC

		byte[] encInstallForLoadComm = this.encryptCommand(dataToEnc);
		byte[] cmac = this.generateMac(encInstallForLoadComm); // generate C-MAC
		installForInstallComm = new byte[encInstallForLoadComm.length + 8];
		System.arraycopy(encInstallForLoadComm, 0, installForInstallComm, 0, encInstallForLoadComm.length);
		System.arraycopy(cmac, 0, installForInstallComm, encInstallForLoadComm.length, cmac.length);
		logger.debug("Final install for Install Command : " + Conversion.arrayToHex(installForInstallComm));
		}
		}

		CommandAPDU cmdInstallForInstall = new CommandAPDU(installForInstallComm);
		ResponseAPDU resp = this.getCardChannel().transmit(cmdInstallForInstall);
		logger.debug("INSTALL FOR INSTALL AND MAKE SELECTABLE "
				+ "(-> " + Conversion.arrayToHex(cmdInstallForInstall.getBytes()) + ") "
				+ "(<- " + Conversion.arrayToHex(resp.getBytes()) + ")");

		if (resp.getSW() != ISO7816.SW_NO_ERROR.getValue()) {
			throw new CardException("Error in INSTALL FOR INSTALL AND MAKE SELECTABLE : " + Integer.toHexString(resp.getSW()));
		}

		this.compudeAndVerifyRMac(resp.getBytes());

		if ((this.getSecMode() == secMode.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC) && (this.scp == SCPMode.SCP_03_65)) {
		this.decryptCardResponseData(resp.getBytes());
		}

		// increment the value of counter icv for CENC
		if (this.getSecMode() == secMode.C_ENC_AND_MAC
				|| this.getSecMode() == secMode.C_ENC_AND_C_MAC_AND_R_MAC
				|| this.getSecMode() == secMode.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC) {
		CENC_Counter++;
		}

		logger.debug("=> Install For Install And Make Selectable End");

		return resp;
		}

		@Override
		public ResponseAPDU getData() throws CardException
		{
		byte headerSize = (byte) 5;
		byte[]
		getDataComm = new byte[headerSize];

		getDataComm[ISO7816.OFFSET_CLA.getValue()] = (byte) 0x00; // (CLA) command class
		getDataComm[ISO7816.OFFSET_INS.getValue()] = (byte) 0xCA; // (INS) SELECT command
		getDataComm[ISO7816.OFFSET_P1.getValue()] = (byte) 0x00; // (P1) SELECT by name
		getDataComm[ISO7816.OFFSET_P2.getValue()] = (byte) 0xC1; // (P2) first or only occurrence
		getDataComm[4] = (byte) 0x00; // (LC) data length
		CommandAPDU cmdGetData = new CommandAPDU(getDataComm);
		ResponseAPDU resp = this.getCardChannel().transmit(cmdGetData);
		logger.debug("Get Data Command "
				+ "(-> " + Conversion.arrayToHex(cmdGetData.getBytes()) + ") "
				+ "(<- " + Conversion.arrayToHex(resp.getBytes()) + ")");
		if (resp.getSW() != ISO7816.SW_NO_ERROR.getValue()) {

			this.resetParams();
			throw new CardException("Invalid response SW after Get Data command (" + Integer.toHexString(resp.getSW()) + ")");
		}
		System.arraycopy(resp.getBytes(), 0, this.sequenceCounter, 0, 2);
		logger.debug("Sequence counter : " + Conversion.arrayToHex(this.sequenceCounter));
		return resp;
		}

		@Override
		public void InitParamForImplicitInitiationMode(byte[] aid, SCPMode desiredScp, byte keyId) throws CardException
		{


		byte keyVersNumRec = (byte) 0xFF;
		//if (desiredScp == SCPMode.SCP_02_0A || desiredScp == SCPMode.SCP_02_0B){
		resetParams();
		if (true) {
		this.sequenceCounter = new byte[2];
		this.scp = desiredScp;
		getData();
		calculateDerivationData();
		if (keyId == (byte)0)
		{
			keyId = (byte)1;
			logger.info("key id switchs from 0 to 1");
		}
		SCKey key = this.getKey(keyVersNumRec, keyId);
		if (key == null)
		{
			this.resetParams();
			throw new CardException("Selected key not found in local repository (keySetVersion: "
					+ (keyVersNumRec & 0xff) + ", keyId: " + keyId + ")");
		}

		SCGPKey kEnc = null;
		SCGPKey kMac = null;
		SCGPKey kKek = null;

		if (scp == SCPMode.SCP_02_15)
		{
			logger.debug("je suis la");
			kEnc = (SCGPKey)key;
			kMac = (SCGPKey)this.getKey(keyVersNumRec, (byte)(++keyId));
			if (kMac == null)
			{
				this.resetParams();
				throw new CardException("Selected MAC Key not found in Local Repository : keySetVersion : " + (keyVersNumRec & 0xff) + ", keyId : " + (keyId));
			}
			kKek = (SCGPKey)this.getKey(keyVersNumRec, (byte)(++keyId));
			if (kKek == null)
			{
				this.resetParams();
				throw new CardException("Selected KEK Key not found in Local Repository : keySetVersion : " + (keyVersNumRec & 0xff) + ", keyId : " + (keyId));
			}

		}
		else if (scp == SCPMode.SCP_02_0A)
		{

			kEnc = (SCGPKey)key;
			kMac = (SCGPKey)this.getKey(keyVersNumRec, (byte)(keyId));
			if (kMac == null)
			{
				this.resetParams();
				throw new CardException("Selected MAC Key not found in Local Repository : keySetVersion : " + (keyVersNumRec & 0xff) + ", keyId : " + (keyId));
			}
			kKek = (SCGPKey)this.getKey(keyVersNumRec, (byte)(keyId));
			if (kKek == null)
			{
				this.resetParams();
				throw new CardException("Selected KEK Key not found in Local Repository : keySetVersion : " + (keyVersNumRec & 0xff) + ", keyId : " + (keyId));
			}

		}
		logger.debug("scp " + this.scp);
		this.generateSessionKeys(kEnc, kMac, kKek);
		initIcvToMacOverAid(aid);


		//initIcvToMacOverAid(aid);
		} else {
		this.resetParams();
		throw new CardException("this SELECT Command is used for card which implement SCPMode (SCP 02_0A or SCP_02_0B) ");
		}
		}


		/* (non-Javadoc)
		* @see fr.xlim.ssd.opal.library.commands.Commands#beginRMacSession();
		*  /
		@Override
		public ResponseAPDU beginRMacSession() throws CardException
		{
		logger.debug("=> Begin R-Mac Session begin");
		if (this.secMode == null) {
		throw new IllegalArgumentException("secLevel must be not null");
		}

		logger.debug("* Sec Mode is" + this.secMode);

		if (this.sessState != SessionState.SESSION_AUTH) {
		this.resetParams();
		throw new CardException("Session has not been authentificate");
		}

		byte[]
		cmd = null;
		if (this.secMode != SecLevel.NO_SECURITY_LEVEL) {
		cmd = new byte[5 + 8];
		} else {
		cmd = new byte[5];
		}
		cmd [ISO7816.OFFSET_CLA.getValue()] = (byte) ((this.getSecMode() == SecLevel.NO_SECURITY_LEVEL) ? 0x80 : 0x84);
		cmd [ISO7816.OFFSET_INS.getValue()] = (byte) 0x7A;
		if ((this.secMode == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) || (this.secMode == SecLevel.C_MAC_AND_R_MAC)) {
		cmd[ISO7816.OFFSET_P1.getValue()] = (byte)0x10;
		} else if ((this.secMode == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)) {
		cmd[ISO7816.OFFSET_P1.getValue()] = (byte)0x30;
		}
		cmd [ISO7816.OFFSET_P2.getValue()] = (byte) 0x01;

		if (this.secMode != SecLevel.NO_SECURITY_LEVEL) {
		cmd[ISO7816.OFFSET_LC.getValue()] = 0x08;
		} else {
		cmd[ISO7816.OFFSET_LC.getValue()] = 0x00;
		}
		if (this.secMode != SecLevel.NO_SECURITY_LEVEL) {
		byte[] dataCmac = new byte[cmd.length - 8]; // data used to generate C-MAC
		System.arraycopy(cmd, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, cmd, dataCmac.length, cmac.length); // put C-MAC into installForLoadComm
		logger.debug("* Begin R-Mac Command whith CMAC is " + Conversion.arrayToHex(cmd));
		}

		CommandAPDU cmdBeginRMac = new CommandAPDU(cmd);
		ResponseAPDU resp = this.getCardChannel().transmit(cmdBeginRMac);

		logger.debug("EXTERNAL AUTHENTICATE command "
				+ "(-> " + Conversion.arrayToHex(cmdBeginRMac.getBytes()) + ") "
				+ "(<- " + Conversion.arrayToHex(resp.getBytes()) + ")");

		if (resp.getSW() != ISO7816.SW_NO_ERROR.getValue()) {

			this.resetParams();
			throw new CardException("Error in External Authenticate : " + Integer.toHexString(resp.getSW()));
		}


		return resp;
		}

		/* (non-Javadoc)
		* @see fr.xlim.ssd.opal.library.commands.Commands#endRMacSession();
		* /
		@Override
		public ResponseAPDU endRMacSession() throws CardException
		{
		logger.debug("=> End R-Mac Session begin");
		if (this.secMode == null) {
		throw new IllegalArgumentException("secLevel must be not null");
		}

		logger.debug("* Sec Mode is" + this.secMode);

		if (this.sessState != SessionState.SESSION_AUTH) {
		this.resetParams();
		throw new CardException("Session has not been authentificate");
		}

		byte[]
		cmd = null;
		if (this.secMode != SecLevel.NO_SECURITY_LEVEL) {
		cmd = new byte[5 + 8];
		} else {
		cmd = new byte[5];
		}
		cmd [ISO7816.OFFSET_CLA.getValue()] = (byte) ((this.getSecMode() == SecLevel.NO_SECURITY_LEVEL) ? 0x80 : 0x84);
		cmd [ISO7816.OFFSET_INS.getValue()] = (byte) 0x78;

		cmd [ISO7816.OFFSET_P1.getValue()] = (byte) 0x00;
		cmd [ISO7816.OFFSET_P2.getValue()] = (byte) 0x03;

		if (this.secMode != SecLevel.NO_SECURITY_LEVEL) {
		cmd[ISO7816.OFFSET_LC.getValue()] = 0x08;
		} else {
		cmd[ISO7816.OFFSET_LC.getValue()] = 0x00;
		}
		if (this.secMode != SecLevel.NO_SECURITY_LEVEL) {
		byte[] dataCmac = new byte[cmd.length - 8]; // data used to generate C-MAC
		System.arraycopy(cmd, 0, dataCmac, 0, dataCmac.length); // data used to generate C-MAC
		byte[] cmac = this.generateMac(dataCmac); // generate C-MAC
		System.arraycopy(cmac, 0, cmd, dataCmac.length, cmac.length); // put C-MAC into installForLoadComm
		logger.debug("* Begin R-Mac Command whith CMAC is " + Conversion.arrayToHex(cmd));
		}

		CommandAPDU cmdBeginRMac = new CommandAPDU(cmd);
		ResponseAPDU resp = this.getCardChannel().transmit(cmdBeginRMac);

		logger.debug("EXTERNAL AUTHENTICATE command "
				+ "(-> " + Conversion.arrayToHex(cmdBeginRMac.getBytes()) + ") "
				+ "(<- " + Conversion.arrayToHex(resp.getBytes()) + ")");

		if (resp.getSW() != ISO7816.SW_NO_ERROR.getValue()) {

			this.resetParams();
			throw new CardException("Error in External Authenticate : " + Integer.toHexString(resp.getSW()));
		}


		return resp;
		}
		*/

		public static void ComputeAndVerifyRMac(byte[] data, byte[] mac, byte[] status, TsmState state, SecLevel secLevel, ScpMode scp)
		{
			Debug.WriteLine("ComputeAndVerifyRMac: icv " + state.Session.MacIcv.ToHexadecimal());

			if (
				(
					(scp == ScpMode.SCP_03_65) ||
					(scp == ScpMode.SCP_03_6D) ||
					(scp == ScpMode.SCP_03_25)
				) && (
					(secLevel == SecLevel.R_MAC) ||
					(secLevel == SecLevel.C_MAC_AND_R_MAC) ||
					(secLevel == SecLevel.C_ENC_AND_C_MAC_AND_R_MAC) ||
					(secLevel == SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC)
				)
			)
			{
				byte[] dataToMac = new byte[data.Length + status.Length];
				dataToMac.Copy(data);
				dataToMac.Copy(status, targetOffset: data.Length);

				Debug.WriteLine("ComputeAndVerifyRMac: dataToMac " + dataToMac.ToHexadecimal());
				var rMac = GenerateMac_Response(dataToMac, state, secLevel, scp);
				Debug.WriteLine("ComputeAndVerifyRMac: generatedMac " + rMac.ToHexadecimal());
				Debug.WriteLine("ComputeAndVerifyRMac: mac " + mac.ToHexadecimal());

				if (!rMac.SequenceEqual(mac))
					throw new ApplicationException("Response APDU error - RMAC Not verification error: ");
			}
		}
		public static byte[] DecryptResponse(byte[] response, TsmSession session, SecLevel secLevel, ScpMode scp)
		{
			if (response.Length > 10) // the response contain data
			{
				//var res = (byte[])null;
				if ((response.Length % 16) != 0)
					throw new ApplicationException("The length of received encrypted data is invalid");

				if (scp == ScpMode.SCP_03_65)
				{
					//if ((response.Length % 16) != 0)
					//	throw new ApplicationException("The length of received encrypted data is invalid");

					//var ivSpec = new IvParameterSpec(iv_zero_scp03);
					//var cipher = Cipher.GetInstance("AES/CBC/NoPadding");
					//cipher.Init(Cipher.DECRYPT_MODE, new SecretKeySpec(session.SessEnc, "AES"), ivSpec);
					//return cipher.DoFinal(response);
				}
				else if (scp == ScpMode.SCP_03_6D)
				{
					var byteCounter = session.REncCounter.ToHexadecimal().FromHexadecimal();

					var icv = iv_zero_scp03.CloneArray();
					icv.Copy(byteCounter, targetOffset: 16 - byteCounter.Length);
					icv = XpSecurity.Cypher_AES_CBC_NoPadding(session.SessEnc, icv, SCP03_R_ENC_COUNTER_ICV_PADDING);

					var resultWithIcv = XpSecurity.Decrypt_AES_CBC_NoPadding(session.SessEnc, response, icv);
					session.REncCounter++;

					return resultWithIcv.CloneArray(resultWithIcv.ToList().FindLastIndex(x => x == 0x80));
				}
				else if (scp == ScpMode.SCP_03_05)
				{
					//// this mode don't support RMAC then the response contain only Crypted data and Statuts words
					//encryptedData.Copy(response, 0, response.Length - 2);
					//if ((response.Length % 16) != 0)
					//	throw new ApplicationException("The length of received encrypted data is invalid");

					//var ivSpec = new IvParameterSpec(iv_zero_scp03);
					//var cipher = Cipher.GetInstance("AES/CBC/NoPadding");
					//cipher.Init(Cipher.DECRYPT_MODE, new SecretKeySpec(session.SessEnc, "AES"), ivSpec);
					//return cipher.DoFinal(response);
				}
				else if (scp == ScpMode.SCP_03_0D)
				{
					//var icvCEnc = new byte[16];
					//var hexaCounter = CENC_Counter.ToHexadecimal();
					//if (hexaCounter.Length % 2 == 1)
					//	hexaCounter = "0" + hexaCounter;
					////hexaCounter = Conversion.arrayToHex(Conversion.hexToArray(hexaCounter));

					//var byteCounter = hexaCounter.FromHexadecimal();
					//icvCEnc.Copy(SCP03_C_ENC_COUNTER_ICV_PADDING, 0, 16 - byteCounter.Length);
					//icvCEnc.Copy(byteCounter, 0, byteCounter.Length, 16 - byteCounter.Length);

					//encryptedData = new byte[response.Length - 2];
					//encryptedData.Copy(response, 0, response.Length - 2);
					//if (encryptedData.Length % 16 != 0)
					//	throw new ApplicationException("The length of received encrypted data is invalid");

					//var ivSpec = new IvParameterSpec(icvCEnc);
					//var cipher = Cipher.GetInstance("AES/CBC/NoPadding");
					//cipher.Init(Cipher.DECRYPT_MODE, new SecretKeySpec(session.SessEnc, "AES"), ivSpec);
					//RENC_counter++;
					//return cipher.DoFinal(encryptedData);
				}

				return null;
			}
			else if (response.Length == 10)   //the response does not contain data
				return null;
			else
				throw new ApplicationException("The length of received encrypted data is invalid");
		}
		public static byte[] GenerateMac_Command(byte[] data, TsmState state, SecLevel secLevel, ScpMode scp)
		{
			return GenerateMac(data, state, secLevel, scp, state.Session.SessMac, true);
		}
		public static byte[] GenerateMac_Response(byte[] data, TsmState state, SecLevel secLevel, ScpMode scp)
		{
			return GenerateMac(data, state, secLevel, scp, state.Session.SessRMac, false);
		}
		protected static byte[] GenerateMac(byte[] data, TsmState state, SecLevel secLevel, ScpMode scp, byte[] key, bool updateIcv = true)
		{
			var dataWithPadding = (byte[])null;
			if (data.Length % 8 != 0)
			{
				// We need a PADDING
				var nbBytes = 8 - (data.Length % 8);
				dataWithPadding = new byte[data.Length + nbBytes];
				dataWithPadding.Copy(data, 0, data.Length);
				dataWithPadding.Copy(GP2xCommands.PADDING, 0, nbBytes, data.Length);
			}
			else
			{
				dataWithPadding = new byte[data.Length + 8];
				dataWithPadding.Copy(data, 0, data.Length);
				dataWithPadding.Copy(GP2xCommands.PADDING, 0, GP2xCommands.PADDING.Length, data.Length);
			}

			var res = new byte[8];

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
				//update ICV
				if (updateIcv)
				{
					var icv = state.Session.MacIcv;

					var dataToCalulateMac = new byte[icv.Length + data.Length];
					dataToCalulateMac.Copy(icv);
					dataToCalulateMac.Copy(data, targetOffset: icv.Length);
					res = XpSecurity.Mac_AES(key, dataToCalulateMac, null, 8 * 8);

					state.Session.MacIcv = XpSecurity.Mac_AES(key, dataToCalulateMac);
					state.Icvs.Add(state.Session.MacIcv);
					Debug.WriteLine("GenerateMac: new icv " + state.Session.MacIcv.ToHexadecimal());
				}
				else if (state.Icvs.Count() > 0)
				{
					var icv = state.Icvs[0];

					var dataToCalulateMac = new byte[icv.Length + data.Length];
					dataToCalulateMac.Copy(icv);
					dataToCalulateMac.Copy(data, targetOffset: icv.Length);
					res = XpSecurity.Mac_AES(key, dataToCalulateMac, null, 8 * 8);

					state.Icvs.RemoveAt(0);
				}
				else
					throw new ApplicationException("Error generando MAC con el ICV");
			}

			return res;
		}
	}
}

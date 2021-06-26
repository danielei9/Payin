using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Dto.Tsm.Arguments;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Tsm.GlobalPlatform.Commands;
using Xp.Application.Tsm.GlobalPlatform.Config;
using Xp.Application.Tsm.GlobalPlatform.Constants;
using Xp.Application.Tsm.GlobalPlatform.Utilities;
using Xp.Common;

namespace Xp.Application.Tsm.GlobalPlatform.Handlers
{
	internal class TsmInitializeUpdateHandler
	{
		private TsmExternalAuthenticateHandler ExternalAuthenticateHandler { get; set; }

		#region Constructors
		public TsmInitializeUpdateHandler(TsmExternalAuthenticateHandler externalAuthenticateHandler)
		{
			ExternalAuthenticateHandler = externalAuthenticateHandler;
		}
		#endregion Constructors

		#region CreateAsync
		public async Task<TsmExecuteArguments> CreateAsync(TsmExecuteArguments arguments, TsmState state, byte keyId)
		{
			return await Task.Run(() =>
			{
				var hostChallenge = RandomGenerator.generateRandom(8);

				var command = new ApduCommand(
					TsmApduCla.COMMAND_GP, // 0x80
					TsmApduIns.INITIALIZE_UPDATE, // 0x50
					TsmApduP1.INITIALIZE_UPDATE, // 0x48,
					keyId,
					hostChallenge
				);

				state.HostChallenge = hostChallenge.ToHexadecimal();
				//state.KeyId = KeyId;
				//state.Scp = SCP;

				arguments.Data.Add(command.ToHexadecimal());
				arguments.State = state.ToJson();
				arguments.NextStep = NextStepEnum.InitializeUpdate;

				return arguments;
			});
		}
		#endregion CreateAsync

		#region ExecuteAsync
		public async Task<TsmState> ExecuteAsync(TsmExecuteArguments arguments, TsmState state, byte keyId, IEnumerable<ISCKey> keys, ScpMode scp)
		{
			return await Task.Run(() =>
			{
				var command = arguments.Data[0];
				arguments.Data.RemoveAt(0);

				var data = command.FromHexadecimal()
					.GP_CheckGeneralErrors()
					.GP_CheckError(0x63, 0x00, "GP INITIALIZE UPDATE ERROR: Authentication of host cryptogram failed")
					.GP_CheckSuccess(false);

				var hostChallenge = state.HostChallenge.FromHexadecimal();

				// Arguments
				var parameters = InitializeUpdate2Arguments.Create(data, scp);
				var derivationData = CalculateDerivationData(parameters.Scp, hostChallenge, parameters.CardChallenge, parameters.SequenceCounter);
				state.Session = GenerateSessionKeys(arguments, state, keys, keyId, scp, parameters, derivationData);
				var cryptograms = CalculateCryptograms(parameters.Scp, parameters.SequenceCounter, hostChallenge, parameters.CardChallenge, derivationData, state.Session);
				state.HostCrypto = cryptograms.HostCrypto;

				if (
					(parameters.Scp == ScpMode.SCP_02_45) ||
					(parameters.Scp == ScpMode.SCP_02_55)
				)
				{
					//byte[] computedCardChallenge = PseudoRandomGenerationCardChallenge(aid, arguments.State.Session);
					//if (!cardChallenge.SequenceEqual(computedCardChallenge))
					//	throw new ApplicationException("Error verifying Card Challenge");
					throw new ApplicationException("SCP {0} not allowed.".FormatString(parameters.Scp));
				}

				if (!parameters.CardCryptoResp.SequenceEqual(cryptograms.CardCrypto))
					throw new ApplicationException("Error verifying Card Cryptogram");

				//state.SecLevel = SEC_LEVEL;

				return state;
			});
		}
		#endregion ExecuteAsync

		#region CalculateDerivationData
		protected byte[] CalculateDerivationData(ScpMode scp, byte[] hostChallenge, byte[] cardChallenge, byte[] sequenceCounter)
		{
			var derivationData = (byte[])null;

			if (
				(scp == ScpMode.SCP_UNDEFINED) ||
				(scp == ScpMode.SCP_01_05) ||
				(scp == ScpMode.SCP_01_15)
			)
			{
				// SCP 01_*
				derivationData = new byte[16];
				derivationData.Copy(hostChallenge, 0, 4, 4);
				derivationData.Copy(hostChallenge, 4, 4, 12);
				derivationData.Copy(cardChallenge, 0, 4, 8);
				derivationData.Copy(cardChallenge, 4, 4);
				return derivationData;
			}
			else if (
				(scp == ScpMode.SCP_02_15) ||
				(scp == ScpMode.SCP_02_04) ||
				(scp == ScpMode.SCP_02_05) ||
				(scp == ScpMode.SCP_02_14) ||
				(scp == ScpMode.SCP_02_0A) ||
				(scp == ScpMode.SCP_02_45) ||
				(scp == ScpMode.SCP_02_55)
			)
			{
				// SCP 02_*
				derivationData = new byte[16];
				derivationData.Copy(sequenceCounter, 0, 2, 2);
				return derivationData;
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
				/*
				 * Derivation data in SCP 03 mode
				 *
				 * -0-----------------------10--11---12--13--14--15--16------------23-24------------31-
				 * | label (11 byte of 00)    | dc | 00 |  L   | i  | Host Challenge | Card Challenge |
				 * ------------------------------------------------------------------------------------
				 * Definition of the derivation constant (dc):
				 * - 00 : derivation data to calculate card cryptogram
				 * - 01 : derivation data to calculate host cryptogram
				 * - 04 : derivation of S-ENC
				 * - 06 : derivation of S-MAC
				 * - 07 : derivation of S-RMAC
				 */

				derivationData = new byte[32];

				var label = new byte[11];
				derivationData.Copy(label); // Label
											//derivationData[11] = ...; // falta el DC
				derivationData.Copy(hostChallenge, 0, hostChallenge.Length, 16); // Host Challenge
				derivationData.Copy(cardChallenge, 0, cardChallenge.Length, 24); // Card Challenge
			}
			return derivationData;
		}
		#endregion CalculateDerivationData

		#region GenerateSessionKeys
		protected TsmSession GenerateSessionKeys(TsmExecuteArguments arguments, TsmState state, IEnumerable<ISCKey> keys, byte keyId, ScpMode scp, InitializeUpdate2Arguments parameters, byte[] derivationData)
		{
			byte[] icv;
			
			if (keyId == 0)
				keyId = 1;

			var key = GetKey(keys, parameters.KeyVersNumRec, keyId);
			if (key == null)
				throw new ApplicationException("Selected key not found in local repository (keySetVersion: " + (parameters.KeyVersNumRec & 0xff) + ", keyId: " + keyId + ")");

			if (
				(parameters.Scp == ScpMode.SCP_UNDEFINED) ||
				(parameters.Scp == ScpMode.SCP_01_05) ||
				(parameters.Scp == ScpMode.SCP_01_15)
			)
			{
				//if (key is ISCDerivableKey)
				//{
				//	var keysFromDerivableKey = ((ISCDerivableKey)key).DeriveKey(parameters.KeyDivData);
				//	result.KEnc = keysFromDerivableKey[0];
				//	result.KMac = keysFromDerivableKey[1];
				//	result.KKek = keysFromDerivableKey[2];
				//}
				//else
				//{
				//	result.KEnc = (SCGPKey)key;
				//	result.KMac = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
				//	if (result.KMac == null)
				//		throw new ApplicationException("Selected MAC Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
				//	result.KKek = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
				//	if (result.KKek == null)
				//		throw new ApplicationException("Selected KEK Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
				//}
				//// TODO: SCPMode.SCP_UNDEFINED Here ?

				//var sessEnc = new byte[24];
				//var sessMac = new byte[24];
				//var sessKek = new byte[24];

				//var myCipher = Cipher.GetInstance("DESede/ECB/NoPadding");

				///* Calculating session encryption key */
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKenc.Value, "DESede"));
				//session = myCipher.DoFinal(derivationData);
				//sessEnc.Copy(session, 0, 16);
				//sessEnc.Copy(session, 0, 8, 16);

				///* Calculating session mac key */
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKmac.Value, "DESede"));
				//session = myCipher.DoFinal(derivationData);
				//sessMac.Copy(session, 0, 16);
				//sessMac.Copy(session, 0, 8, 16);

				///* Calculating session data encryption key */
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKkek.Value, "DESede"));
				//session = myCipher.DoFinal(derivationData);
				//sessMac.Copy(session, 0, 16);
				//sessMac.Copy(session, 0, 8, 16);

				//return new TsmSession
				//{
				//	SessEnc = sessEnc,
				//	SessMac = sessMac,
				//	SessKek = sessKek
				//};
				throw new ApplicationException("SCP {0} not allowed.".FormatString(parameters.Scp));
			}
			else if (
				parameters.Scp == ScpMode.SCP_02_15 ||
				parameters.Scp == ScpMode.SCP_02_45 ||
				parameters.Scp == ScpMode.SCP_02_05 ||
				parameters.Scp == ScpMode.SCP_02_55
			)
			{
				//if (key is ISCDerivableKey)
				//{
				//	SCGPKey[] keysFromDerivableKey = ((ISCDerivableKey)key).DeriveKey(parameters.KeyDivData);
				//	result.KEnc = keysFromDerivableKey[0];
				//	result.KMac = keysFromDerivableKey[1];
				//	result.KKek = keysFromDerivableKey[2];
				//}
				//else
				//{
				//	result.KEnc = (SCGPKey)key;
				//	result.KMac = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
				//	if (result.KMac == null)
				//		throw new ApplicationException("Selected MAC Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
				//	result.KKek = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
				//	if (result.KKek == null)
				//		throw new ApplicationException("Selected KEK Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
				//}

				//var sessEnc = new byte[24];
				//var sessMac = new byte[24];
				//var sessRMac = new byte[24];
				//var sessKek = new byte[24];

				//if (staticKenc.Value.Length == 16)
				//{
				//	var temp = (byte[])staticKenc.Value.CloneArray();
				//	var newStaticKenc = new byte[24];
				//	newStaticKenc.Copy(temp, 0, temp.Length);
				//	newStaticKenc.Copy(temp, 0, 8, 16);
				//	staticKenc = new SCGPKey(staticKenc.Version, staticKenc.Id, KeyType.DES_CBC, newStaticKenc);
				//}
				//if (staticKmac.Value.Length == 16)
				//{
				//	var temp = (byte[])staticKmac.Value.CloneArray();
				//	var newStaticKmac = new byte[24];
				//	newStaticKmac.Copy(temp, 0, temp.Length);
				//	newStaticKmac.Copy(temp, 0, 8, 16);
				//	staticKmac = new SCGPKey(staticKmac.Version, staticKmac.Id, KeyType.DES_CBC, newStaticKmac);
				//}
				//if (staticKkek.Value.Length == 16)
				//{
				//	var temp = (byte[])staticKkek.Value.CloneArray();
				//	var newStaticKkek = new byte[24];
				//	newStaticKkek.Copy(temp, 0, temp.Length);
				//	newStaticKkek.Copy(temp, 0, 8, 16);
				//	staticKkek = new SCGPKey(staticKkek.Version, staticKkek.Id, KeyType.DES_CBC, newStaticKkek);
				//}

				//var myCipher = Cipher.GetInstance("DESede/CBC/NoPadding");
				//var ivSpec = new IvParameterSpec(Icv);

				//// Calculing Encryption Session Keys
				//derivationData.Copy(SCP02_DERIVATION4ENCKEY, 0, 2);
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKenc.Value, "DESede"), ivSpec);
				//session = myCipher.DoFinal(derivationData);
				//sessEnc.Copy(session, 0, 16);
				//sessEnc.Copy(session, 0, 8, 16);

				//// Calculing C_Mac Session Keys
				//derivationData.Copy(SCP02_DERIVATION4CMAC, 0, 2);
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKmac.Value, "DESede"), ivSpec);
				//session = myCipher.DoFinal(derivationData);
				//sessMac.Copy(session, 0, 16);
				//sessMac.Copy(session, 0, 8, 16);

				//// Calculing R_Mac Session Keys
				//derivationData.Copy(SCP02_DERIVATION4RMAC, 0, 2);
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKmac.Value, "DESede"), ivSpec);
				//session = myCipher.DoFinal(derivationData);
				//sessRMac.Copy(session, 0, 16);
				//sessRMac.Copy(session, 0, 8, 16);

				//// Calculing Data Encryption Session Keys
				//derivationData.Copy(SCP02_DERIVATION4DATAENC, 0, 2);
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKkek.Value, "DESede"), ivSpec);
				//session = myCipher.DoFinal(derivationData);
				//sessKek.Copy(session, 0, 16);
				//sessKek.Copy(session, 0, 8, 16);

				//return new TsmSession
				//{
				//	SessEnc = sessEnc,
				//	SessMac = sessMac,
				//	SessRMac = sessRMac,
				//	SessKek = sessKek
				//};
				throw new ApplicationException("SCP {0} not allowed.".FormatString(parameters.Scp));
			}
			else if (
				(parameters.Scp == ScpMode.SCP_02_04) ||
				(parameters.Scp == ScpMode.SCP_02_14)
			)
			{
				//if (key is ISCDerivableKey)
				//{
				//	SCGPKey[] keysFromDerivableKey = ((ISCDerivableKey)key).DeriveKey(parameters.KeyDivData);
				//	result.KEnc = keysFromDerivableKey[0];
				//	result.KMac = keysFromDerivableKey[0];
				//	result.KKek = keysFromDerivableKey[0];
				//}
				//else
				//{
				//	result.KEnc = (SCGPKey)key;
				//	result.KMac = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
				//	if (result.KMac == null)
				//		throw new ApplicationException("Selected MAC Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
				//	result.KKek = (SCGPKey)GetKey(parameters.KeyVersNumRec, ++keyId);
				//	if (result.KKek == null)
				//		throw new ApplicationException("Selected KEK Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
				//}

				//var sessEnc = new byte[24];
				//var sessMac = new byte[24];
				//var sessRMac = new byte[24];
				//var sessKek = new byte[24];

				//if (staticKenc.Value.Length == 16)
				//{
				//	var temp = (byte[])staticKenc.Value.CloneArray();
				//	var newStaticKenc = new byte[24];
				//	newStaticKenc.Copy(temp, 0, temp.Length);
				//	newStaticKenc.Copy(temp, 0, 8, 16);
				//	staticKenc = new SCGPKey(staticKenc.Version, staticKenc.Id, KeyType.DES_CBC, newStaticKenc);
				//}
				//if (staticKmac.Value.Length == 16)
				//{
				//	var temp = (byte[])staticKmac.Value.CloneArray();
				//	var newStaticKmac = new byte[24];
				//	newStaticKmac.Copy(temp, 0, temp.Length);
				//	newStaticKmac.Copy(temp, 0, 8, 16);
				//	staticKmac = new SCGPKey(staticKmac.Version, staticKmac.Id, KeyType.DES_CBC, newStaticKmac);
				//}
				//if (staticKkek.Value.Length == 16)
				//{
				//	var temp = (byte[])staticKkek.Value.CloneArray();
				//	var newStaticKkek = new byte[24];
				//	newStaticKkek.Copy(temp, 0, temp.Length);
				//	newStaticKkek.Copy(temp, 0, 8, 16);
				//	staticKkek = new SCGPKey(staticKkek.Version, staticKkek.Id, KeyType.DES_CBC, newStaticKkek);
				//}

				//var myCipher = Cipher.GetInstance("DESede/CBC/NoPadding");
				//var ivSpec = new IvParameterSpec(Icv);

				//// Calculing Encryption Session Keys
				//derivationData.Copy(SCP02_DERIVATION4ENCKEY, 0, 2);
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKenc.Value, "DESede"), ivSpec);
				//session = myCipher.DoFinal(derivationData);
				//sessEnc.Copy(session, 0, 16);
				//sessEnc.Copy(session, 0, 8, 16);

				//// Calculing C_Mac Session Keys
				//derivationData.Copy(SCP02_DERIVATION4CMAC, 0, 2);
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKmac.Value, "DESede"), ivSpec);
				//session = myCipher.DoFinal(derivationData);
				//sessMac.Copy(session, 0, 16);
				//sessMac.Copy(session, 0, 8, 16);

				//// Calculing R_Mac Session Keys
				//derivationData.Copy(SCP02_DERIVATION4RMAC, 0, 2);
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKmac.Value, "DESede"), ivSpec);
				//session = myCipher.DoFinal(derivationData);
				//sessRMac.Copy(session, 0, 16);
				//sessRMac.Copy(session, 0, 8, 16);

				//// Calculing Data Encryption Session Keys
				//derivationData.Copy(SCP02_DERIVATION4DATAENC, 0, 2);
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(staticKkek.Value, "DESede"), ivSpec);
				//session = myCipher.DoFinal(derivationData);
				//sessKek.Copy(session, 0, 16);
				//sessKek.Copy(session, 0, 8, 16);

				//return new TsmSession
				//{
				//	SessEnc = sessEnc,
				//	SessMac = sessMac,
				//	SessRMac = sessRMac,
				//	SessKek = sessKek
				//};
				throw new ApplicationException("SCP {0} not allowed.".FormatString(parameters.Scp));
			}
			else if (
				(parameters.Scp == ScpMode.SCP_03_65) ||
				(parameters.Scp == ScpMode.SCP_03_6D) ||
				(parameters.Scp == ScpMode.SCP_03_05) ||
				(parameters.Scp == ScpMode.SCP_03_0D) ||
				(parameters.Scp == ScpMode.SCP_03_2D) ||
				(parameters.Scp == ScpMode.SCP_03_25)
			)
			{
				icv = GetInitIcv(scp);

				// Get Secure Channel Keys
				var kEnc = (SCGPKey)null;
				var kMac = (SCGPKey)null;
				var kKek = (SCGPKey)null;

				if (key is ISCDerivableKey)
				{
					var keysFromDerivableKey = ((ISCDerivableKey)key).DeriveKey(parameters.KeyDivData);
					kEnc = keysFromDerivableKey[0];
					kMac = keysFromDerivableKey[0];
					kKek = keysFromDerivableKey[0];
				}
				else
				{
					kEnc = (SCGPKey)key;
					kMac = (SCGPKey)GetKey(keys, parameters.KeyVersNumRec, ++keyId);
					if (kMac == null)
						throw new ApplicationException("Selected MAC Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
					kKek = (SCGPKey)GetKey(keys, parameters.KeyVersNumRec, ++keyId);
					if (kKek == null)
						throw new ApplicationException("Selected KEK Key not found in Local Repository : keySetVersion : " + (parameters.KeyVersNumRec & 0xff) + ", keyId : " + (keyId));
				}

				// Calculing Encryption Session Keys
				derivationData[11] = TsmScpEnum.SCP03_DERIVATION4DATAENC; // 4
				derivationData[13] = 0x00;
				derivationData[14] = 0x80;
				derivationData[15] = 0x01;
				var sEnc = XpSecurity.Mac_AES(kEnc.Value, derivationData);

				// Calculing C_Mac Session Keys
				derivationData[11] = TsmScpEnum.SCP03_DERIVATION4CMAC; // 6
				derivationData[13] = 0x00;
				derivationData[14] = 0x80;
				derivationData[15] = 0x01;
				var sMac = XpSecurity.Mac_AES(kMac.Value, derivationData);

				// Calculing R_Mac Session Keys
				derivationData[11] = TsmScpEnum.SCP03_DERIVATION4RMAC; // 7
				derivationData[13] = 0x00;
				derivationData[14] = 0x80;
				derivationData[15] = 0x01;
				var sRMac = XpSecurity.Mac_AES(kMac.Value, derivationData);

				return new TsmSession
				{
					SessEnc = sEnc,
					SessMac = sMac,
					SessRMac = sRMac,
					MacIcv = icv
				};
			}
			return null;
		}
		#endregion GenerateSessionKeys

		#region CalculateCryptograms
		protected TsmCryptograms CalculateCryptograms(ScpMode scp, byte[] sequenceCounter, byte[] hostChallenge, byte[] cardChallenge, byte[] derivationData, TsmSession session)
		{
			var hostCrypto = (byte[])null;
			var cardCrypto = (byte[])null;

			var data = new byte[24];
			//var myCipher = Cipher.GetInstance("DESede/CBC/NoPadding");
			//var ivSpec = new IvParameterSpec(Icv);

			if (
				(scp == ScpMode.SCP_UNDEFINED) ||
				(scp == ScpMode.SCP_01_05) ||
				(scp == ScpMode.SCP_01_15)
			)
			{
				///* Calculing Cryptogram */
				//data.Copy(hostChallenge, 0, 8);
				//data.Copy(cardChallenge, 0, 8, 8);
				//data.Copy(PADDING, 0, 8, 16);

				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessEnc, "DESede"), ivSpec);
				//var cardcryptogram = myCipher.DoFinal(data);
				//cardCrypto = new byte[8];
				//cardCrypto.Copy(cardcryptogram, 16, 8);

				//data.Copy(cardChallenge, 0, 8);
				//data.Copy(hostChallenge, 0, 8, 8);
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessEnc, "DESede"), ivSpec);
				//var hostcryptogram = myCipher.DoFinal(data);
				//hostCrypto = new byte[8];
				//hostCrypto.Copy(hostcryptogram, 16, 8);

				//return new TsmCryptograms
				//{
				//	CardCrypto = cardCrypto,
				//	HostCrypto = hostCrypto
				//};
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
				///* Calculing Card Cryptogram */
				//data.Copy(hostChallenge, 0, 8);
				//data.Copy(sequenceCounter, 0, 2, 8);
				//data.Copy(cardChallenge, 0, 6, 10);
				//data.Copy(PADDING, 0, PADDING.Length, 16);

				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessEnc, "DESede"), ivSpec);
				//var cardcryptogram = myCipher.DoFinal(data);
				//cardCrypto = new byte[8];
				//cardCrypto.Copy(cardcryptogram, 16, 8);

				///* Calculing Host Cryptogram */
				//data.Copy(sequenceCounter, 0, 2);
				//data.Copy(cardChallenge, 0, 6, 2);
				//data.Copy(hostChallenge, 0, 8, 8);
				//data.Copy(PADDING, 0, 16, PADDING.Length);
				//myCipher.Init(Cipher.ENCRYPT_MODE, new SecretKeySpec(session.SessEnc, "DESede"), ivSpec);
				//var hostcryptogram = myCipher.DoFinal(data);
				//hostCrypto = new byte[8];
				//hostCrypto.Copy(hostcryptogram, 16, 8);

				//return new TsmCryptograms
				//{
				//	CardCrypto = cardCrypto,
				//	HostCrypto = hostCrypto
				//};
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
				/* Calculing Card Cryptogram */
				derivationData[11] = TsmScpEnum.SCP03_DERIVATION4CardCryptogram; // 0x00
				derivationData[13] = 0x00;
				derivationData[14] = 0x40;
				derivationData[15] = 0x01;
				cardCrypto = XpSecurity.Mac_AES(session.SessMac, derivationData, macSizeInBits: 64);

				/* Calculing Host Cryptogram */
				derivationData[11] = TsmScpEnum.SCP03_DERIVATION4HostCryptogram;
				derivationData[13] = 0x00;
				derivationData[14] = 0x40;
				derivationData[15] = 0x01;
				hostCrypto = XpSecurity.Mac_AES(session.SessMac, derivationData, macSizeInBits: 64);

				return new TsmCryptograms
				{
					CardCrypto = cardCrypto,
					HostCrypto = hostCrypto
				};
			}

			return null;
		}
		#endregion CalculateCryptograms

		#region GetKey
		protected ISCKey GetKey(IEnumerable<ISCKey> keys, byte keySetVersion, byte keyId)
		{
			return keys
				.Where(x => x.Version == keySetVersion && x.Id == keyId)
				.FirstOrDefault();
		}
		#endregion GetKey

		#region GetInitIcv
		protected byte[] GetInitIcv(ScpMode scp)
		{
			if (
				(scp == ScpMode.SCP_UNDEFINED) ||
				(scp == ScpMode.SCP_01_15) ||
				(scp == ScpMode.SCP_01_05) ||
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
				return new byte[8];

			if (
				(scp == ScpMode.SCP_03_05) ||
				(scp == ScpMode.SCP_03_0D) ||
				(scp == ScpMode.SCP_03_25) ||
				(scp == ScpMode.SCP_03_2D) ||
				(scp == ScpMode.SCP_03_65) ||
				(scp == ScpMode.SCP_03_6D)
			)
				return new byte[16];

			return null;
		}
		#endregion GetInitIcv
	}
}

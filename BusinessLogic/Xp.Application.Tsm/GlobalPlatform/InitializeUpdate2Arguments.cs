using System;
using System.Linq;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Tsm.GlobalPlatform.Commands;

namespace Xp.Application.Tsm.GlobalPlatform
{
	/// <summary>
	/// Arguments InitializeUpdate2
	/// </summary>
	public class InitializeUpdate2Arguments
	{
		public ScpMode Scp { get; set; }

		public int Sw { get; set; }
		private byte[] Data { get; set; }

		public byte[] KeyDivData { get; set; }
		public byte KeyVersNumRec { get; set; }
		public byte ScpRec { get; set; }
		public byte[] SequenceCounter { get; set; }
		public byte[] CardChallenge { get; set; }
		public byte[] CardCryptoResp { get; set; }

		public static InitializeUpdate2Arguments Create(byte[] data, ScpMode desiredScp)
		{
			var parameters = new InitializeUpdate2Arguments();
			parameters.Data = data;

			// valores de los parametros comunes
			parameters.KeyDivData = parameters.Data.Take(10).ToArray(); // new byte[10].Copy(parameters.Data);
			parameters.KeyVersNumRec = parameters.Data[10];
			parameters.ScpRec = parameters.Data[11];

			//if (parameters.Sw != (int)ISO7816.SW_NO_ERROR)
			//	throw new ApplicationException("Invalid response SW after first INIT UPDATE command (" + parameters.Sw + ")");

			if (parameters.ScpRec == GP2xCommands.SCP01)
				return parameters.LoadScp01(parameters.Data, desiredScp);
			else if (parameters.ScpRec == GP2xCommands.SCP02)
				return parameters.LoadScp02(parameters.Data, desiredScp);
			else if (parameters.ScpRec == GP2xCommands.SCP03)
				return parameters.LoadScp03(parameters.Data, desiredScp);
			else
				throw new ApplicationException("SCP version not available (" + parameters.ScpRec + ")");
		}
		/// <summary>
		/// INITIALIZE UPDATE response in SCP 01 mode
		/// -0-----------------------09-10------11-12------------19-20-------------27-
		/// | Key Diversification Data | Key Info | Card Challenge | Card Cryptogram |
		/// --------------------------------------------------------------------------
		/// </summary>
		/// <param name="data"></param>
		/// <param name="desiredScp"></param>
		/// <returns></returns>
		protected InitializeUpdate2Arguments LoadScp01(byte[] data, ScpMode desiredScp)
		{
			if (data.Length != 28)
				throw new ApplicationException("Invalid response size after first INIT UPDATE command (" + data.Length + ")");
			
			CardChallenge = new byte[8].Copy(data, 12);
			CardCryptoResp = new byte[8].Copy(data, 20);

			if (desiredScp == ScpMode.SCP_UNDEFINED)
				Scp = ScpMode.SCP_01_05;
			else if (desiredScp == ScpMode.SCP_01_05)
				Scp = desiredScp;
			else if (desiredScp == ScpMode.SCP_01_15)
				Scp = desiredScp;
			else
				throw new ApplicationException("Desired SCP does not match with card SCP value (" + ScpRec + ")");

			return this;
		}
		/// <summary>
		/// INITIALIZE UPDATE response in SCP 01 mode
		/// -0-----------------------09-10------11-12------------19-20-------------27-
		/// | Key Diversification Data | Key Info | Card Challenge | Card Cryptogram |
		/// --------------------------------------------------------------------------
		/// </summary>
		/// <param name="data"></param>
		/// <param name="desiredScp"></param>
		/// <returns></returns>
		protected InitializeUpdate2Arguments LoadScp02(byte[] data, ScpMode desiredScp)
		{
			if (data.Length != 28)
				throw new ApplicationException("Invalid response size after first INIT UPDATE command (" + data.Length + ")");
			
			SequenceCounter = new byte[2].Copy(data, 12);
			CardChallenge = new byte[6].Copy(data, 14);
			CardCryptoResp = new byte[8].Copy(data, 20);

			if (desiredScp == ScpMode.SCP_UNDEFINED)
				Scp = ScpMode.SCP_02_15;
			else if (desiredScp == ScpMode.SCP_02_15)
				Scp = desiredScp;
			else if (desiredScp == ScpMode.SCP_02_14)
				Scp = desiredScp;
			else if (desiredScp == ScpMode.SCP_02_04)
				Scp = desiredScp;
			else if (desiredScp == ScpMode.SCP_02_05)
				Scp = desiredScp;
			else if (desiredScp == ScpMode.SCP_02_45)
				Scp = desiredScp;
			else if (desiredScp == ScpMode.SCP_02_55)
				Scp = desiredScp;
			else
				throw new ApplicationException("Desired SCP does not match with card SCP value (" + ScpRec + ")");

			return this;
		}
		/// <summary>
		/// INITIALIZE UPDATE response in SCP 03 mode
		/// -0-----------------------09-10------12-13--------------20-21------------28-29-------------31-
		/// | Key Diversification Data | Key Info | Card Challenge   |Card Cryptogram | Sequence Counter|
		/// ---------------------------------------------------------------------------------------------
		/// Sequence Counter is only present when SCP03 is configured for pseudo-random challenge generation.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="desiredScp"></param>
		/// <returns></returns>
		protected InitializeUpdate2Arguments LoadScp03(byte[] data, ScpMode desiredScp)
		{
			if (data.Length != 32 && data.Length != 29)
				throw new ApplicationException("Invalid response size after first INIT UPDATE command (" + data.Length + ")");

			CardChallenge = new byte[8].Copy(data, 13);
			CardCryptoResp = new byte[8].Copy(data, 21);
			if (data.Length == 32)
				SequenceCounter = new byte[3].Copy(data, 29);

			if (desiredScp == ScpMode.SCP_03_65)
				Scp = desiredScp;
			if (desiredScp == ScpMode.SCP_03_6D)
				Scp = desiredScp;
			if (desiredScp == ScpMode.SCP_03_05)
				Scp = desiredScp;
			if (desiredScp == ScpMode.SCP_03_0D)
				Scp = desiredScp;
			if (desiredScp == ScpMode.SCP_03_2D)
				Scp = desiredScp;
			if (desiredScp == ScpMode.SCP_03_25)
				Scp = desiredScp;

			return this;
		}
	}
}

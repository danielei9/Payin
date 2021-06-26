using Newtonsoft.Json;
using System.Collections.Generic;
using Xp.Application.Dto.Tsm.Mifare4Mobile;

namespace Xp.Application.Dto.Tsm.GlobalPlatform.States
{
	public class TsmState
    {
        public IEnumerable<byte[]> Script { get; set; }
        //public int? OperationId { get; set; }
		public string CardRandomId { get; set; }
		public IList<byte[]> Icvs { get; set; } = new List<byte[]>();
		// Select
		public int MaxDataField { get; set; }
		// InitializeUpdate
		public string HostChallenge { get; set; }
		//public ScpMode Scp { get; set; }
		//public byte KeyId { get; set; }
		public TsmSession Session { get; set; }
		//public SecLevel SecLevel { get; set; }
		public byte[] HostCrypto { get; set; }
		// InstallForPerso
		public string[][] Card { get; set; } = new string[16][];
		// StoreData
		//public bool LastBlock { get; set; }
		//public byte BlockNumber { get; set; }
		public byte Sector { get; set; }
		public BlockBitmap BlockBitmap { get; set; }

		#region Constructors
		[JsonConstructor]
        public TsmState()
		{
		}
		public TsmState(TsmState state)
        {
            Script = state.Script;
            //OperationId = state.OperationId;
			CardRandomId = state.CardRandomId;

			MaxDataField = state.MaxDataField;

			HostChallenge = state.HostChallenge;
			//Scp = state.Scp;
			//KeyId = state.KeyId;
			Session = state.Session;
			//SecLevel = state.SecLevel;
			HostCrypto = state.HostCrypto;

			Card = state.Card;
			
			//LastBlock = state.LastBlock;
			//BlockNumber = state.BlockNumber;
			Sector = state.Sector;
			BlockBitmap = state.BlockBitmap;
		}
		#endregion Constructors
	}
}

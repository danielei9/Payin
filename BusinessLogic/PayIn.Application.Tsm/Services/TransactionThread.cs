using PayIn.Application.Tsm.Services.CardConfiguration;
using PayIn.Application.Tsm.Services.CommChannel;
using PayIn.Application.Tsm.Services.OpalExtensions;
using System;
using System.Collections.Generic;
using Xp.Application.Tsm.GlobalPlatform.Commands;
using Xp.Application.Tsm.GlobalPlatform.Config;
using Xp.Application.Tsm.GlobalPlatform.SmartCardIo;

namespace PayIn.Application.Tsm.Services
{
	public class TransactionThread
	{
		public string TransactionId { get; private set; }
		private string UserName;
		private string Id;

		public Queue<CardResponse> CardResponseQueue { get; private set; }
		private CardConfig CardConfig;
		private LinkedList<CardConfig> CardsToDeleteList;
		public List<ApduCommand> List = new List<ApduCommand>();

		private byte[] sdKeyBytes = "60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F".FromHexadecimal();

		public byte[] CARDMANAGER_CARDLET_AID = "A0 00 00 01 51 00 00 00".FromHexadecimal();
		public byte[] M4M_SP_SD_AID = "A0 00 00 03 96 4D 34 4D A4 00 81 DB 69 01 00 01".FromHexadecimal();
		public byte[] M4M_SM_AID =    "A0 00 00 03 96 4D 34 4D 24 00 81 DB 69 02 00 01".FromHexadecimal();

		private byte[] updateSMMetadata = "20 12 E6 10 F0 F1 F2 F3 CF CE CD CC 84 85 86 87 4B 4A 49 48".FromHexadecimal();
		private byte[] addAndUpdateMdac = "21 20 D0 02 00 01 D2 08 85 6F C8 F8 81 7E CC CF D3 10 C3 13 8C D8 42 9E 2A 1E 36 C9 05 67 E6 82 DC 62".FromHexadecimal();
		private byte[] updateMifareClassic = "06 3B 01 00 07 85 6F C8 F8 81 7E CC CF 4c 2d 53 65 76 65 6e 20 45 6c 65 76 65 6e 00 00 64 00 00 00 9B FF FF FF 64 00 00 00 20 DF 20 DF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00".FromHexadecimal();

		/** CLASSIC EXAMPLE **/
		//	/card
		//	set-key 72/1/AES/606162636465666768696A6B6C6D6E6F 72/2/AES/606162636465666768696A6B6C6D6E6F 72/3/AES/606162636465666768696A6B6C6D6E6F
		//	set-key 72/1/AES/404142434445464748494A4B4C4D4E4F 72/2/AES/404142434445464748494A4B4C4D4E4F 72/3/AES/404142434445464748494A4B4C4D4E4F
		//	/select "A0000003964D344DA40081DB69010001"
		//	init-update 72
		//	ext-auth crmacenc
		//	personalize "A0000003964D344D240081DB69010001"
		//	send 80E28100142012E610F0F1F2F3CFCECDCC848586874B4A4948
		//	personalize "A0000003964D344D240081DB69010001"
		//	send 80E28100222120D0020001D2080B54570745FE3AE7D310C3138CD8429E2A1E36C90567E682DC62
		//	personalize "A0000003964D344D240081DB69010001"
		//	send 80E281001D061B0100010B54570745FE3AE700010203040506070809000102030405
		//	send 80E281004D064B01000F0B54570745FE3AE74c2d536576656e20456c6576656e0000640000009BFFFFFF6400000020DF20DF476f726b610000000000000000000000AAAAAAAAAAAA08778F00313131313131
		//	send 80E281004D063B010007856FC8F8817ECCCF4c2d536576656e20456c6576656e0000640000009BFFFFFF6400000020DF20DF00000000000000000000000000000000

		/** DESFIRE EXAMPLE **/
		// send 80E28100040402AA00 ??

		public TransactionThread(/*Queue<CardResponse> cardResponseQueue,*/ string transactionId, string userName, string id, CardConfig cardconfig, LinkedList<CardConfig> cardsToDeleteList)
		{
			//CardResponseQueue = cardResponseQueue;
			TransactionId = transactionId;
			UserName = userName;
			Id = id;
			CardConfig = cardconfig;
			CardsToDeleteList = cardsToDeleteList;
		}
		public void Run()
		{
			var m4mApplet = (M4MApplet) null;
			var channel = new HttpCardChannel(CardResponseQueue);

			byte[] userUpdateMifareClassic = GetUpdateMifareClassicUser();

			if (!Id.IsNullOrEmpty())
			{
				M4M_SM_AID[M4M_SM_AID.Length - 1] = (Id.FromHexadecimal())[0];
				M4M_SP_SD_AID[M4M_SP_SD_AID.Length - 1] = (Id.FromHexadecimal())[0];
			}

			try
			{
				var implementation = new M4MCommands();
				implementation.CardChannel = channel;

				m4mApplet = new M4MApplet(implementation, M4M_SP_SD_AID);

				//var defKey1 = new SCGPKey(0x48, 1, KeyType.AES_CBC, sdKeyBytes);
				//var defKey2 = new SCGPKey(0x48, 2, KeyType.AES_CBC, sdKeyBytes);
				//var defKey3 = new SCGPKey(0x48, 3, KeyType.AES_CBC, sdKeyBytes);

				//var keys = new[] { defKey1, defKey2, defKey3 };
				//m4mApplet.SetOffCardKeys(keys);

				List.Add(implementation.Select(M4M_SP_SD_AID));

				List.Add(implementation.InitializeUpdate(0x048, 0x00, SCPMode.SCP_03_6D));
				return;

				m4mApplet.ExternalAuthenticate(SecLevel.C_ENC_AND_C_MAC_AND_R_MAC);

				implementation.InstallForPerso(M4M_SM_AID);
				implementation.StoreData(updateSMMetadata, true, 0x00);

				implementation.InstallForPerso(M4M_SM_AID);
				implementation.StoreData(addAndUpdateMdac, true, 0x00);

				implementation.InstallForPerso(M4M_SM_AID);
				implementation.StoreData(userUpdateMifareClassic, true, 0x00);

				channel.Close("Card successfully updated");
			}
			catch
			{
				try
				{
					channel.Close("Error in TransactionThread");
				}
				catch { }
			}
		}
		public byte[] GetUpdateMifareClassicUser()
		{
			int nameLength = UserName.Length > 16 ? 16 : UserName.Length;
			updateMifareClassic.Copy(UserName.ToUtf8(), 0, nameLength, updateMifareClassic.Length - 16);

			return updateMifareClassic;
		}
	}
}

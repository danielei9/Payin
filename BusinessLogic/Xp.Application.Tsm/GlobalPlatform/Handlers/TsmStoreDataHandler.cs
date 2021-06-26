using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Dto.Tsm.Arguments;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Dto.Tsm.Mifare4Mobile;
using Xp.Application.Tsm.GlobalPlatform.Commands;
using Xp.Application.Tsm.GlobalPlatform.Constants;

namespace Xp.Application.Tsm.GlobalPlatform.Handlers
{
	public class TsmStoreDataHandler
	{
		public TsmInstallForPersoHandler InstallForPersoHandler { get; set; }

		#region CreateAsync
		public async Task<TsmExecuteArguments> CreateAsync(TsmExecuteArguments arguments, TsmState state, SecLevel secLevel, ScpMode scp)
		{
			return await Task.Run(() =>
			{
				var storeData = state.Script.FirstOrDefault();
				if (storeData == null)
					throw new ArgumentNullException("State", "Script not exists");
				state.Script = state.Script.Skip(1);

				if (storeData[0] == 0x07) // 0x07 READ MIFARE CLASSIC SECTOR
				{
					state.Sector = storeData[2];
					state.BlockBitmap = (BlockBitmap) ((storeData[3] << 8) | storeData[4]);
				}
				//else if (storeData[0] == 0x06) // 0x06 UPDATE MIFARE CLASSIC SECTOR
				//{
				//	state.Sector = storeData[2];
				//	state.BlockBitmap = (BlockBitmap)((storeData[3] << 8) | storeData[4]);
				//}

				// XAVI: Calcular si lastblock y el número de block
				//state.LastBlock = true;
				var lastBlock = true;
				//state.BlockNumber = 0x00;
				var blockNumber = 0x00;

				var command = new ApduCommand(
					(secLevel == SecLevel.NO_SECURITY_LEVEL) ?
						TsmApduCla.COMMAND_GP : // 0x80
						TsmApduCla.GET_CHALLENGE, // 0x84
					TsmApduIns.STOREDATA, // 0xE2
					(lastBlock == true) ?
						TsmApduP1.STOREDATA_LastBlock : // 0x81
						TsmApduP1.STOREDATA_MoreBlocks, // 0x01
					(byte)(blockNumber & 0xFF),
					storeData, //result
					(secLevel != SecLevel.NO_SECURITY_LEVEL) ?
						(byte)(storeData.Length + 8) :
						(byte)(storeData.Length)
				);

				arguments.Data.Add(command.ToHexadecimalWithMac(state, secLevel, scp));
				arguments.NextStep = NextStepEnum.StoreData;
				arguments.State = state.ToJson();

				return arguments;
			});
		}
		#endregion CreateAsync

		#region ExecuteAsync
		public async Task<TsmState> ExecuteAsync(TsmExecuteArguments arguments, TsmState state, byte[] aid, SecLevel secLevel, ScpMode scp)
		{
			return await Task.Run(() =>
			{
				var command = arguments.Data[0];
				arguments.Data.RemoveAt(0);

				var m4mCommand = command.FromHexadecimal()
					.GP_CheckMac(state, secLevel, scp)
					.GP_CheckGeneralErrors()
					.GP_CheckError(0x6A, 0x80, "Incorrect values in command data")
					.GP_CheckError(0x6A, 0x84, "Not enough memory space")
					.GP_CheckError(0x6A, 0x88, "Referenced data not found")
					.GP_CheckSuccess(false)
					;

				// 0x20 UPDATE SM METADATA
				if (m4mCommand[0] == 0x20)
					m4mCommand.M4M_ReadResponse(0x20, (data) =>
					{
						data
						.M4M_CheckGeneralErrors()
						.M4M_CheckSuccess();
					});
				// 0x21 ADD AND UPDATE MDAC
				else if (m4mCommand[0] == 0x21)
					m4mCommand.M4M_ReadResponse(0x21, (data) =>
					{
						data
						.M4M_CheckGeneralErrors()
						.M4M_CheckSuccess();
					});
				// 0x06 UPDATE MIFARE CLASSIC SECTOR
				else if (m4mCommand[0] == 0x06)
					m4mCommand.M4M_ReadResponse(0x06, (data) =>
					{
						data
						.M4M_CheckError(0x64, 0x00, "The previous CHECKSUM CHECK has failed or the previous UPDATE MIFARE CLASSIC SECTOR failed")
						.M4M_CheckError(0x6A, 0x86, "A parameter is missing or the length of a parameter is not correct.")
						.M4M_CheckError(0x65, 0xF3, "The M4M command is not supported by the VC / Service Manager, e.g.the associated VC is not a MIFARE Classic")
						.M4M_CheckError(0x63, 0xF3, "The command is rejected because a contactless session is ongoing")
						.M4M_CheckError(0x63, 0xE2, "The given command cannot be executed on the targeted VC entry: The VC entry state is EMPTY or DEAD or UNASSOCIATED [VC Management]")
						.M4M_CheckError(0x63, 0xE3, "The given command cannot be executed on the associated VC entry: The VC is swapped out of the MIFARE Implementation")
						.M4M_CheckError(0x63, 0xD4,
							"- Sector Number is out of range.For example, the sector number is greater than 15 for a MIFARE Classic 1Kbyte VC.\n" +
							"- A block reference in BlockBitmap exceeds the number of blocks supported by the targeted sector.For example, BlockBitmap is greater than ‘000F’ for a sector made of 4 blocks.\n" +
							"- A targeted block is block 0 of sector 0")
						.M4M_CheckError(0x63, 0xE9, "The provided password is invalid")
						.M4M_CheckGeneralErrors()
						.M4M_CheckSuccess();
					});
				// 0x07 READ MIFARE CLASSIC SECTOR
				else if (m4mCommand[0] == 0x07)
					m4mCommand.M4M_ReadResponse(0x07, (data) =>
					{
						var result = data
						.M4M_CheckError(0x64, 0x00, "The previous CHECKSUM CHECK has failed")
						.M4M_CheckError(0x6A, 0x86,
							"- A parameter is missing or the length of a parameter is not correct\n" +
							"- The block bitmap reference more than 14 blocks")
						.M4M_CheckError(0x65, 0xF3, "The M4M command is not supported by the VC/ Service Manager, e.g.the associated VC is not a MIFARE Classic")
						.M4M_CheckError(0x6A, 0x84, "All retrieved data do not fit in the remaining STORE DATA response bytes")
						.M4M_CheckError(0x63, 0xF3, "The command is rejected because a contactless session is ongoing")
						.M4M_CheckError(0x63, 0xE2, "The given command cannot be executed on the targeted VC entry: the VC entry state is EMPTY or DEAD or UNASSOCIATED[VC Management]")
						.M4M_CheckError(0x63, 0xE3, "The given command cannot be executed on the associated VC entry: The VC is swapped out of the MIFARE Implementation")
						.M4M_CheckError(0x63, 0xD4,
							"- Sector ID is out of range. For example, the sector ID is greater than 15 for a MIFARE Classic 1Kbyte VC\n" +
							"- A block reference in BlockBitmap exceeds the number of blocks supported by the targeted sector.For example, block bitmap is greater than ‘000F’ for a sector made of 4 blocks")
						.M4M_CheckError(0x63, 0xE9, "The provided password is invalid")
						.M4M_CheckGeneralErrors()
						.M4M_CheckSuccess(false)
						;

						if (state.Card[state.Sector] == null)
							state.Card[state.Sector] = new string[4];
						var i = 0;
						if ((state.BlockBitmap & BlockBitmap.Block0) != 0)
						{
							var value = result.CloneArray(16, i++ * 16);
							state.Card[state.Sector][0] = value.ToHexadecimal();
						}
						if ((state.BlockBitmap & BlockBitmap.Block1) != 0)
						{
							var value = result.CloneArray(16, i++ * 16);
							state.Card[state.Sector][1] = value.ToHexadecimal();
						}
						if ((state.BlockBitmap & BlockBitmap.Block2) != 0)
						{
							var value = result.CloneArray(16, i++ * 16);
							state.Card[state.Sector][2] = value.ToHexadecimal();
						}
						if ((state.BlockBitmap & BlockBitmap.Block3) != 0)
						{
							var value = result.CloneArray(16, i++ * 16);
							state.Card[state.Sector][3] = value.ToHexadecimal();
						}
					});
				else
					throw new NotImplementedException();

				return state;
			});
		}
		#endregion ExecuteAsync
	}
}
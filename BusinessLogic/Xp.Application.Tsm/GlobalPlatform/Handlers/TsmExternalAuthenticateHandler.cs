using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xp.Application.Dto.Tsm.Arguments;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Tsm.GlobalPlatform.Commands;
using Xp.Application.Tsm.GlobalPlatform.Constants;

namespace Xp.Application.Tsm.GlobalPlatform.Handlers
{
	internal class TsmExternalAuthenticateHandler
	{
		private TsmInstallForPersoHandler InstallForPersoHandler { get; set; }

		#region Constructors
		public TsmExternalAuthenticateHandler(TsmInstallForPersoHandler installForPersoHandler)
		{
			InstallForPersoHandler = installForPersoHandler;
		}
		#endregion Constructors

		#region CreateAsync
		public async Task<TsmExecuteArguments> CreateAsync(TsmExecuteArguments arguments, TsmState state, SecLevel secLevel, ScpMode scp, byte[] hostCrypto)
		{
			return await Task.Run(() =>
			{
				var command = new ApduCommand(
					TsmApduCla.GET_CHALLENGE, // 0x84
					TsmApduIns.EXTERNAL_AUTHENTICATE, // 0x82
					(byte)secLevel,
					0x00,
					hostCrypto
				);

				arguments.Data.Add(command.ToHexadecimalWithMac_ExternalAuthenticate(state.Session, scp));
				arguments.NextStep = NextStepEnum.ExternalAuthenticate;
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

				command.FromHexadecimal()
					.GP_CheckGeneralErrors()
					.GP_CheckSuccess();

				return state;
			});
		}
		#endregion ExecuteAsync
	}
}

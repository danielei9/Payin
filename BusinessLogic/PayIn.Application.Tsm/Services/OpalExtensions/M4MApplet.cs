using System.Collections.Generic;
using Xp.Application.Tsm.GlobalPlatform.Applet;
using Xp.Application.Tsm.GlobalPlatform.Commands;

namespace PayIn.Application.Tsm.Services.OpalExtensions
{
	public class M4MApplet : GPApplet
	{
		//public M4MFileControlInformation FileControlInformation { get; private set; }
		public List<ApduCommand> Commands = new List<ApduCommand>();

		public M4MApplet(ICommands implementation, byte[] aid)
			:base(implementation, aid)
		{
			//FileControlInformation = null;
		}

		//public ApduCommand Select()
		//{
		//	var command = Cmds.Select(Aid);

		//	//FileControlInformation = new M4MFileControlInformation(ret.Data);
		//	Commands.Add(command);
		//	return command;
		//}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiCampaignGetEventArguments : IArgumentsBase
	{
		public int CampaignId { get; set; }

		#region Constructors
		public ApiCampaignGetEventArguments(int id)
		{
			CampaignId = id;
		}
		public ApiCampaignGetEventArguments()
		{
		}
		#endregion Constructors
	}
}

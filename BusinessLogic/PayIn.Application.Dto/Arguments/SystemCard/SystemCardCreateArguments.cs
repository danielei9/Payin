using System;
using System.ComponentModel.DataAnnotations;
using PayIn.Common;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Arguments.SystemCard
{
	public class SystemCardCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.systemCard.name")]					 public string Name									{ get; set; }
		[Display(Name = "resources.systemCard.cardType")]				 public CardType CardType							{ get; set; }
		[Display(Name = "resources.systemCard.generationType")]			 public NumberGenerationType NumberGenerationType	{ get; set; }
		[Display(Name = "resources.systemCard.owner")]					 public int ConcessionOwnerId						{ get; set; }
		[DataType(DataType.MultilineText)]
		[Display(Name = "resources.systemCard.affiliationEmailBody")]	 public string AffiliationEmailBody					{ get; set; }
		[Display(Name = "resources.systemCard.synchronizationInterval")] public TimeSpan? SynchronizationInterval			{ get; set; }
		[Display(Name = "resources.systemCard.clientId")]                public string ClientId                             { get; set; }

		#region Constructor
		public SystemCardCreateArguments(string name, CardType cardType, NumberGenerationType numberGenerationType, int concessionOwnerId, string affiliationEmailBody, TimeSpan? synchronizationInterval, string clientId)
		{
			Name = name;
			CardType = cardType;
			NumberGenerationType = numberGenerationType;
			ConcessionOwnerId = concessionOwnerId;
			AffiliationEmailBody = affiliationEmailBody;
			SynchronizationInterval = synchronizationInterval;
			ClientId = clientId ?? "";
		}
		#endregion Constructor
	}
}

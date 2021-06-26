using System;
using System.ComponentModel.DataAnnotations;
using PayIn.Common;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Arguments.SystemCard
{
	public class SystemCardUpdateArguments : IArgumentsBase
	{
																											public int Id										{ get; set; }
		[Display(Name = "resources.systemCard.name")]														public string Name									{ get; set; }
		[Display(Name = "resources.systemCard.cardType")]													public CardType CardType							{ get; set; }
		[Display(Name = "resources.systemCard.generationType")]												public NumberGenerationType NumberGenerationType	{ get; set; }
		[Display(Name = "resources.systemCard.photoUrl")]				[DataType(DataType.ImageUrl)]		public string PhotoUrl								{ get; set; }
		[Display(Name = "resources.systemCard.owner")]														public int ConcessionOwnerId						{ get; set; }
		[Display(Name = "resources.systemCard.affiliationEmailBody")]	[DataType(DataType.MultilineText)]	public string AffiliationEmailBody					{ get; set; }
		[Display(Name = "resources.systemCard.synchronizationInterval")]									public TimeSpan? SynchronizationInterval				{ get; set; }
		[Display(Name = "resources.systemCard.clientId")]													public string ClientId { get; set; }

		#region Constructor
		public SystemCardUpdateArguments(int id, string name, CardType cardType, NumberGenerationType numberGenerationType, string photoUrl, int concessionOwnerId, string affiliationEmailBody, TimeSpan? synchronizationInterval, string clientId)
		{
			Id = id;
			Name = name;
			CardType = cardType;
			NumberGenerationType = numberGenerationType;
			PhotoUrl = photoUrl ?? "";
			ConcessionOwnerId = concessionOwnerId;
			AffiliationEmailBody = affiliationEmailBody;
			SynchronizationInterval = synchronizationInterval;
			ClientId = clientId ?? "";
		}
		#endregion Constructor
	}
}

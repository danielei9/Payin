using System;
using PayIn.Domain.Public;

namespace PayIn.Application.Dto.Results.SystemCard
{
	public class SystemCardGetResult
	{
       	public int Id { get; set; }
		public string Name { get; set; }
		public int ConcessionOwnerId { get; set; }
		public string Owner { get; set; }
		public int? ProfileId { get; set; }
		public int CardType { get; set; }
		public int NumberGenerationType { get; set; }
		public int MembersCount { get; set; }
		public string PhotoUrl { get; set; }
		public string AffiliationEmailBody { get; set; }
		public TimeSpan? SynchronizationInterval { get; set; }
		public string ClientId { get; set; }
	}
}

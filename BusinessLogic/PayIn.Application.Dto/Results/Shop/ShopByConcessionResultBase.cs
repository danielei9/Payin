using System;
using System.Collections.Generic;
using PayIn.Common;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results.Shop
{
	public class ShopByConcessionResultBase : ResultBase<ShopByConcessionResult>
	{
		public class Card
		{
			public int		Id					{ get; set; }
			public string	Text				{ get; set; }
		}

		public class Entrance
		{
			public int		EventId				{ get; set; }
			public string	EventName			{ get; set; }
			public string	EntranceTypeName	{ get; set; }
			public int		Quantity			{ get; set; }
		}

		public class Ticket
		{
			public int Id { get; set; }
			public DateTime DateTime { get; set; }
			public TicketType Type { get; set; }
			public string TypeName { get; set; }
			public decimal Amount { get; set; }
			public TicketStateType State { get; set; }
			public string StateName { get; set; }
			public IEnumerable<EntranceType> EntranceTypes { get; set; }
			public IEnumerable<ProductLine> ProductLines { get; set; }
		}

		//public class ServiceOperation
		//{
		//	public int Id { get; set; }
		//	public int? LastSeq { get; set; }
		//	public int? ESeq { get; set; }
		//	public DateTime Date { get; set; }
		//	public int TicketId { get; set; }
		//	public ServiceOperationState State { get; set; }
		//	public ServiceOperationType Type { get; set; }
		//}

		public class EntranceType
		{
			public decimal	Quantity			{ get; set; }
			public string	EntranceTypeName	{ get; set; }
			public string	EventName			{ get; set; }
		}
        public class ProductLine
        {
            public decimal  Quantity        { get; set; }
            public string   ProductName     { get; set; }
            public decimal  Price           { get; set; }
        }

        public class Product
		{
			public int		ProductId	{ get; set; }
			public string	ProductName { get; set; }
			public string	PhotoUrl	{ get; set; }
			public decimal	Quantity	{ get; set; }
			public decimal	Price		{ get; set; }
		}

		public class ServiceDocument
		{
			public string	ServiceDocumentName { get; set; }
			public string	Url					{ get; set; }
		}

		public string						ConcessionName		{ get; set; }
		public string						ConcessionPhotoUrl	{ get; set; }
		public string						ConcessionLogoUrl	{ get; set; }
		public IEnumerable<Card>			Cards				{ get; set; }

		public IEnumerable<SelectorResult>	ServiceCards		{ get; set; }
		public int							ServiceCardId		{ get; set; }
		public string						ServiceCardName		{ get; set; }
		public bool							InBlackList			{ get; set; }

		public string						OwnerName			{ get; set; }
		public string						CardUid				{ get; set; }
		public decimal						LastBalance			{ get; set; }
		public decimal						PendingBalance		{ get; set; }
		public IEnumerable<Entrance>		Entrances			{ get; set; }
		public IEnumerable<Product>			Products			{ get; set; }
		public IEnumerable<Ticket>			Tickets				{ get; set; }
		public int                          PurseId				{ get; set; }
		public IEnumerable<ServiceDocument> ServiceDocuments	{ get; set; }
        public bool                         IsLinkedCard        { get; set; }

    }
}
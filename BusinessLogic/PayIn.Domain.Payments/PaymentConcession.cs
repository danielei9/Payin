using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class PaymentConcession : IEntity
	{
		                                      public int         Id                            { get; set; }
		                                      public decimal     Vat                           { get; set; }
		                                      public decimal?    IncomeTax                     { get; set; }																					       
		                                      public string      Observations                  { get; set; }
		[Required(AllowEmptyStrings = true)]  public string      Phone                         { get; set; }
		[Required(AllowEmptyStrings = false)] public string      BankAccountNumber             { get; set; }
		[Required(AllowEmptyStrings = true)]  public string      MerchantCode                  { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string      FormUrl                       { get; set; }
		[Required(AllowEmptyStrings = false)] public string      Address                       { get; set; }
		                                      public DateTime    CreateConcessionDate          { get; set; }
		                                      public DateTime?   LiquidationRequestDate        { get; set; }
		[Precision(9,6)]                      public decimal     LiquidationAmountMin          { get; set; }
		                                      public decimal     PayinCommision                { get; set; }
		[Required(AllowEmptyStrings = true)]  public string      OnPaymentMediaCreatedUrl      { get; set; }
		[Required(AllowEmptyStrings = true)]  public string      OnPaymentMediaErrorCreatedUrl { get; set; }
		[Required(AllowEmptyStrings = true)]  public string      OnPayedUrl                    { get; set; }
		[Required(AllowEmptyStrings = true)]  public string      OnPayedEmail                  { get; set; }
                                              public bool        OnlineCartActivated           { get; set; }
											  public bool		 CanHasPublicEvent		       { get; set; }
											  public bool		 AllowUnsafePayments	       { get; set; }
		[Required(AllowEmptyStrings = true)]  public string      Key					       { get; set; }
											  public KeyType?	 KeyType				       { get; set; }
		                                      public bool        GroupEntranceTypeByEvent      { get; set; }
		[Required(AllowEmptyStrings = true)]  public string      PhotoUrl				       { get; set; } = "";

		#region Concession
		public int ConcessionId { get; set; }
		public ServiceConcession Concession { get; set; }
		#endregion Concession

		#region Family
		[InverseProperty("PaymentConcession")]
		public ICollection<ProductFamily> Families { get; set; } = new List<ProductFamily>();
		#endregion Family

		#region Product
		[InverseProperty("PaymentConcession")]
		public ICollection<Product> Products { get; set; } = new List<Product>();
        #endregion Product

        #region Exhibitors
        [InverseProperty("PaymentConcession")]
        public ICollection<Exhibitor> Exhibitors { get; set; } = new List<Exhibitor>();
        #endregion Exhibitors

        #region Notices
        [InverseProperty("PaymentConcession")]
        public ICollection<Notice> Notices { get; set; } = new List<Notice>();
        #endregion Notices

        #region Events
        [InverseProperty("PaymentConcession")]
		public ICollection<Event> Events { get; set; } = new List<Event>();
		#endregion Events

		#region EntranceSystems
		public ICollection<EntranceSystem> EntranceSystems { get; set; } = new List<EntranceSystem>();
        #endregion EntranceSystems

        #region Tickets
        [InverseProperty("Concession")]
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        #endregion Tickets

        #region LiquidationTickets
        [InverseProperty("LiquidationConcession")]
        public ICollection<Ticket> LiquidationTickets { get; set; } = new List<Ticket>();
		#endregion LiquidationTickets

		#region TicketTemplate
		public int? TicketTemplateId { get; set; }
		public TicketTemplate TicketTemplate { get; set; }
        #endregion TicketTemplate

        #region Shipments
        [InverseProperty("Concession")]
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
        #endregion Shipments

        #region Liquidations
        [InverseProperty("Concession")]
        public ICollection<Liquidation> Liquidations { get; set; } = new List<Liquidation>();
        #endregion Liquidations

        #region LiquidationLiquidations
        [InverseProperty(nameof(Liquidation.LiquidationConcession))]
        public ICollection<Liquidation> LiquidationLiquidations { get; set; } = new List<Liquidation>();
        #endregion LiquidationLiquidations

        #region PaymentWorkers 
        [InverseProperty("Concession")]
        public ICollection<PaymentWorker> PaymentWorkers { get; set; } = new List<PaymentWorker>();
        #endregion PaymentWorkers

        #region Campaigns
        [InverseProperty("Concession")]
        public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
        #endregion Campaigns

        #region PaymentConcessionCampaign
        [InverseProperty("PaymentConcession")]
        public ICollection<PaymentConcessionCampaign> PaymentConcessionCampaigns { get; set; } = new List<PaymentConcessionCampaign>();
		#endregion PaymentConcessionCampaign

		#region PaymentUsers
		[InverseProperty("Concession")]
		public ICollection<PaymentUser> PaymentUsers { get; set; } = new List<PaymentUser>();
        #endregion PaymentUsers

        #region Purses
        [InverseProperty("Concession")]
        public ICollection<Purse> Purses { get; set; } = new List<Purse>();
        #endregion Purses

        #region PaymentConcessionPurses
        [InverseProperty("PaymentConcession")]
        public ICollection<PaymentConcessionPurse> PaymentConcessionPurses { get; set; } = new List<PaymentConcessionPurse>();
        #endregion PaymentConcessionPurses

        #region Promotions
        //[InverseProperty("Concession")]
        //public ICollection<Promotion> Promotions { get; set; }
        #endregion Promotions

        #region City
        public int? CityId { get; set; }
		public ServiceCity City { get; set; }
        #endregion City

        #region PaymentMedias
        [InverseProperty("PaymentConcession")]
		public ICollection<PaymentMedia> PaymentMedias { get; set; } = new List<PaymentMedia>();
        #endregion PaymentMedias

        #region LiquidationConcession
        public int? LiquidationConcessionId { get; set; }
        [ForeignKey("LiquidationConcessionId")]
        public PaymentConcession LiquidationConcession { get; set; }
        #endregion LiquidationConcession

        #region LiquidationConcessionsInverse
        [InverseProperty(nameof(PaymentConcession.LiquidationConcession))]
        public List<PaymentConcession> LiquidationConcessionsInverse { get; set; } = new List<PaymentConcession>();
        #endregion LiquidationConcessionsInverse

        #region AccountLines
        [InverseProperty(nameof(AccountLine.Concession))]
        public List<AccountLine> AccountLines { get; set; } = new List<AccountLine>();
        #endregion AccountLines
    }
}

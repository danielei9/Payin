namespace PayIn.Web.App.Factories
{
	public partial class TicketFactory
	{
		public static string UrlApi { get { return "/Api/Ticket"; } }
		public static string UrlDetailApi { get { return "/Api/TicketDetail"; } }
		public static string Url { get { return "/Ticket"; } }

		public static string UrlUserApi { get { return "/Api/Ticket/User"; } }
		public static string UrlSupplierApi { get { return "/Api/Ticket/Supplier"; } }
		public static string UrlSupplier { get { return "/SupplierTicket"; } }

		#region GetAll
		public static string GetAllName { get { return "ticketgetall"; } }
		public static string GetAll { get { return Url; } }
		public static string GetAllApi { get { return UrlApi; } }
		public static string GetAllCsv { get { return GetAllApi + "/csv"; } }
        #endregion GetAll

        #region SystemCard
        public static string SystemCardName { get { return "ticketsystemcard"; } }
        //public static string SystemCard { get { return Url + "/SystemCard"; } }
        public static string SystemCardApi { get { return UrlApi + "/SystemCard"; } }
        #endregion SystemCard

        #region Graph
        public static string GraphName { get { return "ticketgraph"; } }		
		public static string GraphApi { get { return UrlApi + "/Graph"; } }		
		#endregion Graph
		
		#region Create
		public static string CreateName { get { return "ticketcreate"; } }
		public static string Create { get { return Url + "/Create"; } }
		public static string CreateApi { get { return UrlApi; } }
		public static string CreateCsv { get { return CreateApi + "/csv"; } }
		#endregion Create

		#region Update
		public static string UpdateName { get { return "ticketupdate"; } }
		public static string UpdateApi { get { return UrlApi; } }
        #endregion Update

        #region Pay
        public static string PayName { get { return "ticketpay"; } }
        public static string Pay { get { return Url + "/Pay"; } }
        public static string PayApi { get { return UrlApi + "/Pay"; } }
        #endregion Pay

        #region Details
        public static string DetailsName { get { return "ticketdetails"; } }
		public static string DetailsApi { get { return UrlApi + "/Details"; } }
		#endregion Details

		#region TicketDetails
		public static string TicketCreateDetailName { get { return "ticketcreatedetail"; } }
		public static string TicketCreateDetail { get { return Url + "/CreateDetail"; } }
		public static string TicketCreateDetailApi { get { return UrlApi + "/:id/UpdateDetail"; } }
		public static string TicketCreateDetailCsv { get { return TicketCreateDetailApi + "/csv"; } }
		#endregion

		#region UpdateDetail
		public static string TicketUpdateDetailName { get { return "ticketupdatedetail"; } }
		public static string TicketUpdateDetail { get { return Url + "/UpdateDetail"; } }
		public static string TicketUpdateDetailApi { get { return UrlDetailApi + "/:id"; } }
		public static string TicketUpdateDetailCsv { get { return TicketUpdateDetailApi + "/csv"; } }
		#endregion

		#region Delete
		public static string Delete { get { return "ticketdetailsdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion

		#region DeleteDetail
		public static string DeleteDetail { get { return "ticketdeletedetail"; } }
		public static string DeleteDetailApi { get { return UrlApi + "/:id/DeleteDetail"; } }
		#endregion

		#region User
		#region CreateOra
		public static string UserCreateOraName { get { return "TicketCreate"; } }
		public static string UserCreateOraApi { get { return UrlUserApi + "PayOra"; } }
		public static string UserCreateOra { get { return Url + "/Create"; } }
		public static string UserCreateOraUri { get { return TicketFactory.UserRetrieveAllOra + "#" + UserCreateOraName; } }
		//public static string UserCreateOraUri { get { return "#" + UserCreateOra; } }
		public static string UserCreateAndPayOraTemplate { get { return UserCreateOra; } }
		#endregion CreateOra

		#region CreateAndPayOra
		public static string UserCreateAndPayOraName { get { return "TicketCreateAndPay"; } }
		public static string UserCreateAndPayOraApi { get { return UrlUserApi + "CreateAndPay"; } }
		public static string UserCreateAndPayOra { get { return Url + "/Create"; } }
		public static string UserCreateAndPayOraUri { get { return "#" + UserCreateOra; } }
		public static string UserUserCreateOraTemplate { get { return UserCreateOra; } }
		#endregion CreateAndPayOra

		#region RetrieveAllCharge
		public static string UserRetrieveAllChargeName { get { return "TicketRetrieveAll"; } }
		public static string UserRetrieveAllChargeApi { get { return UrlUserApi; } }
		public static string UserRetrieveAllCharge { get { return Url; } }
		public static string UserRetrieveAllChargeUri { get { return "#" + UserRetrieveAllCharge; } }
		public static string UserRetrieveAllChargeTemplate { get { return UserRetrieveAllCharge; } }
		#endregion RetrieveAllCharge

		#region RetrieveAllOra
		public static string UserRetrieveAllOraName { get { return "TicketRetrieveAllOra"; } }
		public static string UserRetrieveAllOraApi { get { return UrlUserApi + "Ora"; } }
		public static string RetrieveAllOraCsv { get { return UrlApi + "/UserOraCsv"; } }
		public static string UserRetrieveAllOra { get { return Url + "/IndexOra"; } }
		public static string UserRetrieveAllOraUri { get { return "#" + UserRetrieveAllOra; } }
		public static string UserRetrieveAllOraTemplate { get { return UserRetrieveAllOra; } }
		#endregion RetrieveAllOra

		//#region Delete
		//public static string DeleteName { get { return "Delete"; } }
		//public static string DeleteApi { get { return UrlUserApi; } }
		//public static string DeleteUri { get { return UserRetrieveAllCharge + "#" + DeleteName; } }
		//#endregion Delete
		#endregion User

		#region Supplier

		#region RetrieveAllCharge
		public static string SupplierRetrieveAllChargeName { get { return "TicketRetrieveAllBySupplier"; } }
		public static string SupplierRetrieveAllChargeApi { get { return UrlSupplierApi; } }
		public static string SupplierRetrieveAllCharge { get { return UrlSupplier; } }
		public static string SupplierRetrieveAllChargeUri { get { return "#" + SupplierRetrieveAllCharge; } }
		public static string SupplierRetrieveAllChargeTemplate { get { return SupplierRetrieveAllCharge; } }
		#endregion RetrieveAllCharge

		#endregion Supplier

		#region SellEntrances
		public static string SellEntrancesApi { get { return UrlApi + "/SellEntrances"; } }
        #endregion SellEntrances

        #region BuyEntrances
        public static string BuyEntrancesApi { get { return UrlApi + "/BuyEntrances"; } }
		#endregion BuyEntrances

		#region BuyManyEntrances
		public static string BuyManyEntrancesApi { get { return UrlApi + "/BuyManyEntrances"; } }
		#endregion BuyManyEntrances

		#region BuyManyProducts
		public static string BuyManyProductsApi { get { return UrlApi + "/BuyManyProducts"; } }
		#endregion BuyManyProducts

		#region GiveEntrances
		//public static string GiveEntrances { get { return "ticketgive"; } }
		public static string GiveEntrancesApi { get { return UrlApi + "/GiveEntrances"; } }
		#endregion GiveEntrances

		#region Recharge
		//public static string Recharge { get { return "ticketrechargebalance"; } }
		public static string RechargeApi { get { return UrlApi + "/RechargeBalance"; } }
		#endregion Recharge

		#region Donate
		public static string Donate { get { return "ticketdonatebalance"; } }
		public static string DonateApi { get { return UrlApi + "/DonateBalance"; } }
		#endregion Donate

		#region BuyRechargeBalance
		public static string BuyRechargeBalanceApi { get { return UrlApi + "/BuyRechargeBalance"; } }
		#endregion BuyRechargeBalance
	}
}

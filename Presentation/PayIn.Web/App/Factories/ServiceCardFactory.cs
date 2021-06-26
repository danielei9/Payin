
namespace PayIn.Web.App.Factories
{
	public partial class ServiceCardFactory
	{
		public static string UrlApi { get { return "/Api/ServiceCard"; } }
		public static string Url { get { return "/ServiceCard"; } }

		#region Get
		public static string GetName { get { return "servicecardget"; } }
		public static string Get { get { return Url; } }
		public static string GetApi { get { return UrlApi + "/:id"; } }
		#endregion Get

		#region GetAll
		public static string GetAll { get { return "servicecardgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetAllMine
		public static string GetAllMyCards { get { return "servicecardgetall_mycards"; } }
		public static string GetAllMyCards_prev { get { return "servicecardgetall_mycards_prev"; } }
		public static string GetAllMyCardsApi { get { return UrlApi + "/MyCards"; } }
		#endregion GetAllMine

		#region Create
		public static string CreateName { get { return "servicecardcreate"; } }
		public static string CreateApi { get { return UrlApi + "/Create"; } }
		#endregion Create

		#region UnlockCard
		public static string UnlockCardName { get { return "servicecardunlock"; } }
		public static string UnlockCardApi { get { return UrlApi + "/UnlockCard"; } }
		#endregion UnlockCard

		#region lockCard
		public static string LockCardApi { get { return UrlApi + "/lockCard"; } }
		public static string LockCardName { get { return "servicecardlock"; } }
		#endregion lockCard

		#region Delete
		public static string DeleteApi { get { return UrlApi + "/Delete"; } }
		public static string DeleteName { get { return "servicecarddelete"; } }
		#endregion Delete

		#region Destroy
		public static string DestroyApi { get { return UrlApi + "/Destroy"; } }
		public static string DestroyName { get { return "servicecarddestroy"; } }
		#endregion Destroy

		#region DonateMoney
		public static string DonateMoney { get { return "servicecarddestroy"; } }
		#endregion DonateMoney

		#region LinkCard
		public static string LinkCardName { get { return "servicecardlinkcard"; } }
		public static string LinkCardApi { get { return UrlApi + "/LinkCard/:id"; } }
		#endregion LinkCard

		#region Link
		public static string LinkName { get { return "servicecardlink"; } }
		public static string LinkApi { get { return UrlApi + "/Link"; } }
		#endregion Link

		#region UnlinkCard
		public static string UnlinkCardName { get { return "servicecardunlink"; } }
		public static string UnlinkCardApi { get { return UrlApi + "/Unlink"; } }
		#endregion UnlinkCard

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
        #endregion RetrieveSelector	
    }
}

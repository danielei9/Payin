
namespace PayIn.Web.App.Factories
{
	public partial class ServiceCardBatchFactory
	{
		public static string UrlApi { get { return "/Api/ServiceCardBatch"; } }
		public static string Url { get { return "/ServiceCardBatch"; } }

		#region GetAll
		public static string GetAllName { get { return "servicecardbatchgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region Create
		public static string CreateName { get { return "servicecardbatchcreate"; } }
		public static string CreateApi { get { return UrlApi + "/Create"; } }
		#endregion Create

		#region Unlock
		public static string UnlockName { get { return "servicecardbatchunlock"; } }
		public static string UnlockApi { get { return UrlApi + "/Unlock/:id"; } }
		#endregion Unlock

		#region Lock		
		public static string LockName { get { return "servicecardbatchlock"; } }
		public static string LockApi { get { return UrlApi + "/Lock/:id"; } }
		#endregion Lock

		#region Delete		
		public static string DeleteName { get { return "servicecardbatchdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion Delete

		#region RetrieveSelector
		public static string RetrieveSelectorApi { get { return UrlApi + "/RetrieveSelector"; } }
		#endregion RetrieveSelector
	}
}

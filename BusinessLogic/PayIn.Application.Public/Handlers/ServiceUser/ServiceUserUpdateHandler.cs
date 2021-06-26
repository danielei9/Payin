using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserUpdateHandler : 
		IServiceBaseHandler<ServiceUserUpdateArguments>
	{
		[Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> RepositoryServiceCard { get; set; }
		[Dependency] public IEntityRepository<GreyList> RepositoryGreyList { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceUserUpdateArguments arguments)
		{
			var serviceUser = (await Repository.GetAsync(arguments.Id, "Card", "OnwnerCards"));

			string oldName = serviceUser.Name.ToString() + " " + serviceUser.LastName.ToString();
			string newName = arguments.Name.ToString() + " " + arguments.LastName.ToString();
			bool nameWasChanged = !(oldName == newName );

			// Service user
			serviceUser.VatNumber = arguments.VatNumber ?? "";
			serviceUser.Name = arguments.Name;
			serviceUser.LastName = arguments.LastName;
			serviceUser.BirthDate = arguments.BirthDate;
			serviceUser.Phone = arguments.Phone;
			serviceUser.Email = arguments.Email;
			serviceUser.Code = arguments.Code;
			serviceUser.Observations = arguments.Observations;
			serviceUser.Address = arguments.Address;
			//serviceUser.Card.State = arguments.CardState;
			if (nameWasChanged)
			{
				var now = DateTime.Now.ToUTC();
				foreach(ServiceCard serviceCard in serviceUser.OnwnerCards)
				{
					var createItem = new GreyList
					{
						Uid = serviceCard.Uid,
						RegistrationDate = now,
						Action = PayIn.Domain.Transport.GreyList.ActionType.ModifyField,
						Field = "NAME",
						NewValue = newName,
						Resolved = false,
						ResolutionDate = null,
						Machine = PayIn.Domain.Transport.GreyList.MachineType.All,
						Source = PayIn.Domain.Transport.GreyList.GreyListSourceType.Payin,
						State = GreyList.GreyListStateType.Active
					};
					await RepositoryGreyList.AddAsync(createItem);
				}
			}


			//			// Save doc in Azure
			//			//if (arguments.AssertDoc.Content.Length > 0)
			//			if (arguments.AssertDoc != null)
			//			{
			//				var repositoryAzure = new AzureBlobRepository();
			//				var guid = Guid.NewGuid();
			//				var oldAssertDoc = serviceUser.AssertDoc;

			//#if TEST || HOMO || DEBUG || EMULATOR
			//				serviceUser.AssertDoc = ServiceUserResources.AssertDocUrlTest.FormatString(serviceUser.Id, guid);
			//#else // TEST
			//				serviceUser.AssertDoc = ServiceUserResources.AssertDocUrl.FormatString(serviceUser.Id, guid);
			//#endif // TEST
			//				repositoryAzure.SaveFile(ServiceUserResources.AssertDocShortUrl.FormatString(serviceUser.Id, guid), arguments.AssertDoc);

			//				//repositoryAzure.SaveFile(ServiceUserResources.AssertDocShortUrl.FormatString(serviceUser.Id, guid), arguments.AssertDoc.Content);

			//				//if (oldAssertDoc != null && oldAssertDoc != "")
			//				//{
			//				//	var route = Regex.Split(oldAssertDoc, "[/?]");
			//				//	var fileName = route[route.Length - 2] + "/" + route[route.Length - 1];
			//				//	repositoryAzure.DeleteFile(oldAssertDoc);
			//				//}
			//			}

			return serviceUser;
		}
		#endregion ExecuteAsync
	}
}

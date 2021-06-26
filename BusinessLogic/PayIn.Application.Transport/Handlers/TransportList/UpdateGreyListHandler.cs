using PayIn.Application.Dto.Transport.Arguments.TransportList;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class UpdateGreyListHandler :
		IServiceBaseHandler<UpdateGreyListArguments>
	{
		private readonly IEntityRepository<GreyList> Repository;
		List<GreyList> GreyLists = new List<GreyList>();

		#region constructors
		public UpdateGreyListHandler(
			IEntityRepository<GreyList> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion constructors

		#region executeasync
		public async Task<dynamic> ExecuteAsync(UpdateGreyListArguments arguments)
		{
			FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://193.144.127.124/LG/LG.TXT");
			request.Method = WebRequestMethods.Ftp.DownloadFile;
			request.Credentials = new NetworkCredential("listaPayIn", "p4y1n4231");
			FtpWebResponse response = (FtpWebResponse)request.GetResponse();
			Stream responseStream = response.GetResponseStream();
			System.IO.StreamReader file = new System.IO.StreamReader(responseStream);
			string line;

			while ((line = file.ReadLine()) != null)
			{
				dynamic c = line.Split(';');
				var item = new GreyList
				{
					Uid = long.Parse(c[0]),
					OperationNumber = int.Parse(c[1]),
					RegistrationDate = Convert.ToDateTime(c[2]),
					Action = (PayIn.Domain.Transport.GreyList.ActionType) int.Parse(c[3]),
					Field = c[4],
					NewValue = c[5],
					Resolved = c[6] == "1" ? true : false,
					ResolutionDate = c[7] == "" ? null : Convert.ToDateTime(c[7]),
					Machine = (PayIn.Domain.Transport.GreyList.MachineType) int.Parse(c[8]),
					Source = GreyList.GreyListSourceType.SigAPunt
				};
				GreyLists.Add(item);
			}
			foreach (var greyList in GreyLists.ToList())
			{
				await Repository.AddAsync(greyList);
			}
			response.Close();
			responseStream.Close();
			return null;
		}
		#endregion executeasync
	}
}
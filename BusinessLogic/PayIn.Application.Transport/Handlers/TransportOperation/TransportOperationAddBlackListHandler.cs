using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Transport.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class TransportOperationAddBlackListHandler :
		IServiceBaseHandler<TransportOperationAddBlackListArguments>
	{
		private readonly IEntityRepository<TransportOperation> Repository;
		private readonly IEntityRepository<BlackList> BlackListRepository;
		private readonly IUnitOfWork UnitOfWork;
		private readonly SigapuntService SigapuntService;

		#region Contructors
		public TransportOperationAddBlackListHandler(
			IEntityRepository<TransportOperation> repository,
			IEntityRepository<BlackList> blacklistrepository,
			IUnitOfWork unitOfWork,
			SigapuntService sigapuntService
		)
		{
			if (repository == null)	throw new ArgumentNullException("repository");
			if(blacklistrepository == null)	throw new ArgumentNullException("blacklistrepository");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if(sigapuntService == null) throw new ArgumentNullException("sigapuntService");

			Repository = repository;
			BlackListRepository = blacklistrepository;
			UnitOfWork = unitOfWork;
			SigapuntService = sigapuntService;
		}
		#endregion Contructors
        
		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportOperationAddBlackListArguments>.ExecuteAsync(TransportOperationAddBlackListArguments arguments)
		{
			var now = DateTime.Now;
			var yesterday = now.AddDays(-1);
			
			var operations = (await Repository.GetAsync())
				.Where(x => 
				x.DateTimeEventError != null &&
				x.DateTimeEventError < yesterday && 
				x.ConfirmationDate == null
				)
			    .ToList();
			/*	var loadedList = new List<BlackList>();
				var request = WebRequest.Create("ftp://193.144.127.124/LN/LN.TXT");
				request.Method = WebRequestMethods.Ftp.AppendFile;			
				request.Credentials = new NetworkCredential("listaPayIn", "p4y1n4231");

				var text = "";
				foreach (var operation in operations)
				{
					 text += operation.Uid + ";" + now + ";2047;0;;0;0;0;\n";			

					operation.ConfirmationDate = now;

					await UnitOfWork.SaveAsync();
				}
				request.ContentLength = text.Length;
				using (Stream request_stream = request.GetRequestStream())
				{
					byte[] bytes = Encoding.UTF8.GetBytes(text);
					request_stream.Write(bytes, 0, text.Length);
					request_stream.Close();
					FtpWebResponse response = (FtpWebResponse)request.GetResponse();
					response.Close();
				}
				*/

			foreach (var operation in operations)
			{
				await SigapuntService.INCLNAsync((long)operation.Uid, 1);

				operation.ConfirmationDate = now;

				await UnitOfWork.SaveAsync();
			}

			return 1;
		}
		#endregion ExecuteAsync
	}
}

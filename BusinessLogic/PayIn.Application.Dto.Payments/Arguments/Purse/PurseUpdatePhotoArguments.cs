using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Purse
{
	public class PurseUpdatePhotoArguments : IArgumentsBase
	{
		

		[DataSubType(DataSubType.Image)] 	public string Image { get; set; }
		                                    public int Id { get; set; }        

	#region Constructor
		public PurseUpdatePhotoArguments(string image,int purseId)
		{

			Image = image;
			Id = purseId;
		}
		#endregion Constructor
	
	
	}		
	
}

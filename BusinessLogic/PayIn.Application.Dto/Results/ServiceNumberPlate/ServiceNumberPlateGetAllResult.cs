using PayIn.Common;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ServiceNumberPlate
{
	public partial class ServiceNumberPlateGetAllResult
	{
		public int		Id					{ get; set; }
		public string	NumberPlate	{ get; set; }
		public string	Model				{ get; set; }
		public string	Color				{ get; set; }
	}
}

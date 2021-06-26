using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.GreyList
{
	public class GreyListModifyFieldArguments : IArgumentsBase
	{
		public long Id { get; private set; }
		public IEnumerable<GreyListModifyFieldArguments_Item> ModifyValues { get; private set; }
		public string[] ModifyBlock { get; private set; }		
		public string[] Block { get; private set; }
		public long Uid { get; private set; }
		public bool ModifyCard { get; private set; }

		#region Constructors
		public GreyListModifyFieldArguments(long id, IEnumerable<GreyListModifyFieldArguments_Item> modifyValues, string[] modifyBlock, string[] block, long uid, bool modifyCard)
		{
			Id = id;
			Uid = uid;
			ModifyValues = modifyValues;
			ModifyBlock = modifyBlock;
			Block = block;
			ModifyCard = modifyCard;
		}
		#endregion Constructors
	}
}

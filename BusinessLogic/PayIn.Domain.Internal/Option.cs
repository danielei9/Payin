using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Internal
{
	public class Option : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }
		public string Name      { get; set; }
		public string ValueType { get; set; }
		public string Value     { get; set; }
	}
}

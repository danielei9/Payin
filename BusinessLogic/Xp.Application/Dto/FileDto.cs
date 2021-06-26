namespace Xp.Application.Dto
{
	public class FileDto
	{
		public string Url { get; set; }
		public string Name { get; set; }
		public byte[] Content { get; set; }
		public bool Remove { get; set; }
	}
}

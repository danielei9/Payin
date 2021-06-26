namespace PayIn.Application.Transport.Services
{
    public class ServerResult
    {
        public bool Success { get; set; } = true;
        public string Result { get; set; }
        public string Exception { get; set; }
    }
}

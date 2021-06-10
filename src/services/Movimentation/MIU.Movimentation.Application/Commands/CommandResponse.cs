namespace MIU.Movimentations.Application.Commands
{
    public class CommandResponse
    {
        public CommandResponse(string message, int statusCode = 200)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}

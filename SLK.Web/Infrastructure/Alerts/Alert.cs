namespace SLK.Web.Infrastructure.Alerts
{
    public class Alert
    {
        public string AlertClass;

        public string Message;

        public Alert(string alertClass, string message)
        {
            AlertClass = alertClass;
            Message = message;
        }
    }
}
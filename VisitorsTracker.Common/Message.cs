using System.Globalization;

namespace VisitorsTracker.Common
{
    public class Message
    {
        public string Referrer { get; set; }

        public string UserAgent { get; set; }

        public string ClientIp { get; set; }

        public DateTime Date { get; set; }

        public override string ToString() =>
            $"{Date.ToString("o", CultureInfo.InvariantCulture)}|" +
            $"{(string.IsNullOrEmpty(Referrer) ? "null" : Referrer)}|" +
            $"{(string.IsNullOrEmpty(UserAgent) ? "null" : UserAgent)}|" +
            $"{ClientIp}";
    }
}
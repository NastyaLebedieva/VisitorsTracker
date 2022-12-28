using System.Globalization;

namespace Common
{
    public class Message
    {
        public string? Referrer { get; set; }

        public string? UserAgent { get; set; }

        public string ClientIp { get; set; }

        public DateTime Date { get; set; }

        public override string ToString() =>
            $"{Date.ToString("o", CultureInfo.InvariantCulture)}|{Referrer ?? "null"}|{UserAgent ?? "null"}|{ClientIp}";
    }
}
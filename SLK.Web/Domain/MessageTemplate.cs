namespace Slk.Domain.Core
{
    public class MessageTemplate
    {
        protected MessageTemplate() { }

        public long ID { get; protected set; }

        public string LanguageCode { get; protected set; }

        public string SystemName { get; protected set; }

        public string Bcc { get; protected set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool Active { get; protected set; }
    }
}

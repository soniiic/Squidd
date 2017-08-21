namespace Squidd.Commander.Domain.Entities
{
    public class Runner : BaseEntity
    {
        public string EndPoint { get; set; }

        public string ComputerName { get; set; }

        public string FriendlyName { get; set; }
    }
}

namespace Core.Entities
{
    public class BaseTransaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
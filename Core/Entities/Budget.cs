namespace Core.Entities
{
    public class Budget : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
    }
}
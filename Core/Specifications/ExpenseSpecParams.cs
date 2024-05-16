namespace Core.Specifications
{
    public class ExpenseSpecParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        private string _search;
        public int? CategoryId { get; set; }

        public int? Month { get; set; }
        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }

        public string Sort { get; set; }
    }
}
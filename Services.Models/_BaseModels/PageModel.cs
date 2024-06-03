namespace Services.Models._BaseModels
{
    public class PageModel<TModel>
    {
        public List<TModel> Items { get; set; } = new List<TModel>();
        public int TotalItems { get; set; }
        public int StartIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}

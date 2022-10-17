namespace Application.Common.Models
{
    public sealed class PaginationHeader
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public PaginationHeader(
            int currentPage, 
            int itemsPerPage, 
            int totalPages, 
            int totalItems)
        {
            this.CurrentPage = currentPage;
            this.PageSize = itemsPerPage;
            this.TotalPages = totalPages;
            this.TotalCount = totalItems;
        }
    }
}

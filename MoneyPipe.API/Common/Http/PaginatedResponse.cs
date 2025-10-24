namespace MoneyPipe.API.Common.Http
{
    public class PaginatedResponse<T> : ApiResponse<IEnumerable<T>>
    {
        public PaginationMetadata Pagination { get; set; } = default!;
    }

    public class PaginationMetadata
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    }
}

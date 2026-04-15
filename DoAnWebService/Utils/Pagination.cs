using System.Text.Json.Serialization;

namespace DoAnWebService.Utils
{
    public class Pagination
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
    }

    public class PagedResult<T>
    {
        [JsonPropertyName("items")]
        public List<T> Data { get; set; } = new List<T>();

        [JsonPropertyName("pagination")]
        public Pagination Pagination { get; set; } = new Pagination();
    }

    public static class PaginationHelper
    {
        public static PagedResult<T> CreatePagedResult<T>(
            List<T> source,
            int page,
            int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var totalCount = source.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var data = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                Data = data,
                Pagination = new Pagination
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    HasNext = page < totalPages,
                    HasPrevious = page > 1
                }
            };
        }
    }
}
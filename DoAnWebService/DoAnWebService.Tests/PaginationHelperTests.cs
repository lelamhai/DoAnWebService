using DoAnWebService.Utils;
using Xunit;

namespace DoAnWebService.Tests;

public class PaginationHelperTests
{
    [Fact]
    public void CreatePagedResult_ReturnsCorrectPageAndMetadata()
    {
        var source = Enumerable.Range(1, 25).ToList();

        var result = PaginationHelper.CreatePagedResult(source, page: 2, pageSize: 10);

        Assert.Equal(2, result.Pagination.Page);
        Assert.Equal(10, result.Pagination.PageSize);
        Assert.Equal(25, result.Pagination.TotalCount);
        Assert.Equal(3, result.Pagination.TotalPages);
        Assert.True(result.Pagination.HasNext);
        Assert.True(result.Pagination.HasPrevious);
        Assert.True(result.Data.SequenceEqual(new[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }));
    }

    [Fact]
    public void CreatePagedResult_FallsBackToDefaultValues_WhenInvalidArguments()
    {
        var source = Enumerable.Range(1, 5).ToList();

        var result = PaginationHelper.CreatePagedResult(source, page: 0, pageSize: 0);

        Assert.Equal(1, result.Pagination.Page);
        Assert.Equal(10, result.Pagination.PageSize);
        Assert.Equal(5, result.Pagination.TotalCount);
        Assert.Equal(1, result.Pagination.TotalPages);
        Assert.False(result.Pagination.HasNext);
        Assert.False(result.Pagination.HasPrevious);
        Assert.True(result.Data.SequenceEqual(source));
    }
}

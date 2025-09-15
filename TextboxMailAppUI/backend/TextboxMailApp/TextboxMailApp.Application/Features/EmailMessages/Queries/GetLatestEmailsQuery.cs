using MediatR;
using TextboxMailApp.Application.Common.Responses;

namespace TextboxMailApp.Application.Features.EmailMessages.Queries
{
  public class GetLatestEmailsQuery : IRequest<ApiResult<IEnumerable<EmailMessagesDto>>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
    public GetLatestEmailsQuery(int pageNumber, int pageSize, bool sortDesc, string? sortBy, string? searchTerm = null)
    {
      PageNumber = pageNumber;
      PageSize = pageSize;
      SearchTerm = searchTerm;
      SortBy = sortBy;
      SortDesc = sortDesc;

    }

  }
}

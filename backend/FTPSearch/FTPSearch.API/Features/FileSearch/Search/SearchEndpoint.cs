using Carter;
using FTPSearch.API.Application.Extensions;
using FTPSearch.API.Features.FileManagement.GetAllFromDirectory;
using Mapster;
using MediatR;

namespace FTPSearch.API.Features.FileSearch.Search;

public record SearchRequest(
    string SearchKeyword,
    int PageNumber,
    int PageSize);

public class SearchEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/Search",
                handler: async ([AsParameters]SearchRequest request,  ISender sender) =>
                {
                    var query = request.Adapt<SearchQuery>();
                    var result = await sender.Send(query);
                    return result.FromResult();
                }).WithName("Search Files")
            .WithTags("Search")
            .WithSummary("Search Files Flow")
            .WithDescription("Search files from the keyword.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
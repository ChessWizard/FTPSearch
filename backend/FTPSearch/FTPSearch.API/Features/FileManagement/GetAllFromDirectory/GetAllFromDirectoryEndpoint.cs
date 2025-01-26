using Carter;
using FTPSearch.API.Application.Extensions;
using Mapster;
using MediatR;

namespace FTPSearch.API.Features.FileManagement.GetAllFromDirectory;

public record GetAllFromDirectoryRequest(
    string Directory,
    int PageNumber,
    int PageSize);

public class GetAllFromDirectoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/FileManagements",
                handler: async ([AsParameters]GetAllFromDirectoryRequest request,  ISender sender) =>
                {
                    var query = request.Adapt<GetAllFromDirectoryQuery>();
                    var result = await sender.Send(query);
                    return result.FromResult();
                }).WithName("Get All Files From Directory")
            .WithTags("FileManagements")
            .WithSummary("Get All Files From Directory Flow")
            .WithDescription("Returns a list of all files from the directory.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
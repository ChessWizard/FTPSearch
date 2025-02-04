using Carter;
using FTPSearch.API.Application.Extensions;
using Mapster;
using MediatR;

namespace FTPSearch.API.Features.FileManagement.Download;

public record DownloadRequest(string FilePath);

public class DownloadEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(pattern: "/FileManagements/Download",
                handler: async (DownloadRequest request, ISender sender) =>
                {
                    var command = request.Adapt<DownloadCommand>();
                    var result = await sender.Send(command);
                    var fileName = Path.GetFileName(command.FilePath);
                    return result.FromFileResult(fileName: fileName);
                }).WithName("Download File")
            .DisableAntiforgery()
            .WithTags("FileManagements")
            .WithSummary("Download File Flow")
            .WithDescription("Download file from FTP Server.")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status200OK);
    }
}
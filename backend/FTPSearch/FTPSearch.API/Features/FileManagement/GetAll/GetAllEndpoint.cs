using Carter;
using FTPSearch.API.Application.Extensions;
using MediatR;

namespace FTPSearch.API.Features.FileManagement.GetAll;

public class GetAllEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/FileManagements",
                handler: async (ISender sender) =>
                {
                    var result = await sender.Send(new GetAllQuery());
                    return result.FromResult();
                }).WithName("Get All Files From Ftp")
            .WithTags("FileManagements")
            .WithSummary("Get All Files From Ftp Flow")
            .WithDescription("Returns a list of all files from the FTP server.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
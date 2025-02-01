using Carter;
using FTPSearch.API.Application.Extensions;
using FTPSearch.API.Domain.Enums;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FTPSearch.API.Features.FileManagement.RemoveFtpAndDatabase;

public record RemoveFtpAndDatabaseRequest(Guid Id,
    FileMetaType FileMetaType);

public class RemoveFtpAndDatabaseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(pattern: "/FileManagements",
                handler: async ([FromBody]RemoveFtpAndDatabaseRequest request, ISender sender) =>
                {
                    var command = request.Adapt<RemoveFtpAndDatabaseCommand>();
                    var result = await sender.Send(command);
                    return result.FromResult();
                }).WithName("Remove Files from Ftp and Database")
            .WithTags("FileManagements")
            .WithSummary("Remove Files from Ftp and Database Flow")
            .WithDescription("Remove files from FTP and soft delete on database.")
            .Produces(StatusCodes.Status200OK);
    }
}
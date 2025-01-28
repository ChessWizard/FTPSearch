using Carter;
using FTPSearch.API.Application.Extensions;
using FTPSearch.API.Domain.Enums;
using FTPSearch.API.Features.FileManagement.TransferFromFtpToDatabase;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FTPSearch.API.Features.FileManagement.AddFtpAndDatabase;

public record AddFtpAndDatabaseRequest(
    string Directory,
    FileMetaType FileMetaType);

public class AddFtpAndDatabaseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(pattern: "/FileManagements/Upload",
                handler: async ([FromForm]AddFtpAndDatabaseRequest request, IFormFileCollection? files, ISender sender) =>
                {
                    AddFtpAndDatabaseCommand command = new(files, request.Directory, request.FileMetaType);
                    var result = await sender.Send(command);
                    return result.FromResult();
                }).WithName("Add Files to Ftp and Database")
            .DisableAntiforgery()
            .WithTags("FileManagements")
            .WithSummary("Add Files to Ftp and Database Flow")
            .WithDescription("Add files to FTP and save details into a database.")
            .Produces(StatusCodes.Status200OK);
    }
}
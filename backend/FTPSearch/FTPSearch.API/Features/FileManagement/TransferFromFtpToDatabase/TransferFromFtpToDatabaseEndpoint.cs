using Carter;
using FTPSearch.API.Application.Extensions;
using MediatR;

namespace FTPSearch.API.Features.FileManagement.TransferFromFtpToDatabase;
public record GetAllFromDirectoryRequest(int SiteId);
public class TransferFromFtpToDatabaseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(pattern: "/FileManagements",
                handler: async (GetAllFromDirectoryRequest request, ISender sender) =>
                {
                    
                    var result = await sender.Send(new TransferFromFtpToDatabaseCommand(request.SiteId));
                    return result.FromResult();
                }).WithName("Transfer Files From Ftp to Database")
            .WithTags("FileManagements")
            .WithSummary("Transfer Files From Ftp to Database Flow")
            .WithDescription("Retrieve FTP files and save details into a database.")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status200OK);
    }
}
namespace FTPSearch.API.Application.Results.Messages.Common;

public record BusinessMessage(string Message,
    string Code,
    int HttpStatus);
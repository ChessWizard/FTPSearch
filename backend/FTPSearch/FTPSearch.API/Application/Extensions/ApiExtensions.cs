using FTPSearch.API.Application.Results;

namespace FTPSearch.API.Application.Extensions;

public static class ApiExtensions
{
    public static IResult FromResult<T>(this BaseResult<T> result)
        => Microsoft.AspNetCore.Http.Results.Json(result, statusCode: result.Message.HttpStatus);
}
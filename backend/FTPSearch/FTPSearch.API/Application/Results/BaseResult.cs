using System.Text.Json.Serialization;
using FTPSearch.API.Application.Results.Messages.Common;

namespace FTPSearch.API.Application.Results;

public class BaseResult<TData>
{
    public TData Data { get; set; }

    public BusinessMessage Message { get; set; }
    
    public ErrorResult? ErrorDto { get; set; }
    
    [JsonIgnore] public bool IsSuccessful { get; set; }
}
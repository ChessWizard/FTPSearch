using FTPSearch.API.Application.Results.Messages.Common;

namespace FTPSearch.API.Application.Results;

public class Result<TData> : BaseResult<TData>
{
    public static Result<TData> Success(TData data, BusinessMessage message)
    {
        return new Result<TData> { Data = data, Message = message , IsSuccessful = true };
    }

    public static Result<TData> Success(BusinessMessage message)
    {
        return new Result<TData> { Message = message, IsSuccessful = true };
    }

    public static Result<TData> Error(ErrorResult errorDto, BusinessMessage message)
    {
        return new Result<TData> { ErrorDto = errorDto, Message = message , IsSuccessful = false };
    }

    public static Result<TData> Error(BusinessMessage message, bool isShow = true)
    {
        return new Result<TData>
        {
            Data = default,
            ErrorDto = new ErrorResult(message.Message, isShow),
            Message = message,
            IsSuccessful = false
        };
    }
}
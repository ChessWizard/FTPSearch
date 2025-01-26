using FTPSearch.API.Application.Results.Messages.Common;

namespace FTPSearch.API.Application.Results.Paging;

public class PagingResult<TData> : BaseResult<TData>
{
    public PagingMetaData PagingMetaData { get; set; }

    public static PagingResult<TData> Success(TData data, PagingMetaData pagingMetaData, BusinessMessage message)
    {
        return new PagingResult<TData> { Data = data, PagingMetaData = pagingMetaData, Message = message, IsSuccessful = true };
    }

    public static PagingResult<TData> Error(ErrorResult errorDto, BusinessMessage message)
    {
        return new PagingResult<TData> { ErrorDto = errorDto, Message = message , IsSuccessful = false };
    }

    public static PagingResult<TData> Error(BusinessMessage message, bool isShow = true)
    {
        return new PagingResult<TData>
        {
            Data = default,
            PagingMetaData = default,
            ErrorDto = new ErrorResult(message.Message, isShow),
            Message = message,
            IsSuccessful = false
        };
    }
}
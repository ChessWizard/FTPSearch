using FTPSearch.API.Domain.Enums;

namespace FTPSearch.API.Application.Response;

public record FileResponse(string Name,
    string Path,
    string Url,
    FileMetaType FileMetaType);
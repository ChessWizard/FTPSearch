namespace FTPSearch.API.Domain.Entities.Base.Interfaces;

public interface ISoftDeleteEntity
{
    bool IsDeleted { get; set; }
}
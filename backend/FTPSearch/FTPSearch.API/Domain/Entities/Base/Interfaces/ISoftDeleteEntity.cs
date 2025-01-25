namespace FTPSearch.API.Domain.Entities.Base.Interfaces;

public interface ISoftDeleteEntity : IDeletedOn
{
    bool IsDeleted { get; set; }
}
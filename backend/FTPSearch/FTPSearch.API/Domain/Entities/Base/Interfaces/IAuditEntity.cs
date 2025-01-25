namespace FTPSearch.API.Domain.Entities.Base.Interfaces;

public interface IAuditEntity : IEntity, ICreatedOn, IModifiedOn, IDeletedOn
{
}


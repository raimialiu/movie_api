using MovieSvc.Domain.Interface;

namespace MovieSvc.Application.Interface;

public interface IDomainEventDispatcher
{
    Task Dispatch(IDomainEvent domainEvent);
}
using System.Collections.Concurrent;

namespace MovieSvc.Domain.Interface;

public interface IEntity
{
    IProducerConsumerCollection<IDomainEvent> DomainEvents { get; }
}
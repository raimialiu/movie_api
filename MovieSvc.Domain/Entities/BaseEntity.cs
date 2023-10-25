using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using MovieSvc.Domain.Interface;

namespace MovieSvc.Domain.Entities;

public class BaseEntity : IEntity
{
    [NotMapped]
    private readonly ConcurrentQueue<IDomainEvent> _domainEvents = new ConcurrentQueue<IDomainEvent>();

    [NotMapped]
    public IProducerConsumerCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected void PublishEvent(IDomainEvent @event)
    {
        _domainEvents.Enqueue(@event);
    }
}


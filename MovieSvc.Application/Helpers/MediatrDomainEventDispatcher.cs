using MediatR;
using MovieSvc.Application.Interface;
using MovieSvc.Domain.Interface;

namespace MovieSvc.Application.Helpers;

public class MediatrDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public MediatrDomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Dispatch(IDomainEvent domainEvent)
    {
        var domainEventNotification = _createDomainEventNotification(domainEvent);
        await _mediator.Publish(domainEventNotification);
    }

    private INotification _createDomainEventNotification(IDomainEvent domainEvent)
    {
        var genericDispatcherType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
        return (INotification)Activator.CreateInstance(genericDispatcherType, domainEvent);
    }
}

public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}
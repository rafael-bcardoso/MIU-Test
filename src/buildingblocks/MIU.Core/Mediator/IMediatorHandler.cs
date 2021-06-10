using FluentValidation.Results;
using MIU.Core.Messages;
using System.Threading.Tasks;

namespace MIU.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublisherEvent<T>(T eventMessage) where T : Event;

        Task<ValidationResult> PublisherCommand<T>(T command) where T : Command;
    }
}

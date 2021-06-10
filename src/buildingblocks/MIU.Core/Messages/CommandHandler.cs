using FluentValidation.Results;
using MIU.Core.Data;
using System.Threading.Tasks;

namespace MIU.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        public CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }

        protected async Task<ValidationResult> SaveData(IUnitOfWork uow)
        {
            var commited = await uow.Commit();
            if (!commited)
                AddError("Houve um erro ao persistir os dados");

            return ValidationResult;
        }
    }
}

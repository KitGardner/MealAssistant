using System;
using System.Threading.Tasks;

namespace MealAssistant.Data
{
    public interface ITransactionManager
    {
        Task ExecuteInTransactionWithSaveAsync(Func<Task> work);
    }
}


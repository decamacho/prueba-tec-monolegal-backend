using System;

namespace Application.Interfaces
{
    public interface IAppLogger<T>
    {
        void LogError(Exception exception, string message, params object[] args);
        void LogInformation(string message, params object[] args);
    }
}

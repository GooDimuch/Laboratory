using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure
{
    public class Application
    {
        public readonly ApplicationStateMachine StateMachine;

        public Application(ServiceRegister services)
        {
            StateMachine = new ApplicationStateMachine(services);
        }
    }
}
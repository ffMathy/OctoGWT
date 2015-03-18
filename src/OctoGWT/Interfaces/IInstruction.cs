using OctoGWT.Facades;

namespace OctoGWT.Interfaces
{
    public interface IInstruction<TDriverFacade>
    {
        void Run(TDriverFacade o);
    }
}

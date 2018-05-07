

namespace CommonLevelEditor
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
    
}
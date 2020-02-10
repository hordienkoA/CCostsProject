namespace CCostsProject.Models
{
    public interface ITransactionManager
    {
        void execute(string username ,double money);
        void undo(string username ,double money);
    }
}
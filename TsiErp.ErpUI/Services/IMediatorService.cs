namespace TsiErp.ErpUI.Services
{
    public interface IMediatorService
    {
        Task Publish(string message);
        Task Subscribe(Func<string, Task> action);
    }
}

namespace TsiErp.ErpUI.Services
{
    public class MediatorService : IMediatorService
    {
        private Func<string, Task> subscriber;

        public async Task Publish(string message)
        {
            if (subscriber != null)
            {
                await subscriber.Invoke(message);
            }
        }

        public Task Subscribe(Func<string, Task> action)
        {
            subscriber = action;
            return Task.CompletedTask;
        }
    }
}

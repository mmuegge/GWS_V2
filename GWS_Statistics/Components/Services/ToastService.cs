namespace GWS_Statistics.Components.Services
{
    public class ToastService
    {
        public event Action<ToastOption>? ToastInstance;

        public void Open(ToastOption options)
        {
            // Invoke ToastComponent to update and show the toast with messages
            ToastInstance?.Invoke(options);
        }
    }
}

namespace BookstoreGraphQL.Blazor.Services
{
    public class ToastService
    {
        public event Action<string, string, string> OnShow;

        public void ShowToast(string message, string type = "success", string title = "Notification")
        {
            OnShow?.Invoke(message, type, title);
        }
    }
}

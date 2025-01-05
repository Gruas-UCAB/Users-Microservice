namespace UsersMicroservice.core.Application
{
    public interface IEmailSender<T>
    {
        Task SendEmail(string to, T data);
    }
}

namespace UsersMicroservice.core.Application
{
    public record EmailContent
    (
        string to,
        string subject,
        string body
    );
}

namespace UsersMicroservice.core.Application
{
    public interface ICodeGenerator<T>
    {
        T GenerateRandomCode();
    }
}

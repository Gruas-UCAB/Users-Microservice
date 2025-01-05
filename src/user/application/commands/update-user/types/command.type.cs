namespace UsersMicroservice.src.user.application.commands.update_user.types
{
    public class UpdateUserByIdCommand(string Id, string? Name, string? Phone)
    {
        public string Id = Id;
        public string? Name = Name;
        public string? Phone = Phone;
    }
}

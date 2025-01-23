using System.Text;

namespace UsersMicroservice.core.Common
{
    public class RandomPasswordGenerator
    {
        public static string GenerateRandomPassword()
        {
            var uuid = Guid.NewGuid().ToString("N");
            return uuid.Substring(0, 8);
        }
    }
}

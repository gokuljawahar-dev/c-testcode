namespace LXP.Common.Utils;

using System.Text;

public class RandomPassword
{
    public static string Randompasswordgenerator()
    {
        var random = new Random();
        //int passwordlenth = random.Next(6, 10);
        const string Validcharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var password = new StringBuilder();
        for (var i = 0; i < 6; i++)
        {
            var randomcode = random.Next(0, Validcharacters.Length);
            password.Append(Validcharacters[randomcode]);
        }
        return password.ToString();
    }
}

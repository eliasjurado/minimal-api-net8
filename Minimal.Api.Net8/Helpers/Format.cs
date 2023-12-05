using System.Text;

namespace Infrastructure.Minimal.Api.Net8
{
    public static class Format
    {
        public static string GetName(string name)
        {
            var sb = new StringBuilder();
            sb.Append(name.First().ToString().ToUpper());
            name.Substring(1).ToList().ForEach(x => sb.Append(char.IsUpper(x) ? $" {x}" : x.ToString()));
            return sb.ToString();
        }
    }
}

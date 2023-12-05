
namespace Minimal.Api.Net8.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public char IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

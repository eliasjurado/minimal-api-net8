using Infrastructure.Minimal.Api.Net8;
using System.Net;

namespace Minimal.Api.Net8.Models
{
    public class APIResponse<T>
    {
        public bool IsSuccess { get; set; }
        public Guid CorrelationId { get; set; }
        public T Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
        public ICollection<string> Errors { get; set; }

        public APIResponse()
        {
            CorrelationId = Guid.NewGuid();
            IsSuccess = false;
            StatusCode = HttpStatusCode.BadRequest;
            Status = Format.GetName(nameof(HttpStatusCode.BadRequest));
            Errors = new List<string>();
        }

    }
}

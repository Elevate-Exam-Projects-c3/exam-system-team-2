using System.Security.Claims;

namespace exam_system.Features.Shared.CurrentUser
{
    public class CurrentUserId:ICurrentUserId
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserId(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid? GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(Guid.TryParse(userId, out var id))
            {
                return id;
            }
            return null;
        }
        public Guid? GetStudentId()
        {
            var studentIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("StudentId");
            if (studentIdClaim != null && Guid.TryParse(studentIdClaim.Value, out var studentId))
            {
                return studentId;
            }
            return null;
        }
    }
}

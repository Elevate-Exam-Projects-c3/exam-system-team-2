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
    }
}

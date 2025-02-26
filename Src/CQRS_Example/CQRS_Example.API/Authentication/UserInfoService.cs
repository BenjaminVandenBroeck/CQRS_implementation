using CQRS_Example.Common;

namespace CQRS_Example.API.Authentication
{
    public class UserInfoService: IInfoService
    {

        private readonly IHttpContextAccessor _contextAccessor;

        public UserInfoService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Name
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
            }
        }

    }
}

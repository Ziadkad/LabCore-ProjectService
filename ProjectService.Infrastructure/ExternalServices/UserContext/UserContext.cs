using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;

namespace ProjectService.Infrastructure.ExternalServices.UserContext;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{    
    public Guid GetCurrentUserId()
    {
        var userIdStr = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            throw new UnauthorizedAccessException("Invalid or missing user ID");

        return userId;
    }

    public UserRole GetUserRole()
    {
        var roleStr = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrWhiteSpace(roleStr) || !Enum.TryParse<UserRole>(roleStr, true, out var role))
            throw new UnauthorizedAccessException("Invalid or missing user role");

        return role;
    }
    
}
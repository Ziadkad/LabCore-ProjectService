using ProjectService.Application.Common.Models;

namespace ProjectService.Application.Common.Interfaces;

public interface IUserContext
{
    Guid GetCurrentUserId();
    UserRole GetUserRole();
}
namespace Modulio.Application.Abstractions.Services
{
    /// <summary>
    /// Defines the contract for a service that provides information about the current user
    /// interacting with the application. Essential for auditing, authorization, etc.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the unique identifier of the current user.
        /// Returns null if the user is not authenticated.
        /// </summary>
        int? UserId { get; }

        /// <summary>
        /// Gets the username or name identifier of the current user.
        /// Returns null if the user is not authenticated or username is unavailable.
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Gets a value indicating whether the current user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets the IP address of the current user.
        /// </summary>
        string? IpAddress { get; }

        /// <summary>
        /// Gets the roles of the current user.
        /// </summary>
        IEnumerable<string> Roles { get; }

        /// <summary>
        /// Gets the permissions of the current user.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        bool IsInRole(string roleName);
    }
}

using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// 创建用户
        /// </summary>
        Task<User> CreateUserAsync(User user);

        /// <summary>
        /// 更新用户
        /// </summary>
        Task<bool> UpdateUserAsync(int id, User user);

        /// <summary>
        /// 删除用户
        /// </summary>
        Task<bool> DeleteUserAsync(int id);

        /// <summary>
        /// 检查邮箱是否已存在
        /// </summary>
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
    }
}

using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    /// <summary>
    /// 用户服务实现类
    /// </summary>
    public class UserService : IUserService
    {
        private static List<User> _users = new List<User>
        {
            new User 
            { 
                Id = 1, 
                Name = "Alice Smith", 
                Email = "alice@techhive.com",
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                UpdatedAt = DateTime.UtcNow.AddDays(-1),
                IsActive = true
            },
            new User 
            { 
                Id = 2, 
                Name = "Bob Johnson", 
                Email = "bob@techhive.com",
                CreatedAt = DateTime.UtcNow.AddDays(-20),
                UpdatedAt = DateTime.UtcNow.AddDays(-2),
                IsActive = true
            },
            new User 
            { 
                Id = 3, 
                Name = "Charlie Brown", 
                Email = "charlie@techhive.com",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-3),
                IsActive = false
            }
        };

        private static int _nextId = 4;

        /// <summary>
        /// 获取所有用户
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            await Task.Delay(10); // 模拟异步操作
            return _users.OrderBy(u => u.Id);
        }

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            await Task.Delay(10); // 模拟异步操作
            return _users.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        public async Task<User> CreateUserAsync(User user)
        {
            await Task.Delay(10); // 模拟异步操作
            
            user.Id = _nextId++;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            
            _users.Add(user);
            return user;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            await Task.Delay(10); // 模拟异步操作
            
            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.IsActive = user.IsActive;
            existingUser.UpdatedAt = DateTime.UtcNow;

            return true;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        public async Task<bool> DeleteUserAsync(int id)
        {
            await Task.Delay(10); // 模拟异步操作
            
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }

            _users.Remove(user);
            return true;
        }

        /// <summary>
        /// 检查邮箱是否已存在
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            await Task.Delay(10); // 模拟异步操作
            
            return _users.Any(u => 
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
                (excludeUserId == null || u.Id != excludeUserId));
        }
    }
}

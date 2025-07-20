using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using UserManagementAPI.DTOs;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    /// <summary>
    /// 用户管理控制器
    /// 提供用户的CRUD操作功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userService">用户服务</param>
        /// <param name="logger">日志记录器</param>
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <returns>用户列表</returns>
        /// <response code="200">成功返回用户列表</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            _logger.LogInformation("开始获取所有用户列表");

            var users = await _userService.GetAllUsersAsync();
            var userDtos = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                IsActive = u.IsActive
            });

            _logger.LogInformation("成功获取用户列表，共 {Count} 个用户", userDtos.Count());
            return Ok(userDtos);
        }

        /// <summary>
        /// 根据ID获取特定用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户信息</returns>
        /// <response code="200">成功返回用户信息</response>
        /// <response code="404">用户不存在</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"用户 ID {id} 不存在");
            }

            var userDto = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsActive = user.IsActive
            };

            return Ok(userDto);
        }

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="createUserDto">创建用户请求</param>
        /// <returns>创建的用户信息</returns>
        /// <response code="201">用户创建成功</response>
        /// <response code="400">请求数据无效</response>
        /// <response code="409">邮箱已存在</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            _logger.LogInformation("尝试创建新用户: {Email}", createUserDto.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("创建用户请求数据验证失败: {Email}", createUserDto.Email);
                return BadRequest(ModelState);
            }

            // 检查邮箱是否已存在
            if (await _userService.EmailExistsAsync(createUserDto.Email))
            {
                _logger.LogWarning("尝试创建用户失败，邮箱已存在: {Email}", createUserDto.Email);
                return Conflict($"邮箱 '{createUserDto.Email}' 已被使用");
            }

            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                IsActive = true
            };

            var createdUser = await _userService.CreateUserAsync(user);
            _logger.LogInformation("成功创建用户: {Email}, ID: {UserId}", createdUser.Email, createdUser.Id);

            var userDto = new UserResponseDto
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email,
                CreatedAt = createdUser.CreatedAt,
                UpdatedAt = createdUser.UpdatedAt,
                IsActive = createdUser.IsActive
            };

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, userDto);
        }

        /// <summary>
        /// 更新现有用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="updateUserDto">更新用户请求</param>
        /// <returns>更新结果</returns>
        /// <response code="200">用户更新成功</response>
        /// <response code="400">请求数据无效</response>
        /// <response code="404">用户不存在</response>
        /// <response code="409">邮箱已存在</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 检查用户是否存在
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound($"用户 ID {id} 不存在");
            }

            // 检查邮箱是否已被其他用户使用
            if (await _userService.EmailExistsAsync(updateUserDto.Email, id))
            {
                return Conflict($"邮箱 '{updateUserDto.Email}' 已被其他用户使用");
            }

            var user = new User
            {
                Name = updateUserDto.Name,
                Email = updateUserDto.Email,
                IsActive = updateUserDto.IsActive
            };

            var success = await _userService.UpdateUserAsync(id, user);
            if (!success)
            {
                return NotFound($"用户 ID {id} 不存在");
            }

            // 返回更新后的用户信息
            var updatedUser = await _userService.GetUserByIdAsync(id);
            var userDto = new UserResponseDto
            {
                Id = updatedUser!.Id,
                Name = updatedUser.Name,
                Email = updatedUser.Email,
                CreatedAt = updatedUser.CreatedAt,
                UpdatedAt = updatedUser.UpdatedAt,
                IsActive = updatedUser.IsActive
            };

            return Ok(userDto);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>删除结果</returns>
        /// <response code="204">用户删除成功</response>
        /// <response code="404">用户不存在</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("尝试删除用户: {UserId}", id);

            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                _logger.LogWarning("删除用户失败，用户不存在: {UserId}", id);
                return NotFound($"用户 ID {id} 不存在");
            }

            _logger.LogInformation("成功删除用户: {UserId}", id);
            return NoContent();
        }

        /// <summary>
        /// 获取活跃用户列表
        /// </summary>
        /// <returns>活跃用户列表</returns>
        /// <response code="200">成功返回活跃用户列表</response>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetActiveUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var activeUsers = users.Where(u => u.IsActive).Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                IsActive = u.IsActive
            });

            return Ok(activeUsers);
        }

        /// <summary>
        /// 测试异常处理中间件的端点
        /// </summary>
        /// <returns>抛出异常用于测试</returns>
        /// <response code="500">内部服务器错误</response>
        [HttpGet("test-exception")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TestException()
        {
            // 模拟一个未处理的异常，用于测试全局异常处理中间件
            await Task.Delay(1); // 模拟异步操作
            throw new InvalidOperationException("这是一个测试异常，用于验证全局异常处理中间件");
        }
    }
}
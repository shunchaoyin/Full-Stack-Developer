using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    /// <summary>
    /// 用户实体模型
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户唯一标识符
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required(ErrorMessage = "用户姓名是必需的")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "用户姓名长度必须在2-100个字符之间")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 用户邮箱
        /// </summary>
        [Required(ErrorMessage = "邮箱地址是必需的")]
        [EmailAddress(ErrorMessage = "请输入有效的邮箱地址")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 用户创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 用户最后更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 用户是否激活
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}

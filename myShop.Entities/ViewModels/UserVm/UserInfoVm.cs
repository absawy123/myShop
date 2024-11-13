namespace myShop.Entities.ViewModels.UserVm
{
    public class UserInfoVm
    {
        public string userId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

    }
}

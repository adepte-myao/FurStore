using System.ComponentModel.DataAnnotations;

namespace FurStore.ViewModels.Auth
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}

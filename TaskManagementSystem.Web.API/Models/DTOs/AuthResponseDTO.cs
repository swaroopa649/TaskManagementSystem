namespace TaskManagementSystem.Web.API.Models.DTOs
{
    public class AuthResponseDTO
    {
        public string Message { get; set; }
        public bool IsValidUser { get; set; }
        public bool IsValidPassword { get; set; }
        public string JwtToken { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}

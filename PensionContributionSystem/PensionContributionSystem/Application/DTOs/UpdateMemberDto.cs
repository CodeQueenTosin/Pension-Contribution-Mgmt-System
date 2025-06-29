namespace PensionContributionSystem.Application.DTOs
{
    public class UpdateMemberDto
    {
        public Guid MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }

 
}

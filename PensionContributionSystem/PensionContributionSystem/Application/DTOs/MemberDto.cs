namespace PensionContributionSystem.Application.DTOs
{
    public class MemberDto
    {
        public Guid MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        public MemberDto(string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber)
        {
                FirstName = firstName;
                LastName = lastName;
                DateOfBirth = dateOfBirth;
                Email = email;
                PhoneNumber = phoneNumber;
            
        }
    }

 
}

namespace PensionContributionSystem.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
        public ICollection<Benefit> Benefits { get; set; } = new List<Benefit>();


    }
}

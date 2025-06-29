namespace PensionContributionSystem.Domain.Entities
{
   public class Employer
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string RegistrationNumber { get; set; }
        public bool IsActive { get; set; }
    }

}

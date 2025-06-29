namespace PensionContributionSystem.Application.DTOs
{
    public class ContributionDto
    {
        public Guid MemberId { get; set; }
        public string ContributionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ContributionDate { get; set; }
        public string ReferenceNumber { get; set; }
    }

}

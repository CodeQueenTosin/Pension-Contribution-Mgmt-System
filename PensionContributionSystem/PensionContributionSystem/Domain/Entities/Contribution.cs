using static PensionContributionSystem.Domain.Enum.ContributorTypeEnum;

namespace PensionContributionSystem.Domain.Entities
{
    public class Contribution
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public Member Member { get; set; }
        public string ContributionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ContributionDate { get; set; }
        public string ReferenceNumber { get; set; }
    }
}

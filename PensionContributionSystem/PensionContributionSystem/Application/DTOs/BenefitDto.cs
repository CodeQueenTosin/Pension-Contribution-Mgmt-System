namespace PensionContributionSystem.Application.DTOs
{
    public class BenefitDto
    {
        public Guid MemberId { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal BenefitAmount { get; set; }
        public bool Eligible { get; set; }
    }

}

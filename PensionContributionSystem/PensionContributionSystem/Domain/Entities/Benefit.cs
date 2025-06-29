namespace PensionContributionSystem.Domain.Entities
{
    public class Benefit
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public string BenefitType { get; set; }
        public DateTime CalculationDate { get; set; }
        public bool EligibilityStatus { get; set; }
        public decimal Amount { get; set; }
    }
}

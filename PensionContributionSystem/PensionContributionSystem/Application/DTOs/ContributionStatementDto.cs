namespace PensionContributionSystem.Application.DTOs
{
    public class ContributionStatementDto
    {
        public Guid MemberId { get; set; }
        public decimal MandatoryTotal { get; set; }
        public decimal VoluntaryTotal { get; set; }
        public List<ContributionDto> Contributions { get; set; } = new();
    }
}

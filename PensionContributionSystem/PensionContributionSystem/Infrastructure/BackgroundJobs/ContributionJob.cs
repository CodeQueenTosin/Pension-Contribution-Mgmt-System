using PensionContributionSystem.Application.Interfaces;
using PensionContributionSystem.Infrastructure.Repositories;

namespace PensionContributionSystem.Infrastructure.BackgroundJobs
{
    public class ContributionJob
    {
        private readonly IContributionService _contributionService;
        private readonly IMemberRepository _memberRepository;

        public ContributionJob(IContributionService contributionService, IMemberRepository memberRepository)
        {
            _contributionService = contributionService;
            _memberRepository = memberRepository;
        }
    }

}

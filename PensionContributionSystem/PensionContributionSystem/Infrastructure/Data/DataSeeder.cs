using PensionContributionSystem.Domain.Entities;


namespace PensionContributionSystem.Infrastructure.Data
{
    public class DataSeeder
    {
        public static async Task SeedAsync(AppDBContext context)
        {
            // Check if database is empty
            if (!context.Employers.Any())
            {
                // Seed Employers
                var employer = new Employer
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Sample Corp",
                    RegistrationNumber = "REG123456",
                    IsActive = true
                };
                context.Employers.Add(employer);

                // Seed Members
                var member = new Member
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Tosin",
                    LastName = "Olorunnisola",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Email = "tosin@example.com",
                    PhoneNumber = "08065564416",
                    IsDeleted = false
                };
                context.Members.Add(member);

                // Seed Contributions
                var contribution = new Contribution
                {
                    Id = Guid.NewGuid(),
                    MemberId = member.Id,
                    ContributionType = "Monthly",
                    Amount = 1000.00m,
                    ContributionDate = DateTime.UtcNow,
                    ReferenceNumber = "CONTRIB-" + Guid.NewGuid().ToString().Substring(0, 8)
                };
                context.Contributions.Add(contribution);

                // Seed Benefits
                var benefit = new Benefit
                {
                    Id = Guid.NewGuid(),
                    MemberId = member.Id,
                    BenefitType = "Retirement",
                    CalculationDate = DateTime.UtcNow,
                    EligibilityStatus = true,
                    Amount = 5000.00m
                };
                context.Benefits.Add(benefit);

                await context.SaveChangesAsync();
            }
        }
    }

}


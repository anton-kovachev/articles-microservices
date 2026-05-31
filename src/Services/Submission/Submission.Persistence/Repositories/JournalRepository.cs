namespace Submission.Persistence.Repositories;

public class JournalRepository(SubmissionDbContext dbContext) :
    Repository<Domain.Entities.Journal>(dbContext);

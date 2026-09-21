using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Orchestrators
{
    public class DeleteQuizOrchestratorHandler : IRequestHandler<DeleteQuizOrchestrator, RequestResponse<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<Quiz> quizRepository;

        public DeleteQuizOrchestratorHandler(IUnitOfWork unitOfWork, IGenericRepository<Quiz> quizRepository)
        {
            this.unitOfWork = unitOfWork;
            this.quizRepository = quizRepository;
        }

        public async Task<RequestResponse<bool>> Handle(DeleteQuizOrchestrator request, CancellationToken cancellationToken)
        {
            var GetQuiz = await quizRepository.GetByIdAsync(request.QuizId);
            if (GetQuiz is null)
            {
                return RequestResponse<bool>.Fail("The Exam is not found", 404);
            }

            if (GetQuiz.Status == QuizStatus.Published)
            {
                return RequestResponse<bool>.Fail("The Exam is published, you can't delete it. You must unpublish it first then delete it", 400);
            }

            GetQuiz.IsDeleted = true;
            GetQuiz.DeletedAt = DateTime.UtcNow;

            var result = await unitOfWork.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return RequestResponse<bool>.Ok(true, "The Exam has been deleted successfully");
            }
            else
            {
                return RequestResponse<bool>.Fail("Failed to delete the Exam", 500);
            }

        }
    }
}

using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Orchestrators
{
    public class DeleteQuizOrchestratorHandler : IRequestHandler<DeleteQuizOrchestrator, ApiResponse<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<Quiz> quizRepository;

        public DeleteQuizOrchestratorHandler(IUnitOfWork unitOfWork, IGenericRepository<Quiz> quizRepository)
        {
            this.unitOfWork = unitOfWork;
            this.quizRepository = quizRepository;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteQuizOrchestrator request, CancellationToken cancellationToken)
        {
            var GetQuiz = await quizRepository.GetByIdAsync(request.QuizId);
            if(GetQuiz is null)
            {
                return ApiResponse<bool>.Fail("The Exam is not found", 404);
            }

            GetQuiz.UnPublichArchived();
        }
    }
}

using exam_system.Domain.Entities.Quizzes;
using exam_system.Persistence.DataAccess;
using FluentValidation;
using MediatR;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Mediator
{
    public record AddQuizCommand(string Title , int DurationMinutes , int PassScore, int Score, DateTime StartDate, DateTime EndDate) : IRequest<bool>;

    internal class AddQuizCommandValidator : AbstractValidator<AddQuizCommand>
    {
        public AddQuizCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.DurationMinutes).GreaterThan(0).WithMessage("Duration must be greater than 0.");
            RuleFor(x => x.PassScore).GreaterThanOrEqualTo(60).WithMessage("Pass score must be greater than or equal to 60.");
            RuleFor(x => x.Score).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).WithMessage("Score must be greater than or equal to 0 and less than or equal to 100.");
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate).WithMessage("Start date must be before end date.");
        }
    }

    internal class AddQuizCommandHandler : IRequestHandler<AddQuizCommand , bool>
    {
        private readonly IGenericRepository<Quiz> _quizRepository ;
        private readonly IUnitOfWork _unitOfWork ;
        public Task<bool> Handle(AddQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = new Quiz
            {
                Title = request.Title,
                DurationMinutes = request.DurationMinutes,
                PassScore = request.PassScore,
                Score = request.Score,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
            
            _quizRepository.Add(quiz);

            _unitOfWork.SaveChangesAsync(cancellationToken);
            return Task.FromResult(true);
        }
    }
}

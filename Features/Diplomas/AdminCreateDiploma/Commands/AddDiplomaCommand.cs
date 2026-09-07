using MediatR;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Commands
{
    public record AddDiplomaCommand(string Title, string? Description, string? ImageUrl) : IRequest<Unit>
    {
    }
}

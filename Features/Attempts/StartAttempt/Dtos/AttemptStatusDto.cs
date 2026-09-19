using exam_system.Common.Enums;

namespace exam_system.Features.Attempts.StartAttempt.Dtos
{
    public class AttemptStatusDto
    {
        public Guid AttemptId { get; set; }
        public Guid StudentId { get; set; }
        public AttemptStatus Status { get; set; }
    }
}

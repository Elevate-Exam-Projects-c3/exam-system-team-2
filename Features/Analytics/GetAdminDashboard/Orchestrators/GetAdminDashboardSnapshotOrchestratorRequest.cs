using exam_system.Features.Analytics.GetAdminDashboard.ViewModels;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Analytics.GetAdminDashboard.Orchestrators;

public record GetAdminDashboardSnapshotOrchestratorRequest : IRequest<RequestResponse<AdminDashboardSnapshotViewModel>>;

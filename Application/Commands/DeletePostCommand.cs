using MediatR;

namespace Application.Commands;

public record DeletePostCommand(Guid Id, Guid UserId) : IRequest;
using AmarEServir.Core.Results.Base;
using MediatR;

namespace Auth.API.Application.Users.UpdateUser
{

    public interface IUpdateUserComandHandler : IRequestHandler<UpdateUserCommand, Result>
    { }
    public class UpdateUserCommandHandler : IUpdateUserComandHandler
    {
        public Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

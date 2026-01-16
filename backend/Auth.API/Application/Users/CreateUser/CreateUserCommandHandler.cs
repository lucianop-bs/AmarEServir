using AmarEServir.Core.Results.Base;
using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using Auth.API.Infrastructure.Persistence.Context;
using MediatR;

namespace Auth.API.Application.Users.CreateUser
{

    public record class CreatedUserResponse(Guid Id, string Message = "Usuário criado com sucesso!");

    public interface ICreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreatedUserResponse>>
    {
    }
    public class CreateUserCommandHandler : ICreateUserCommandHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly ICellRepository _cellRepository;
        private readonly MongoContext _context;

        public CreateUserCommandHandler(IUserRepository userRepository, MongoContext context, ICellRepository cellRepository)
        {
            _userRepository = userRepository;
            _context = context;
            _cellRepository = cellRepository;
        }
        public async Task<Result<CreatedUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new CreatedUserResponse(Guid.NewGuid());
            using var session = await _context.Client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                var cellId = Guid.NewGuid();
                var userId = Guid.NewGuid();

                var user = request.ToDomain(cellId, userId);
                var cell = new Cell(request.CellName!, leaderId: user.Id, membro: user, id: cellId);

                var userResult = user.Validate();
                var cellResult = cell.Validate();

                if (!userResult.IsSuccess || !cellResult.IsSuccess)
                {

                    return Result<CreatedUserResponse>.Fail(UserError.InvalidForRole);
                }
                await _userRepository.Create(session, user);

                await _cellRepository.Create(session, cell);

                await session.CommitTransactionAsync();

                response = new CreatedUserResponse(user.Id, "Usuário e célula criados com sucesso!");

                return Result<CreatedUserResponse>.Ok(response);
            }
            catch
            {
                await session.AbortTransactionAsync();
                return Result<CreatedUserResponse>.Fail(UserError.InvalidForRole);

            }
        }
    }
}

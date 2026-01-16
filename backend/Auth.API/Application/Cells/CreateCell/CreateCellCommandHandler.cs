using AmarEServir.Core.Results.Base;
using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using MediatR;

namespace Auth.API.Application.Cells.CreateCell
{
    public record CreatedCellResponse(Guid Id, string Message = "Célula criada com sucesso!");
    public class CreateCellCommandHandler : IRequestHandler<CreateCellCommand, Result<CreatedCellResponse>>
    {
        private readonly ICellRepository _cellRepository;

        public CreateCellCommandHandler(ICellRepository cellRepository)
        {
            _cellRepository = cellRepository;
        }

        public async Task<Result<CreatedCellResponse>> Handle(CreateCellCommand request, CancellationToken cancellationToken)
        {
            var endereco = new Address("Amazonas", "2", "12", "Alto Das Caraibas", "Luziania", "GO", "TESTE", "72813110");
            var usuario = new User(name: "Miqueias", email: "miqueias@ipcc.com", phone: "61996100819", password: "123456789", endereco, Guid.NewGuid(), "Sou + Jesus", UserRole.Leader);

            var validacaoUser = usuario.Validate();

            if (!validacaoUser.IsSuccess) return Result<CreatedCellResponse>.Fail(validacaoUser.Errors);

            var cell = new Cell(usuario.Name, usuario.Id, usuario);

            var validacaoCell = cell.Validate();

            if (!validacaoCell.IsSuccess) return Result<CreatedCellResponse>.Fail(validacaoCell.Errors);

            await _cellRepository.Create(cell);

            var response = new CreatedCellResponse(cell.Id);

            return Result<CreatedCellResponse>.Ok(response);
        }
    }
}

using Auth.API.Application.Cells.UpdateCell;

namespace Auth.API.Application.Cells.Models
{
    public record class UpdateCellInputModel(string Name, Guid LiderId);

    public static class UpdateCellInputModelExtensions
    {
        public static UpdateCellCommand ToCommand(this UpdateCellInputModel inputModel, Guid id)
        {
            var request = new UpdateCellRequest(inputModel.Name, inputModel.LiderId);
            return new UpdateCellCommand(id, request);
        }
    }
}

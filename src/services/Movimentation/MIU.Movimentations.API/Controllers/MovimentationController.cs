using Microsoft.AspNetCore.Mvc;
using MIU.Core.Mediator;
using MIU.Movimentations.Application.Commands;
using MIU.Movimentations.Domain.Repositories;
using MIU.WebAPI.Core.Controllers;
using System;
using System.Threading.Tasks;

namespace MIU.Movimentations.API.Controllers
{
    [ApiController]
    [Route("api/movimentations")]
    public class MovimentationController : MainController
    {
        private readonly IMovimentationRepository _movimentationRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public MovimentationController(IMovimentationRepository movimentationRepository, IMediatorHandler mediatorHandler)
        {
            _movimentationRepository = movimentationRepository;
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var movimentations = await _movimentationRepository.GetMovimentations();

            return Ok(movimentations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovimentation(RegisterMovimentationCommand registerMovimentationCommand)
        {
            var result = await _mediatorHandler.PublisherCommand(registerMovimentationCommand);

            if (result.Errors.Count > 0)
                return CustomResponse(result);

            return CustomResponse(new CommandResponse("Movimentação adicionanda com sucesso"));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovimentation(string id)
        {
            var deleteMovimentationCommand = new DeleteMovimentationCommand { Id = Guid.Parse(id) };

            var result = await _mediatorHandler.PublisherCommand(deleteMovimentationCommand);

            if (result.Errors.Count > 0)
                return CustomResponse(result);

            return CustomResponse(new CommandResponse("Movimentação deletada com sucesso"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovimentation(UpdateMovimentationCommand updateMovimentationCommand)
        {
            var result = await _mediatorHandler.PublisherCommand(updateMovimentationCommand);

            if (result.Errors.Count > 0)
                return CustomResponse(result);

            return CustomResponse(new CommandResponse("Movimentação atualizada com sucesso"));
        }
    }
}

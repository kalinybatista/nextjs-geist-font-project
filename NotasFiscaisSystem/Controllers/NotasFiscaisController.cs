using Microsoft.AspNetCore.Mvc;
using NotasFiscaisSystem.Models;
using NotasFiscaisSystem.Services;

namespace NotasFiscaisSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotasFiscaisController : ControllerBase
    {
        private readonly INotaFiscalService _notaFiscalService;

        public NotasFiscaisController(INotaFiscalService notaFiscalService)
        {
            _notaFiscalService = notaFiscalService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotaFiscal>>> GetAll()
        {
            try
            {
                var notas = await _notaFiscalService.GetAllAsync();
                return Ok(notas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar notas fiscais: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotaFiscal>> GetById(int id)
        {
            try
            {
                var nota = await _notaFiscalService.GetByIdAsync(id);
                if (nota == null)
                    return NotFound();

                return Ok(nota);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar nota fiscal: {ex.Message}");
            }
        }

        [HttpGet("chave/{chaveAcesso}")]
        public async Task<ActionResult<NotaFiscal>> GetByChaveAcesso(string chaveAcesso)
        {
            try
            {
                var nota = await _notaFiscalService.GetByChaveAcessoAsync(chaveAcesso);
                if (nota == null)
                    return NotFound();

                return Ok(nota);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar nota fiscal: {ex.Message}");
            }
        }

        [HttpGet("periodo")]
        public async Task<ActionResult<IEnumerable<NotaFiscal>>> GetByPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            try
            {
                var notas = await _notaFiscalService.GetByPeriodoAsync(inicio, fim);
                return Ok(notas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar notas por período: {ex.Message}");
            }
        }

        [HttpGet("total/{tipoOperacao}")]
        public async Task<ActionResult<decimal>> GetTotalPorPeriodo(
            [FromQuery] DateTime inicio, 
            [FromQuery] DateTime fim, 
            string tipoOperacao)
        {
            try
            {
                var total = await _notaFiscalService.GetTotalPorPeriodoAsync(inicio, fim, tipoOperacao);
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao calcular total: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<NotaFiscal>> Create([FromBody] NotaFiscal notaFiscal)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _notaFiscalService.CreateAsync(notaFiscal);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar nota fiscal: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NotaFiscal>> Update(int id, [FromBody] NotaFiscal notaFiscal)
        {
            try
            {
                if (id != notaFiscal.Id)
                    return BadRequest("ID da URL não corresponde ao ID da nota");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updated = await _notaFiscalService.UpdateAsync(notaFiscal);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar nota fiscal: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _notaFiscalService.DeleteAsync(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover nota fiscal: {ex.Message}");
            }
        }

        [HttpPost("{id}/autorizar")]
        public async Task<IActionResult> Autorizar(int id, [FromBody] AutorizacaoRequest request)
        {
            try
            {
                var autorizado = await _notaFiscalService.AutorizarNotaAsync(id, request.Protocolo);
                if (!autorizado)
                    return NotFound();

                return Ok(new { message = "Nota autorizada com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao autorizar nota fiscal: {ex.Message}");
            }
        }

        [HttpPost("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id, [FromBody] CancelamentoRequest request)
        {
            try
            {
                var cancelado = await _notaFiscalService.CancelarNotaAsync(id, request.Motivo);
                if (!cancelado)
                    return NotFound();

                return Ok(new { message = "Nota cancelada com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cancelar nota fiscal: {ex.Message}");
            }
        }
    }

    public class AutorizacaoRequest
    {
        public string Protocolo { get; set; } = string.Empty;
    }

    public class CancelamentoRequest
    {
        public string Motivo { get; set; } = string.Empty;
    }
}

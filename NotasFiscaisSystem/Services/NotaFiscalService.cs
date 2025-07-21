using Microsoft.EntityFrameworkCore;
using NotasFiscaisSystem.Data;
using NotasFiscaisSystem.Models;

namespace NotasFiscaisSystem.Services
{
    public interface INotaFiscalService
    {
        Task<IEnumerable<NotaFiscal>> GetAllAsync();
        Task<NotaFiscal?> GetByIdAsync(int id);
        Task<NotaFiscal?> GetByChaveAcessoAsync(string chaveAcesso);
        Task<NotaFiscal> CreateAsync(NotaFiscal notaFiscal);
        Task<NotaFiscal> UpdateAsync(NotaFiscal notaFiscal);
        Task<bool> DeleteAsync(int id);
        Task<bool> AutorizarNotaAsync(int id, string protocolo);
        Task<bool> CancelarNotaAsync(int id, string motivo);
        Task<IEnumerable<NotaFiscal>> GetByPeriodoAsync(DateTime inicio, DateTime fim);
        Task<decimal> GetTotalPorPeriodoAsync(DateTime inicio, DateTime fim, string tipoOperacao);
    }

    public class NotaFiscalService : INotaFiscalService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NotaFiscalService> _logger;

        public NotaFiscalService(AppDbContext context, ILogger<NotaFiscalService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<NotaFiscal>> GetAllAsync()
        {
            return await _context.NotasFiscais
                .Include(n => n.Itens)
                .OrderByDescending(n => n.DataEmissao)
                .ToListAsync();
        }

        public async Task<NotaFiscal?> GetByIdAsync(int id)
        {
            return await _context.NotasFiscais
                .Include(n => n.Itens)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<NotaFiscal?> GetByChaveAcessoAsync(string chaveAcesso)
        {
            return await _context.NotasFiscais
                .Include(n => n.Itens)
                .FirstOrDefaultAsync(n => n.ChaveAcesso == chaveAcesso);
        }

        public async Task<NotaFiscal> CreateAsync(NotaFiscal notaFiscal)
        {
            try
            {
                // Validar chave de acesso única
                if (await _context.NotasFiscais.AnyAsync(n => n.ChaveAcesso == notaFiscal.ChaveAcesso))
                {
                    throw new InvalidOperationException("Nota fiscal com esta chave de acesso já existe.");
                }

                _context.NotasFiscais.Add(notaFiscal);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Nota fiscal {NumeroNota} criada com sucesso", notaFiscal.NumeroNota);
                return notaFiscal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar nota fiscal");
                throw;
            }
        }

        public async Task<NotaFiscal> UpdateAsync(NotaFiscal notaFiscal)
        {
            try
            {
                var existing = await _context.NotasFiscais
                    .Include(n => n.Itens)
                    .FirstOrDefaultAsync(n => n.Id == notaFiscal.Id);

                if (existing == null)
                    throw new KeyNotFoundException("Nota fiscal não encontrada");

                // Atualizar propriedades
                _context.Entry(existing).CurrentValues.SetValues(notaFiscal);
                
                // Atualizar itens
                existing.Itens.Clear();
                foreach (var item in notaFiscal.Itens)
                {
                    existing.Itens.Add(item);
                }

                existing.DataAtualizacao = DateTime.Now;
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Nota fiscal {NumeroNota} atualizada com sucesso", notaFiscal.NumeroNota);
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar nota fiscal");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var nota = await _context.NotasFiscais.FindAsync(id);
                if (nota == null)
                    return false;

                _context.NotasFiscais.Remove(nota);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Nota fiscal {Id} removida com sucesso", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover nota fiscal");
                return false;
            }
        }

        public async Task<bool> AutorizarNotaAsync(int id, string protocolo)
        {
            try
            {
                var nota = await _context.NotasFiscais.FindAsync(id);
                if (nota == null)
                    return false;

                nota.Status = "Autorizada";
                nota.ProtocoloAutorizacao = protocolo;
                nota.DataAutorizacao = DateTime.Now;
                nota.DataAtualizacao = DateTime.Now;

                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Nota fiscal {Id} autorizada com protocolo {Protocolo}", id, protocolo);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao autorizar nota fiscal");
                return false;
            }
        }

        public async Task<bool> CancelarNotaAsync(int id, string motivo)
        {
            try
            {
                var nota = await _context.NotasFiscais.FindAsync(id);
                if (nota == null)
                    return false;

                nota.Status = "Cancelada";
                nota.MotivoCancelamento = motivo;
                nota.DataCancelamento = DateTime.Now;
                nota.DataAtualizacao = DateTime.Now;

                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Nota fiscal {Id} cancelada: {Motivo}", id, motivo);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cancelar nota fiscal");
                return false;
            }
        }

        public async Task<IEnumerable<NotaFiscal>> GetByPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _context.NotasFiscais
                .Include(n => n.Itens)
                .Where(n => n.DataEmissao >= inicio && n.DataEmissao <= fim)
                .OrderByDescending(n => n.DataEmissao)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPorPeriodoAsync(DateTime inicio, DateTime fim, string tipoOperacao)
        {
            return await _context.NotasFiscais
                .Where(n => n.DataEmissao >= inicio && 
                           n.DataEmissao <= fim && 
                           n.TipoOperacao == tipoOperacao &&
                           n.Status == "Autorizada")
                .SumAsync(n => n.ValorTotal);
        }
    }
}

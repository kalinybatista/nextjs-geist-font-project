using System.ComponentModel.DataAnnotations;

namespace NotasFiscaisSystem.Models
{
    public class NotaFiscal
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(44)]
        public string ChaveAcesso { get; set; } = string.Empty;
        
        [Required]
        public string NumeroNota { get; set; } = string.Empty;
        
        [Required]
        public string Serie { get; set; } = string.Empty;
        
        public DateTime DataEmissao { get; set; }
        
        public DateTime DataEntradaSaida { get; set; }
        
        [Required]
        public string CNPJEmitente { get; set; } = string.Empty;
        
        [Required]
        public string NomeEmitente { get; set; } = string.Empty;
        
        [Required]
        public string CNPJDestinatario { get; set; } = string.Empty;
        
        [Required]
        public string NomeDestinatario { get; set; } = string.Empty;
        
        public decimal ValorTotal { get; set; }
        
        public decimal ValorICMS { get; set; }
        
        public decimal ValorIPI { get; set; }
        
        public decimal ValorFrete { get; set; }
        
        public decimal ValorSeguro { get; set; }
        
        public decimal ValorDesconto { get; set; }
        
        public decimal ValorOutrasDespesas { get; set; }
        
        public string NaturezaOperacao { get; set; } = string.Empty;
        
        public string TipoOperacao { get; set; } = string.Empty; // Entrada ou Saída
        
        public string Status { get; set; } = "Pendente"; // Pendente, Autorizada, Cancelada
        
        public string? ProtocoloAutorizacao { get; set; }
        
        public DateTime? DataAutorizacao { get; set; }
        
        public string? MotivoCancelamento { get; set; }
        
        public DateTime? DataCancelamento { get; set; }
        
        public string XMLContent { get; set; } = string.Empty;
        
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
        
        public virtual ICollection<ItemNotaFiscal> Itens { get; set; } = new List<ItemNotaFiscal>();
    }

    public class ItemNotaFiscal
    {
        [Key]
        public int Id { get; set; }
        
        public int NotaFiscalId { get; set; }
        
        public int NumeroItem { get; set; }
        
        [Required]
        public string CodigoProduto { get; set; } = string.Empty;
        
        [Required]
        public string DescricaoProduto { get; set; } = string.Empty;
        
        public string NCM { get; set; } = string.Empty;
        
        public string CFOP { get; set; } = string.Empty;
        
        public string UnidadeMedida { get; set; } = string.Empty;
        
        public decimal Quantidade { get; set; }
        
        public decimal ValorUnitario { get; set; }
        
        public decimal ValorTotal { get; set; }
        
        public decimal Desconto { get; set; }
        
        public virtual NotaFiscal NotaFiscal { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace NotasFiscaisSystem.Models
{
    public class Empresa
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(14)]
        public string CNPJ { get; set; } = string.Empty;
        
        [Required]
        public string RazaoSocial { get; set; } = string.Empty;
        
        public string NomeFantasia { get; set; } = string.Empty;
        
        [Required]
        public string InscricaoEstadual { get; set; } = string.Empty;
        
        public string? InscricaoMunicipal { get; set; }
        
        [Required]
        public string Endereco { get; set; } = string.Empty;
        
        [Required]
        public string Numero { get; set; } = string.Empty;
        
        public string? Complemento { get; set; }
        
        [Required]
        public string Bairro { get; set; } = string.Empty;
        
        [Required]
        public string Cidade { get; set; } = string.Empty;
        
        [Required]
        public string UF { get; set; } = string.Empty;
        
        [Required]
        public string CEP { get; set; } = string.Empty;
        
        [Required]
        public string Telefone { get; set; } = string.Empty;
        
        [Required]
        public string Email { get; set; } = string.Empty;
        
        public string? CertificadoDigital { get; set; }
        
        public DateTime? ValidadeCertificado { get; set; }
        
        public bool Ativo { get; set; } = true;
        
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        
        public virtual ICollection<NotaFiscal> NotasFiscaisEmitidas { get; set; } = new List<NotaFiscal>();
    }
}

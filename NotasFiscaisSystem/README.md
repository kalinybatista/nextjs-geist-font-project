# Sistema de Declaração de Notas Fiscais

Sistema completo em C# para gerenciamento e declaração de notas fiscais eletrônicas, desenvolvido com ASP.NET Core 8.0.

## Funcionalidades

- ✅ Cadastro de notas fiscais eletrônicas (NF-e)
- ✅ Gerenciamento de empresas emitentes e destinatárias
- ✅ Autorização e cancelamento de notas
- ✅ Relatórios por período
- ✅ Dashboard com estatísticas
- ✅ API RESTful completa
- ✅ Interface web responsiva

## Tecnologias Utilizadas

- **ASP.NET Core 8.0**
- **Entity Framework Core 8.0**
- **SQL Server** (LocalDB)
- **Bootstrap 5.3**
- **Font Awesome**
- **Swagger/OpenAPI**

## Instalação e Configuração

### Pré-requisitos

- .NET 8.0 SDK
- SQL Server LocalDB ou SQL Server Express

### Passos para Execução

1. **Clonar o repositório**
   ```bash
   git clone [url-do-repositorio]
   cd NotasFiscaisSystem
   ```

2. **Restaurar pacotes NuGet**
   ```bash
   dotnet restore
   ```

3. **Criar e aplicar migrações do banco de dados**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Executar a aplicação**
   ```bash
   dotnet run
   ```

5. **Acessar o sistema**
   - API: https://localhost:5001/api/notasfiscais
   - Interface Web: https://localhost:5001
   - Swagger: https://localhost:5001/swagger

## Estrutura do Projeto

```
NotasFiscaisSystem/
├── Models/              # Entidades do banco de dados
├── Services/            # Lógica de negócios
├── Controllers/         # Controladores da API
├── Views/               # Interface web
├── Data/                # Contexto do Entity Framework
├── Program.cs           # Configuração da aplicação
├── appsettings.json     # Configurações
└── README.md           # Este arquivo
```

## Endpoints da API

### Notas Fiscais
- `GET /api/notasfiscais` - Listar todas as notas
- `GET /api/notasfiscais/{id}` - Buscar nota por ID
- `GET /api/notasfiscais/chave/{chave}` - Buscar por chave de acesso
- `GET /api/notasfiscais/periodo?inicio={data}&fim={data}` - Buscar por período
- `POST /api/notasfiscais` - Criar nova nota
- `PUT /api/notasfiscais/{id}` - Atualizar nota
- `DELETE /api/notasfiscais/{id}` - Remover nota
- `POST /api/notasfiscais/{id}/autorizar` - Autorizar nota
- `POST /api/notasfiscais/{id}/cancelar` - Cancelar nota

## Modelos de Dados

### NotaFiscal
- Chave de acesso única (44 caracteres)
- Dados do emitente e destinatário
- Valores totais e tributos
- Status (Pendente/Autorizada/Cancelada)
- Itens da nota

### ItemNotaFiscal
- Produto/serviço
- Quantidade e valores
- NCM, CFOP, unidade de medida

### Empresa
- Dados cadastrais completos
- CNPJ, inscrição estadual
- Endereço completo
- Certificado digital

## Exemplos de Uso

### Criar uma nova nota fiscal
```json
POST /api/notasfiscais
{
  "chaveAcesso": "35240312345678000195550010000000011000000000",
  "numeroNota": "0001",
  "serie": "1",
  "dataEmissao": "2024-03-15T10:00:00",
  "cnpjEmitente": "12345678000195",
  "nomeEmitente": "EMPRESA EXEMPLO LTDA",
  "cnpjDestinatario": "98765432000195",
  "nomeDestinatario": "CLIENTE EXEMPLO SA",
  "valorTotal": 1000.00,
  "valorICMS": 180.00,
  "tipoOperacao": "Saída",
  "naturezaOperacao": "Venda de mercadoria"
}
```

### Buscar notas por período
```
GET /api/notasfiscais/periodo?inicio=2024-01-01&fim=2024-03-31
```

## Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

## Suporte

Para dúvidas ou suporte, abra uma issue no repositório ou entre em contato através do email: suporte@notasfiscaissystem.com.br

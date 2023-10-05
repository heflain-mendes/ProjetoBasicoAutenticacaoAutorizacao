using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Identity.Data.DTOs
{
    public  class UsuarioLoginResponse
    {
        public bool Sucesso { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Token { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? DataExpiracao { get; set; }
        public List<string> Erros { get; private set; }

        public UsuarioLoginResponse() => Erros = new List<string>();

        public UsuarioLoginResponse(bool sucesso = true) : this() => this.Sucesso = sucesso;

        public UsuarioLoginResponse(bool sucesso, string token, DateTime DataExpiracao) : this()
        {
            this.Sucesso = sucesso;
            this.Token = token;
            this.DataExpiracao = DataExpiracao;
        }

        public void AdicionarError(string erro)
        {
            this.Erros.Add(erro);
        }
    }
}

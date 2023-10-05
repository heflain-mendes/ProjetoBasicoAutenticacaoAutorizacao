
using System.Text.Json.Serialization;

namespace Identity.Data.DTOs
{
    public class UsuarioCadastroResponse
    {
        public bool Sucesso { get; private set; }
        public List<string> Erros { get; private set; }

        public UsuarioCadastroResponse()
        {
            Erros = new List<string>();
        }

        public UsuarioCadastroResponse(bool sucesso = true): this() {
            this.Sucesso = sucesso;
        }

        public void AdicionarErros(IEnumerable<string> erros)
        {
            this.Erros.AddRange(erros);
        }
    }
}

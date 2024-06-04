namespace Acessos.Models;

public class Circular
{
    public int Id { get; set; }
    public string Protocolo {  get; set; }
    public string Assunto { get; set; }
    public string Conteudo { get; set; }
    public string Destinatario { get; set; }
    public string Remetente { get; set; }
    public DateTime DataEnvio { get; set; }
    public DateTime? DataRecebimento { get; set; }
    public string Status { get; set; }
}

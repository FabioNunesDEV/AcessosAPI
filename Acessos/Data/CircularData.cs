using Acessos.Models;
using Acessos.Services;

namespace Acessos.Data;

public class CircularData
{
    static int _Id = 0;
    List<Circular> _circulares;

    public CircularData()
    {
        _Id = 0;
        _circulares = new List<Circular>();
    }

    /// <summary>
    /// Cria uma lista de objetos Circular.
    /// </summary>
    /// <returns>Retorna lista de objetos Circular</returns>
    public List<Circular> GetDadosCirculares()
    {

        _circulares.Add(new Circular
        {
            Id = 1,
            Protocolo = "f8c8e3a1-9b4d-4f6b-8e2a-7a5d6b9c2e0d",
            Assunto = "Expediente no proximo feriado",
            Conteudo = "Prezados funcionários, lembramos que não haverá expediente no dia 30/06/2024 devido ao feriado de Corpus Christi. Aproveitem o descanso e retornem com energia renovada!",
            Destinatario = "Depto Geral",
            Remetente = "Fábio Nunes",
            DataEnvio = new DateTime(2024, 05,27,8,0,0),
            DataRecebimento = new DateTime(2024,05,27,12,0,0),
            Status = "Lido"
        });

        _circulares.Add(new Circular
        {
            Id = 2,
            Protocolo = "b5a7f8d2-3e9c-4a1a-8e6b-9f0d7c6e2b4f",
            Assunto = "Horario de uso refeitorios",
            Conteudo = "Caros colaboradores, lembramos que o refeitório deve ser utilizado apenas durante o horário de almoço. Agradecemos a compreensão e colaboração de todos!",
            Destinatario = "Depto Financeiro",
            Remetente = "Fábio Nunes",
            DataEnvio = new DateTime(2024, 06, 04, 8, 0, 0),
            DataRecebimento = null,
            Status = "Não lido"
        });

        _circulares.Add(new Circular
        {
            Id = 3,
            Protocolo = "e1d9b0c3-8a6f-4c7e-9b2d-5f8a7b6c3d9e",
            Assunto = "Registro em carteira",
            Conteudo = "Prezado RH, informamos que as carteiras de trabalho serão preenchidas após 90 dias de experiência do candidato. Agradecemos a compreensão e colaboração de todos!",
            Destinatario = "Depto RH",
            Remetente = "Fábio Nunes",
            DataEnvio = new DateTime(2024, 05, 27, 15, 30, 0),
            DataRecebimento = new DateTime(2024, 06, 04, 8, 0, 0),
            Status = "Lido"
        });



        return _circulares;
    } 

    /// <summary>
    /// Incre
    /// </summary>
    /// <returns>Retornar numero de Id</returns>
    public int IncrementarId()
    {
        // Registra o valor do ultimo id para simular incrementação quando necessário.
        _Id = _circulares[_circulares.Count - 1].Id;

        return ++_Id;
    }
}

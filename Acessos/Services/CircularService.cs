namespace Acessos.Services;

public class CircularService
{
    public string CriarProtocolo()
    {
        return Guid.NewGuid().ToString();
    }
}


public interface IOutcome
{
    public string DisplayText { get; } //lo que se muestra en la UI de eventos, texto descriptivo rollo ganas 20 pierdes tal
    public void Execute();
}

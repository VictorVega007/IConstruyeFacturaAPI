namespace IConstruye.Factura.Core.ValueObjects;

public class QueryCounter(int maxViews = 3)
{
    private int MaxViews { get; } = maxViews;
    public int TimesConsulted { get; private set; }

    public bool CanBeConsulted => TimesConsulted < MaxViews;

    public void RegisterView()
    {
        if (!CanBeConsulted)
            throw new InvalidOperationException("The consult has reach the maximum number of views.");
        
        TimesConsulted++;
    }
}
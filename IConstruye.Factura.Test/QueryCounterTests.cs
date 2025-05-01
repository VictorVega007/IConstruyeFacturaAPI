using FluentAssertions;
using IConstruye.Factura.Core.ValueObjects;

namespace IConstruye.Factura.Test;

public class QueryCounterTests
{
    [Fact]
    public void Should_Allow_Up_To_Max_Views()
    {
        var counter = new QueryCounter();
        
        counter.RegisterView();
        counter.RegisterView();
        counter.RegisterView();

        counter.TimesConsulted.Should().Be(3);
        counter.CanBeConsulted.Should().BeFalse();
    }

    [Fact]
    public void Should_Throw_When_Exceeding_MaxViews()
    {
        var counter = new QueryCounter();
        
        counter.RegisterView();
        counter.RegisterView();
        counter.RegisterView();
        
        var act = () => counter.RegisterView();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The consult has reach the maximum number of views.");
    }
}
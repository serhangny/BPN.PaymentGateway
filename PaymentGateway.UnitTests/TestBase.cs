using System.Linq;
using AutoFixture;

namespace PaymentGateway.UnitTests;

public abstract class TestBase
{
    protected readonly IFixture Fixture;
    
    protected TestBase()
    {
        Fixture = new Fixture();
        
        // Configure AutoFixture to ignore circular references
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => Fixture.Behaviors.Remove(b));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}


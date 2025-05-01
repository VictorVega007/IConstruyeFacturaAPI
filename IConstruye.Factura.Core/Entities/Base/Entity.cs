namespace IConstruye.Factura.Core.Entities.Base;

public abstract class Entity : EntityBase<Guid>
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}
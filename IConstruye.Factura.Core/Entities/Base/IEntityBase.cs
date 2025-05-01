namespace IConstruye.Factura.Core.Entities.Base;

public interface IEntityBase<TId>
{
    TId Id { get; }
}
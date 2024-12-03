namespace MetaCortex.Orders.DataAcess.Interface;

public interface IEntity<T>
{
    T Id { get; set; }
}

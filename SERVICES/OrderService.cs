using GadgetShop.Models;

public class OrderService
{
    private List<Order> _orders = new();

    public IEnumerable<Order> GetAll() => _orders;

    public Order Get(string id) =>
        _orders.FirstOrDefault(o => o.Id == id);

    public void Add(Order order)
    {
        _orders.Add(order);
    }

    public void UpdateStatus(string id, OrderStatus status)
    {
        var order = Get(id);
        if (order != null)
            order.Status = status;
    }
}

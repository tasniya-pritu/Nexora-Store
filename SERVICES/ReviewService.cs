using GadgetShop.Models;

public class ReviewService
{
    private List<Review> _reviews = new();

    public IEnumerable<Review> GetByProduct(int productId)
    {
        return _reviews.Where(r => r.ProductId == productId);
    }

    public void Add(Review review)
    {
        _reviews.Add(review);
    }
}

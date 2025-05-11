namespace FoodShoppingCartMvc.Repositories
{
    public interface ICategoryRepository
    {
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(Category category);
        Task<Category?> GetCategoryById(int id);
        Task<IEnumerable<Category>> GetCategory();
    }
}

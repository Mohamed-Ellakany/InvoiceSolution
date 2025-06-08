using Invoice.DAL.Data;
using Invoice.DAL.Entities;

namespace Invoice.PL.DataSeeding
{
    public class ProductsSeeding
    {
        public static async Task Seeding(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new()
                    {
                        Name = "Laptop",
                       UnitPrice = 1200.00m,
                    },
                    new()
                    {
                        Name = "Smartphone",
                        UnitPrice = 800.00m,
                    },
                    new()
                    {
                        Name = "Tablet",
                        UnitPrice = 500.00m,
                    },
                    new()
                    {
                        Name = "Headphones",
                        UnitPrice = 150.00m,
                    },
            };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();

            }
        }
        }
}

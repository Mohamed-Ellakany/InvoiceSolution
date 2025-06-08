using Invoice.DAL.Data;
using Invoice.DAL.Entities;

namespace Invoice.PL.DataSeeding
{
    public static class CustomersSeeding
    {
        public static async Task Seeding(ApplicationDbContext context)
        {
            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
                {
                    new()
                    {
                        Name = "Ali",

                    },
                    new()
                    {
                        Name = "Hassan",
                    },
                    new()
                    {
                        Name = "Mohamed",
                    },
                    new()
                    {
                        Name = "Ahmed",
                    },
            };

                await context.Customers.AddRangeAsync(customers);
               await context.SaveChangesAsync();

            }
        }
    }
}
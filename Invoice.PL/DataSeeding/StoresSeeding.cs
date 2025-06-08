using Invoice.DAL.Data;
using Invoice.DAL.Entities;

namespace Invoice.PL.DataSeeding
{
    public static class StoresSeeding
    {
        public static async Task Seeding(ApplicationDbContext context)
        {
            if (!context.Stores.Any())
            {
                var stores = new List<Store>
                {
                    new()
                    {
                        Name = "Amazon"
                    },
                    new()
                    {
                        Name = "eBay"
                    },
                    new()
                    {
                        Name = "Walmart"
                    },
                    new()
                    {
                        Name = "Best Buy"
                    },
            };

                await context.Stores.AddRangeAsync(stores);
               await context.SaveChangesAsync();

            }
        }
    }
}

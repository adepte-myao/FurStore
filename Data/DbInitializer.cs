using FurStore.Common.Enums;
using FurStore.Models.Auth;
using FurStore.Models.Store;
using Microsoft.AspNetCore.Identity;

namespace FurStore.Data
{
    public class DbInitializer
    {
        public async static Task Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (context is null)
                {
                    Console.WriteLine("AppDataInitializer: context is null");
                    return;
                }

                // Furnitures
                if (!context.Furnitures.Any())
                {
                    context.Furnitures.AddRange(new List<Furniture>()
                    {
                        new Furniture()
                        {
                            Id = Guid.Parse("EAA7B389-F66C-4938-876F-A39671DAA301"),
                            Title= "sofa",
                            Description= "description",
                            ImageUrl = "https://m.media-amazon.com/images/I/51dx6RUA+BL._SL1000_.jpg",
                            Price = 240.5,
                            FurnitureType = FurnitureTypes.Leather
                        },
                        new Furniture()
                        {
                            Id = Guid.NewGuid(),
                            Title= "chair",
                            Description= "description2",
                            ImageUrl = "https://vsestulya.ru/image/cache/catalog/m-siti/stulya/riverbank/m_GREY1-1000x1000.jpg",
                            Price = 210.5,
                            FurnitureType = FurnitureTypes.Soft
                        }
                    });

                    context.SaveChanges();
                }

                // Orders
                if (!context.Orders.Any())
                {
                    context.Orders.AddRange(new List<Order>()
                    {
                        new Order()
                        {
                            User = null,
                            FurnitureElementsList = new List<FurnitureOrderElement>()
                            {
                                new FurnitureOrderElement()
                                {
                                    FurnitureId = Guid.Parse("EAA7B389-F66C-4938-876F-A39671DAA301"),
                                    Title= "title",
                                    Description= "description",
                                    ImageUrl = "https://m.media-amazon.com/images/I/51dx6RUA+BL._SL1000_.jpg",
                                    Price = 240.5,
                                    FurnitureType = FurnitureTypes.Leather,
                                    Amount = 4,
                                }
                            },
                            Status = OrderStatus.Accepted,
                            CreationTime = DateTime.Now,
                            DoneTime = null,
                        }
                    });

                    context.SaveChanges();
                }

                if (!context.Roles.Any() || 
                    !context.Roles.Where(role => role.Name == "Admin").Any() ||
                    !context.Roles.Where(role => role.Name == "Factory").Any() ||
                    !context.Roles.Where(role => role.Name == "Client").Any())
                {
                    var rolesManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                    await rolesManager.CreateAsync(new IdentityRole("Admin"));
                    await rolesManager.CreateAsync(new IdentityRole("Client"));
                    await rolesManager.CreateAsync(new IdentityRole("Factory"));

                    context.SaveChanges();
                }

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                if (await userManager.FindByEmailAsync("admin@admin.com") == null)
                {
                    var user = new User
                    {
                        Email = "admin@admin.com",
                        Year = 1999,
                        UserName = "admin@admin.com",
                    };
                    var result = await userManager.CreateAsync(user, "Fire_000");

                    await userManager.AddToRoleAsync(user, "Admin");

                    context.SaveChanges();
                }

                if (await userManager.FindByEmailAsync("factory@faf.com") == null)
                {
                    var user = new User
                    {
                        Email = "factory@faf.com",
                        Year = 1999,
                        UserName = "factory@faf.com",
                    };
                    var result = await userManager.CreateAsync(user, "Fire_000");

                    await userManager.AddToRoleAsync(user, "Factory");

                    context.SaveChanges();
                }

                if (await userManager.FindByEmailAsync("client@client.com") == null)
                {
                    var user = new User
                    {
                        Email = "client@client.com",
                        Year = 1999,
                        UserName = "client@client.com",
                    };
                    var result = await userManager.CreateAsync(user, "Fire_000");

                    await userManager.AddToRoleAsync(user, "Client");

                    context.SaveChanges();
                }
            }    
        }
    }
}

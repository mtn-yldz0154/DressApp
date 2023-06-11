using Microsoft.EntityFrameworkCore;


namespace DressApp.WebUi.Data.Models
{
    public class Context:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=LAPTOP-LM2N83TK;database=DressAppShopp;" +
                "integrated security=true;");
        }

        public DbSet<Rayon> Rayons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductUser> ProductUsers { get; set; }
        public DbSet<ProductUserItem> ProductUserItems { get; set; }
        public DbSet<Admin> Admins { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Sepet> Sepets { get; set; }

        public DbSet<Favori> Favoris { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Like> Likes { get; set; }
        public DbSet<LikeItem> LikeItems { get; set; }



    }
}

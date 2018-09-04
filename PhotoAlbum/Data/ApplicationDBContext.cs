using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Models;


namespace PhotoAlbum.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Stack> Stacks { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<People> PeopleDb { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<PictureLocation> PictureLocations { get; set; }
        public DbSet<PictureEvent> PictureEvents { get; set; }
        public DbSet<PictureAuthor> PictureAuthors { get; set; }
        public DbSet<PictureXAlbum> PictureXAlbums { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PictureLocation>().HasKey(p => new { p.LocationID, p.PictureID });
            modelBuilder.Entity<PictureEvent>().HasKey(p => new { p.EventID, p.PictureID });
            modelBuilder.Entity<PictureAuthor>().HasKey(p => new { p.AuthorID, p.PictureID });
            modelBuilder.Entity<PictureXAlbum>().HasKey(p => new { p.AlbumID, p.PictureID });
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Entities;

public partial class KoiCareSystemDbContext : DbContext
{
    public KoiCareSystemDbContext()
    {
    }

    public KoiCareSystemDbContext(DbContextOptions<KoiCareSystemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<KoiGrowthChartsTbl> KoiGrowthChartsTbls { get; set; }

    public virtual DbSet<KoisTbl> KoisTbls { get; set; }

    public virtual DbSet<MembershipsTbl> MembershipsTbls { get; set; }

    public virtual DbSet<NotesTbl> NotesTbls { get; set; }

    public virtual DbSet<OrderDetailsTbl> OrderDetailsTbls { get; set; }

    public virtual DbSet<OrdersTbl> OrdersTbls { get; set; }

    public virtual DbSet<PondsTbl> PondsTbls { get; set; }

    public virtual DbSet<ProductsTbl> ProductsTbls { get; set; }

    public virtual DbSet<ShopsTbl> ShopsTbls { get; set; }

    public virtual DbSet<UserTbl> UserTbls { get; set; }

    public virtual DbSet<WaterParametersTbl> WaterParametersTbls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(DatabaseConfig.DATABASE);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KoiGrowthChartsTbl>(entity =>
        {
            entity.HasKey(e => e.ChartId).HasName("PK__koi_grow__D7FFC8C2DA959ACE");

            entity.ToTable("koi_growth_charts_tbl");

            entity.Property(e => e.ChartId).HasColumnName("chart_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.HealthStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("healthStatus");
            entity.Property(e => e.KoiId).HasColumnName("koiId");
            entity.Property(e => e.Length)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("length");
            entity.Property(e => e.Weight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("weight");

            entity.HasOne(d => d.Koi).WithMany(p => p.KoiGrowthChartsTbls)
                .HasForeignKey(d => d.KoiId)
                .HasConstraintName("FK__koi_growt__koiId__4D94879B");
        });

        modelBuilder.Entity<KoisTbl>(entity =>
        {
            entity.HasKey(e => e.KoiId).HasName("PK__kois_tbl__915924CF4A83A9DA");

            entity.ToTable("kois_tbl");

            entity.Property(e => e.KoiId).HasColumnName("koiId");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Breed)
                .HasMaxLength(50)
                .HasColumnName("breed");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Length)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("length");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Physique)
                .HasMaxLength(50)
                .HasColumnName("physique");
            entity.Property(e => e.PondId).HasColumnName("pondId");
            entity.Property(e => e.Sex).HasColumnName("sex");
            entity.Property(e => e.Weight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("weight");

            entity.HasOne(d => d.Pond).WithMany(p => p.KoisTbls)
                .HasForeignKey(d => d.PondId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__kois_tbl__pondId__4AB81AF0");
        });

        modelBuilder.Entity<MembershipsTbl>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__membersh__86AA3B173B79CB53");

            entity.ToTable("memberships_tbl");

            entity.Property(e => e.MembershipId).HasColumnName("membershipId");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.MembershipsTbls)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__membershi__userI__4222D4EF");
        });

        modelBuilder.Entity<NotesTbl>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PK__notes_tb__03C97EFD1DAF93F2");

            entity.ToTable("notes_tbl");

            entity.Property(e => e.NoteId).HasColumnName("noteId");
            entity.Property(e => e.NoteName)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("noteName");
            entity.Property(e => e.NoteText)
                .HasColumnType("text")
                .HasColumnName("noteText");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.NotesTbls)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__notes_tbl__userI__3F466844");
        });

        modelBuilder.Entity<OrderDetailsTbl>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__order_de__BAD83E4B4128035F");

            entity.ToTable("order_details_tbl");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalPrice");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetailsTbls)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__order_det__order__5CD6CB2B");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetailsTbls)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__order_det__produ__5DCAEF64");
        });

        modelBuilder.Entity<OrdersTbl>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders_t__0809335DC52DF519");

            entity.ToTable("orders_tbl");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalAmount");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.OrdersTbls)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__orders_tb__userI__5535A963");
        });

        modelBuilder.Entity<PondsTbl>(entity =>
        {
            entity.HasKey(e => e.PondId).HasName("PK__ponds_tb__74327499656CA398");

            entity.ToTable("ponds_tbl");

            entity.Property(e => e.PondId).HasColumnName("pondId");
            entity.Property(e => e.Depth)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("depth");
            entity.Property(e => e.DrainCount).HasColumnName("drain_count");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PumpCapacity).HasColumnName("pump_capacity");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Volume).HasColumnName("volume");

            entity.HasOne(d => d.User).WithMany(p => p.PondsTbls)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ponds_tbl__userI__44FF419A");
        });

        modelBuilder.Entity<ProductsTbl>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__2D10D16AC8597916");

            entity.ToTable("products_tbl");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductInfo)
                .HasColumnType("text")
                .HasColumnName("productInfo");
            entity.Property(e => e.ShopId).HasColumnName("shopId");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.Shop).WithMany(p => p.ProductsTbls)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__products___shopI__59FA5E80");
        });

        modelBuilder.Entity<ShopsTbl>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__shops_tb__E5C424DC9E9CDB1B");

            entity.ToTable("shops_tbl");

            entity.Property(e => e.ShopId).HasColumnName("shopId");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasDefaultValue("")
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("phone");
        });

        modelBuilder.Entity<UserTbl>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__user_tbl__CB9A1CFFF6B9DF98");

            entity.ToTable("user_tbl");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasDefaultValue("")
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("fullName");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("member")
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
        });

        modelBuilder.Entity<WaterParametersTbl>(entity =>
        {
            entity.HasKey(e => e.ParameterId).HasName("PK__water_pa__F762666B73576B81");

            entity.ToTable("water_parameters_tbl");

            entity.Property(e => e.ParameterId).HasColumnName("parameterId");
            entity.Property(e => e.Co2Level)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("co2Level");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.GhLevel)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("ghLevel");
            entity.Property(e => e.KhLevel)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("khLevel");
            entity.Property(e => e.Nh4Level)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("nh4Level");
            entity.Property(e => e.No2Level)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("no2Level");
            entity.Property(e => e.No3Level)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("no3Level");
            entity.Property(e => e.Note)
                .HasColumnType("text")
                .HasColumnName("note");
            entity.Property(e => e.O2Level)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("o2Level");
            entity.Property(e => e.OutDoorTemp)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("outDoorTemp");
            entity.Property(e => e.PhLevel)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("phLevel");
            entity.Property(e => e.Po4Level)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("po4Level");
            entity.Property(e => e.PondId).HasColumnName("pondId");
            entity.Property(e => e.Salt)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("salt");
            entity.Property(e => e.Temperature)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("temperature");
            entity.Property(e => e.TotalChlorines)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("totalChlorines");

            entity.HasOne(d => d.Pond).WithMany(p => p.WaterParametersTbls)
                .HasForeignKey(d => d.PondId)
                .HasConstraintName("FK__water_par__pondI__47DBAE45");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

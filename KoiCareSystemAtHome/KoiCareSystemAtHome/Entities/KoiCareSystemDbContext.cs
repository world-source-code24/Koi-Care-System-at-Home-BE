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

    public virtual DbSet<AccountTbl> AccountTbls { get; set; }

    public virtual DbSet<CartTbl> CartTbls { get; set; }

    public virtual DbSet<KoiGrowthChartsTbl> KoiGrowthChartsTbls { get; set; }

    public virtual DbSet<KoisTbl> KoisTbls { get; set; }

    public virtual DbSet<NotesTbl> NotesTbls { get; set; }

    public virtual DbSet<OrderDetailsTbl> OrderDetailsTbls { get; set; }

    public virtual DbSet<OrdersTbl> OrdersTbls { get; set; }

    public virtual DbSet<PondsTbl> PondsTbls { get; set; }

    public virtual DbSet<ProductsTbl> ProductsTbls { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<ShopsTbl> ShopsTbls { get; set; }

    public virtual DbSet<WaterParametersTbl> WaterParametersTbls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration.GetConnectionString("KoiCareSystem");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountTbl>(entity =>
        {
            entity.HasKey(e => e.AccId).HasName("PK__account___A471AFDA79B571DC");

            entity.ToTable("account_tbl");

            entity.Property(e => e.AccId).HasColumnName("accId");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasDefaultValue("")
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValue("")
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
            entity.Property(e => e.Role)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Guest")
                .HasColumnName("role");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
        });

        modelBuilder.Entity<CartTbl>(entity =>
        {
            entity.HasKey(e => new { e.AccId, e.ProductId }).HasName("PK__cart_tbl__16A0A2CCC0105883");

            entity.ToTable("cart_tbl");

            entity.Property(e => e.AccId).HasColumnName("accId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");

            entity.HasOne(d => d.Acc).WithMany(p => p.CartTbls)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__cart_tbl__accId__60A75C0F");

            entity.HasOne(d => d.Product).WithMany(p => p.CartTbls)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__cart_tbl__produc__619B8048");
        });

        modelBuilder.Entity<KoiGrowthChartsTbl>(entity =>
        {
            entity.HasKey(e => e.ChartId).HasName("PK__koi_grow__D7FFC8C2FB9971B1");

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
                .HasConstraintName("FK__koi_growt__koiId__4AB81AF0");
        });

        modelBuilder.Entity<KoisTbl>(entity =>
        {
            entity.HasKey(e => e.KoiId).HasName("PK__kois_tbl__915924CF8B764D37");

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
                .HasConstraintName("FK__kois_tbl__pondId__47DBAE45");
        });

        modelBuilder.Entity<NotesTbl>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PK__notes_tb__03C97EFD89962C9B");

            entity.ToTable("notes_tbl");

            entity.Property(e => e.NoteId).HasColumnName("noteId");
            entity.Property(e => e.AccId).HasColumnName("accId");
            entity.Property(e => e.NoteName)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("noteName");
            entity.Property(e => e.NoteText)
                .HasColumnType("text")
                .HasColumnName("noteText");

            entity.HasOne(d => d.Acc).WithMany(p => p.NotesTbls)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__notes_tbl__accId__3F466844");
        });

        modelBuilder.Entity<OrderDetailsTbl>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__order_de__BAD83E4B01FBF199");

            entity.ToTable("order_details_tbl");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalPrice");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetailsTbls)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__order_det__order__59063A47");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetailsTbls)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__order_det__produ__59FA5E80");
        });

        modelBuilder.Entity<OrdersTbl>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders_t__0809335D900CFDD3");

            entity.ToTable("orders_tbl");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.AccId).HasColumnName("accId");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.StatusOrder)
                .HasMaxLength(100)
                .HasColumnName("statusOrder");
            entity.Property(e => e.StatusPayment)
                .HasMaxLength(100)
                .HasColumnName("statusPayment");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalAmount");

            entity.HasOne(d => d.Acc).WithMany(p => p.OrdersTbls)
                .HasForeignKey(d => d.AccId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__orders_tb__accId__5165187F");
        });

        modelBuilder.Entity<PondsTbl>(entity =>
        {
            entity.HasKey(e => e.PondId).HasName("PK__ponds_tb__74327499BA73CDE4");

            entity.ToTable("ponds_tbl");

            entity.Property(e => e.PondId).HasColumnName("pondId");
            entity.Property(e => e.AccId).HasColumnName("accId");
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
            entity.Property(e => e.Volume).HasColumnName("volume");

            entity.HasOne(d => d.Acc).WithMany(p => p.PondsTbls)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__ponds_tbl__accId__4222D4EF");
        });

        modelBuilder.Entity<ProductsTbl>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__2D10D16AAFCC94D2");

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
                .HasConstraintName("FK__products___shopI__5629CD9C");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__refresh___AC16DB470AEAE1D2");

            entity.ToTable("refresh_token");

            entity.Property(e => e.TokenId)
                .HasMaxLength(100)
                .HasColumnName("tokenId");
            entity.Property(e => e.AccId).HasColumnName("accId");
            entity.Property(e => e.ExpiredAt)
                .HasColumnType("datetime")
                .HasColumnName("expiredAt");
            entity.Property(e => e.IsRevoked).HasColumnName("isRevoked");
            entity.Property(e => e.IsUsed).HasColumnName("isUsed");
            entity.Property(e => e.IssueAt)
                .HasColumnType("datetime")
                .HasColumnName("issueAt");
            entity.Property(e => e.JwtId)
                .HasMaxLength(100)
                .HasColumnName("jwtId");
            entity.Property(e => e.Token)
                .HasMaxLength(100)
                .HasColumnName("token");

            entity.HasOne(d => d.Acc).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.AccId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__refresh_t__accId__5CD6CB2B");
        });

        modelBuilder.Entity<ShopsTbl>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__shops_tb__E5C424DC04FD0A7D");

            entity.ToTable("shops_tbl");

            entity.Property(e => e.ShopId).HasColumnName("shopId");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasDefaultValue("")
                .HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("phone");
        });

        modelBuilder.Entity<WaterParametersTbl>(entity =>
        {
            entity.HasKey(e => e.ParameterId).HasName("PK__water_pa__F762666B772C743A");

            entity.ToTable("water_parameters_tbl");

            entity.Property(e => e.ParameterId).HasColumnName("parameterId");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
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
                .HasConstraintName("FK__water_par__pondI__44FF419A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

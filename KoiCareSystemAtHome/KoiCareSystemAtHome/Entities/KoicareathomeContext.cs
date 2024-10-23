using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Entities;

public partial class KoicareathomeContext : DbContext
{
    public KoicareathomeContext()
    {
    }

    public KoicareathomeContext(DbContextOptions<KoicareathomeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountTbl> AccountTbls { get; set; }

    public virtual DbSet<CartTbl> CartTbls { get; set; }

    public virtual DbSet<KoiGrowthChartsTbl> KoiGrowthChartsTbls { get; set; }

    public virtual DbSet<KoisTbl> KoisTbls { get; set; }

    public virtual DbSet<MembershipDashboard> MembershipDashboards { get; set; }

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
        => optionsBuilder.UseSqlServer("Data Source=koicaresever.database.windows.net,1433;Initial Catalog=koicareathome;Persist Security Info=True;User ID=mysql;Password=minh0123456789.,;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountTbl>(entity =>
        {
            entity.HasKey(e => e.AccId).HasName("PK__account___A471AFDA953244D3");

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
            entity.HasKey(e => new { e.AccId, e.ProductId }).HasName("PK__cart_tbl__16A0A2CC8AED831E");

            entity.ToTable("cart_tbl");

            entity.Property(e => e.AccId).HasColumnName("accId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");

            entity.HasOne(d => d.Acc).WithMany(p => p.CartTbls)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__cart_tbl__accId__7C4F7684");

            entity.HasOne(d => d.Product).WithMany(p => p.CartTbls)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__cart_tbl__produc__7D439ABD");
        });

        modelBuilder.Entity<KoiGrowthChartsTbl>(entity =>
        {
            entity.HasKey(e => e.ChartId).HasName("PK__koi_grow__D7FFC8C2B4975265");

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
                .HasConstraintName("FK__koi_growt__koiId__7E37BEF6");
        });

        modelBuilder.Entity<KoisTbl>(entity =>
        {
            entity.HasKey(e => e.KoiId).HasName("PK__kois_tbl__915924CFF0B0C732");

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
                .HasConstraintName("FK__kois_tbl__pondId__7F2BE32F");
        });

        modelBuilder.Entity<MembershipDashboard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__membersh__3213E83F6CF4F344");

            entity.ToTable("membership_dashboard");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccId).HasColumnName("accId");
            entity.Property(e => e.Money)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("money");
            entity.Property(e => e.StartDate).HasColumnName("startDate");

            entity.HasOne(d => d.Acc).WithMany(p => p.MembershipDashboards)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__membershi__accId__160F4887");
        });

        modelBuilder.Entity<NotesTbl>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PK__notes_tb__03C97EFDA4453DE2");

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
                .HasConstraintName("FK__notes_tbl__accId__00200768");
        });

        modelBuilder.Entity<OrderDetailsTbl>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__order_de__BAD83E4BD242818A");

            entity.ToTable("order_details_tbl");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalPrice");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetailsTbls)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__order_det__order__01142BA1");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetailsTbls)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__order_det__produ__02084FDA");
        });

        modelBuilder.Entity<OrdersTbl>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders_t__0809335D73547A87");

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
                .HasConstraintName("FK__orders_tb__accId__02FC7413");
        });

        modelBuilder.Entity<PondsTbl>(entity =>
        {
            entity.HasKey(e => e.PondId).HasName("PK__ponds_tb__7432749998457A7C");

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
                .HasConstraintName("FK__ponds_tbl__accId__03F0984C");
        });

        modelBuilder.Entity<ProductsTbl>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__2D10D16A906D5970");

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
                .HasConstraintName("FK__products___shopI__04E4BC85");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__refresh___AC16DB47EBF9FE16");

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
                .HasConstraintName("FK__refresh_t__accId__05D8E0BE");
        });

        modelBuilder.Entity<ShopsTbl>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__shops_tb__E5C424DC641D9AC5");

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
            entity.HasKey(e => e.ParameterId).HasName("PK__water_pa__F762666B9BCD2DBE");

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
                .HasConstraintName("FK__water_par__pondI__06CD04F7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

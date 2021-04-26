using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BuyFood_Template.Models
{
    public partial class 擺腹BuyFoodContext : DbContext
    {
        public 擺腹BuyFoodContext()
        {
        }

        public 擺腹BuyFoodContext(DbContextOptions<擺腹BuyFoodContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RandomVieww> RandomViewws { get; set; }
        public virtual DbSet<TActivity> TActivities { get; set; }
        public virtual DbSet<TBoard> TBoards { get; set; }
        public virtual DbSet<TChatRoom> TChatRooms { get; set; }
        public virtual DbSet<TCombo> TCombos { get; set; }
        public virtual DbSet<TComboDetail> TComboDetails { get; set; }
        public virtual DbSet<TCupon> TCupons { get; set; }
        public virtual DbSet<TCuponCategory> TCuponCategories { get; set; }
        public virtual DbSet<TDeposit> TDeposits { get; set; }
        public virtual DbSet<TEatPeriod> TEatPeriods { get; set; }
        public virtual DbSet<TIsOnSale> TIsOnSales { get; set; }
        public virtual DbSet<TLoginRecord> TLoginRecords { get; set; }
        public virtual DbSet<TMember> TMembers { get; set; }
        public virtual DbSet<TMessageOrder> TMessageOrders { get; set; }
        public virtual DbSet<TNotifyList> TNotifyLists { get; set; }
        public virtual DbSet<TOrder> TOrders { get; set; }
        public virtual DbSet<TOrderDetail> TOrderDetails { get; set; }
        public virtual DbSet<TOrderStatus> TOrderStatuses { get; set; }
        public virtual DbSet<TPayType> TPayTypes { get; set; }
        public virtual DbSet<TProduct> TProducts { get; set; }
        public virtual DbSet<TProductCategory> TProductCategories { get; set; }
        public virtual DbSet<TProductTag> TProductTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:buyfoodmsit129.database.windows.net,1433;Initial Catalog=擺腹BuyFood;Persist Security Info=False;User ID=SQLAdmin;Password=@Abcdef1234@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<RandomVieww>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RandomVieww");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<TActivity>(entity =>
            {
                entity.HasKey(e => e.CActivityId)
                    .HasName("PK_產品活動資料表");

                entity.ToTable("tActivities");

                entity.Property(e => e.CActivityId).HasColumnName("cActivityID");

                entity.Property(e => e.CActivityName)
                    .HasMaxLength(50)
                    .HasColumnName("cActivityName");

                entity.Property(e => e.CDescription).HasColumnName("cDescription");

                entity.Property(e => e.CLink)
                    .HasMaxLength(255)
                    .HasColumnName("cLink");

                entity.Property(e => e.CPicture)
                    .HasMaxLength(255)
                    .HasColumnName("cPicture");

                entity.Property(e => e.CRank).HasColumnName("cRank");

                entity.Property(e => e.CStatus).HasColumnName("cStatus");

                entity.Property(e => e.CTime)
                    .HasMaxLength(50)
                    .HasColumnName("cTime");
            });

            modelBuilder.Entity<TBoard>(entity =>
            {
                entity.HasKey(e => e.CBoardId)
                    .HasName("PK_留言版資料表");

                entity.ToTable("tBoards");

                entity.Property(e => e.CBoardId).HasColumnName("cBoardID");

                entity.Property(e => e.CBoardTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cBoardTime");

                entity.Property(e => e.CBordStatus)
                    .HasMaxLength(50)
                    .HasColumnName("cBordStatus");

                entity.Property(e => e.CContent)
                    .HasMaxLength(50)
                    .HasColumnName("cContent");

                entity.Property(e => e.CGrades)
                    .HasColumnType("money")
                    .HasColumnName("cGrades");

                entity.Property(e => e.CMemberId).HasColumnName("cMemberID");

                entity.Property(e => e.CPicture)
                    .HasMaxLength(255)
                    .HasColumnName("cPicture");

                entity.Property(e => e.CProductId).HasColumnName("cProductID");

                entity.HasOne(d => d.CMember)
                    .WithMany(p => p.TBoards)
                    .HasForeignKey(d => d.CMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_留言版資料表_會員資料表");

                entity.HasOne(d => d.CProduct)
                    .WithMany(p => p.TBoards)
                    .HasForeignKey(d => d.CProductId)
                    .HasConstraintName("FK_留言版資料表_產品資料表1");
            });

            modelBuilder.Entity<TChatRoom>(entity =>
            {
                entity.HasKey(e => e.CChatRoomId);

                entity.ToTable("tChatRoom");

                entity.Property(e => e.CChatRoomId).HasColumnName("cChatRoomID");

                entity.Property(e => e.CContent)
                    .HasMaxLength(50)
                    .HasColumnName("cContent");

                entity.Property(e => e.CDifRoomId).HasColumnName("cDifRoomID");

                entity.Property(e => e.CMemberId).HasColumnName("cMemberID");

                entity.Property(e => e.CMessageTime)
                    .HasMaxLength(50)
                    .HasColumnName("cMessageTime");

                entity.Property(e => e.CPhoto)
                    .HasMaxLength(255)
                    .HasColumnName("cPhoto");

                entity.Property(e => e.CReview).HasColumnName("cReview");

                entity.Property(e => e.CSaveTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cSaveTime");

                entity.HasOne(d => d.CMember)
                    .WithMany(p => p.TChatRooms)
                    .HasForeignKey(d => d.CMemberId)
                    .HasConstraintName("FK_tChatRoom_tMembers");
            });

            modelBuilder.Entity<TCombo>(entity =>
            {
                entity.HasKey(e => e.CComboId)
                    .HasName("PK_tCombo");

                entity.ToTable("tCombos");

                entity.Property(e => e.CComboId).HasColumnName("cComboID");

                entity.Property(e => e.CComboName)
                    .HasMaxLength(50)
                    .HasColumnName("cComboName");

                entity.Property(e => e.CMemberId).HasColumnName("cMemberID");

                entity.HasOne(d => d.CMember)
                    .WithMany(p => p.TCombos)
                    .HasForeignKey(d => d.CMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tCombos_tMembers");
            });

            modelBuilder.Entity<TComboDetail>(entity =>
            {
                entity.HasKey(e => e.CComboDetailId)
                    .HasName("PK_tComboDetail");

                entity.ToTable("tComboDetails");

                entity.Property(e => e.CComboDetailId).HasColumnName("cComboDetailID");

                entity.Property(e => e.CComboId).HasColumnName("cComboID");

                entity.Property(e => e.CProductId).HasColumnName("cProductID");

                entity.HasOne(d => d.CCombo)
                    .WithMany(p => p.TComboDetails)
                    .HasForeignKey(d => d.CComboId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tComboDetails_tCombos");

                entity.HasOne(d => d.CProduct)
                    .WithMany(p => p.TComboDetails)
                    .HasForeignKey(d => d.CProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tComboDetails_tProducts");
            });

            modelBuilder.Entity<TCupon>(entity =>
            {
                entity.HasKey(e => e.CCuponId)
                    .HasName("PK_折價卷資料表");

                entity.ToTable("tCupons");

                entity.Property(e => e.CCuponId).HasColumnName("cCuponID");

                entity.Property(e => e.CBeUsed).HasColumnName("cBeUsed");

                entity.Property(e => e.CCuponCategoryId).HasColumnName("cCuponCategoryID");

                entity.Property(e => e.CDiscountCode)
                    .HasMaxLength(20)
                    .HasColumnName("cDiscountCode")
                    .IsFixedLength(true);

                entity.Property(e => e.CMenberId).HasColumnName("cMenberID");

                entity.Property(e => e.CReceivedTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cReceivedTime");

                entity.Property(e => e.CValidDate)
                    .HasColumnType("datetime")
                    .HasColumnName("cValidDate");

                entity.HasOne(d => d.CCuponCategory)
                    .WithMany(p => p.TCupons)
                    .HasForeignKey(d => d.CCuponCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_折價卷資料表_折價卷類型資料表");

                entity.HasOne(d => d.CMenber)
                    .WithMany(p => p.TCupons)
                    .HasForeignKey(d => d.CMenberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_折價卷資料表_會員資料表");
            });

            modelBuilder.Entity<TCuponCategory>(entity =>
            {
                entity.HasKey(e => e.CCuponCategoryId)
                    .HasName("PK_折價卷類型資料表");

                entity.ToTable("tCuponCategories");

                entity.Property(e => e.CCuponCategoryId).HasColumnName("cCuponCategoryID");

                entity.Property(e => e.CCutPrice)
                    .HasColumnType("money")
                    .HasColumnName("cCutPrice");

                entity.Property(e => e.CategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<TDeposit>(entity =>
            {
                entity.HasKey(e => e.CDepositId)
                    .HasName("PK_儲值資料表");

                entity.ToTable("tDeposits");

                entity.Property(e => e.CDepositId).HasColumnName("cDepositID");

                entity.Property(e => e.CDepositAmount)
                    .HasColumnType("money")
                    .HasColumnName("cDepositAmount");

                entity.Property(e => e.CDepositRecordNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cDepositRecordNo");

                entity.Property(e => e.CDepositTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cDepositTime");

                entity.Property(e => e.CMemberId).HasColumnName("cMemberID");

                entity.HasOne(d => d.CMember)
                    .WithMany(p => p.TDeposits)
                    .HasForeignKey(d => d.CMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_儲值資料表_會員資料表");
            });

            modelBuilder.Entity<TEatPeriod>(entity =>
            {
                entity.HasKey(e => e.CEatPeriodId)
                    .HasName("PK_各時段餐點資料表");

                entity.ToTable("tEatPeriods");

                entity.Property(e => e.CEatPeriodId).HasColumnName("cEatPeriodID");

                entity.Property(e => e.CEatPeriodName)
                    .HasMaxLength(16)
                    .HasColumnName("cEatPeriodName");
            });

            modelBuilder.Entity<TIsOnSale>(entity =>
            {
                entity.HasKey(e => e.CIsOnSaleId)
                    .HasName("PK_上架下架資料表");

                entity.ToTable("tIsOnSale");

                entity.Property(e => e.CIsOnSaleId).HasColumnName("cIsOnSaleID");

                entity.Property(e => e.CStatusName)
                    .HasMaxLength(50)
                    .HasColumnName("cStatusName");
            });

            modelBuilder.Entity<TLoginRecord>(entity =>
            {
                entity.HasKey(e => e.CLoginRecordId)
                    .HasName("PK_登入記錄表");

                entity.ToTable("tLoginRecords");

                entity.Property(e => e.CLoginRecordId).HasColumnName("cLoginRecordID");

                entity.Property(e => e.CIp)
                    .HasMaxLength(50)
                    .HasColumnName("cIP");

                entity.Property(e => e.CIsLoginSuccess)
                    .HasMaxLength(16)
                    .HasColumnName("cIsLoginSuccess");

                entity.Property(e => e.CLoginTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cLoginTime");

                entity.Property(e => e.CMemberId).HasColumnName("cMemberID");

                entity.HasOne(d => d.CMember)
                    .WithMany(p => p.TLoginRecords)
                    .HasForeignKey(d => d.CMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_登入記錄表_會員資料表");
            });

            modelBuilder.Entity<TMember>(entity =>
            {
                entity.HasKey(e => e.CMemberId)
                    .HasName("PK_會員資料表");

                entity.ToTable("tMembers");

                entity.Property(e => e.CMemberId).HasColumnName("cMemberID");

                entity.Property(e => e.CAddress)
                    .HasMaxLength(50)
                    .HasColumnName("cAddress");

                entity.Property(e => e.CAge)
                    .HasColumnType("date")
                    .HasColumnName("cAge");

                entity.Property(e => e.CBlackList).HasColumnName("cBlackList");

                entity.Property(e => e.CDeposit)
                    .HasColumnType("money")
                    .HasColumnName("cDeposit");

                entity.Property(e => e.CEmail)
                    .HasMaxLength(50)
                    .HasColumnName("cEmail");

                entity.Property(e => e.CFacebookId)
                    .HasMaxLength(50)
                    .HasColumnName("cFacebookID");

                entity.Property(e => e.CFreezeCount).HasColumnName("cFreezeCount");

                entity.Property(e => e.CGender)
                    .HasMaxLength(50)
                    .HasColumnName("cGender");

                entity.Property(e => e.CName)
                    .HasMaxLength(50)
                    .HasColumnName("cName");

                entity.Property(e => e.COpenMember).HasColumnName("cOpenMember");

                entity.Property(e => e.CPassword)
                    .HasMaxLength(50)
                    .HasColumnName("cPassword");

                entity.Property(e => e.CPhone)
                    .HasMaxLength(50)
                    .HasColumnName("cPhone");

                entity.Property(e => e.CPicture)
                    .HasMaxLength(255)
                    .HasColumnName("cPicture");

                entity.Property(e => e.CReferrerCode)
                    .HasMaxLength(50)
                    .HasColumnName("cReferrerCode");

                entity.Property(e => e.CReferrerId).HasColumnName("cReferrerID");

                entity.Property(e => e.CRegisteredTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cRegisteredTime");
            });

            modelBuilder.Entity<TMessageOrder>(entity =>
            {
                entity.HasKey(e => e.CMessageOrderId);

                entity.ToTable("tMessageOrder");

                entity.Property(e => e.CMessageOrderId).HasColumnName("cMessageOrderID");

                entity.Property(e => e.CIsRead).HasColumnName("cIsRead");

                entity.Property(e => e.CMessageOrder)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cMessageOrder");

                entity.Property(e => e.CSaveTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cSaveTime");

                entity.Property(e => e.CUserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cUserName");
            });

            modelBuilder.Entity<TNotifyList>(entity =>
            {
                entity.HasKey(e => e.CNotifyId)
                    .HasName("PK_tFavoriteList");

                entity.ToTable("tNotifyList");

                entity.Property(e => e.CNotifyId).HasColumnName("cNotifyID");

                entity.Property(e => e.CEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cEmail");

                entity.Property(e => e.CIsSend).HasColumnName("cIsSend");

                entity.Property(e => e.CProductId).HasColumnName("cProductID");

                entity.HasOne(d => d.CProduct)
                    .WithMany(p => p.TNotifyLists)
                    .HasForeignKey(d => d.CProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tNotifyList_tProducts");
            });

            modelBuilder.Entity<TOrder>(entity =>
            {
                entity.HasKey(e => e.COrderId)
                    .HasName("PK_訂單資料表");

                entity.ToTable("tOrders");

                entity.Property(e => e.COrderId).HasColumnName("cOrderID");

                entity.Property(e => e.CArrivedAddress)
                    .HasMaxLength(50)
                    .HasColumnName("cArrivedAddress");

                entity.Property(e => e.CCuponId).HasColumnName("cCuponID");

                entity.Property(e => e.CMemberId).HasColumnName("cMemberID");

                entity.Property(e => e.COrderDate)
                    .HasMaxLength(50)
                    .HasColumnName("cOrderDate");

                entity.Property(e => e.COrderStatusId).HasColumnName("cOrderStatusID");

                entity.Property(e => e.CPayTypeId).HasColumnName("cPayTypeID");

                entity.Property(e => e.CTransportMinute).HasColumnName("cTransportMinute");

                entity.HasOne(d => d.CCupon)
                    .WithMany(p => p.TOrders)
                    .HasForeignKey(d => d.CCuponId)
                    .HasConstraintName("FK_訂單資料表_折價卷資料表");

                entity.HasOne(d => d.CMember)
                    .WithMany(p => p.TOrders)
                    .HasForeignKey(d => d.CMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_訂單資料表_會員資料表");

                entity.HasOne(d => d.COrderStatus)
                    .WithMany(p => p.TOrders)
                    .HasForeignKey(d => d.COrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_訂單資料表_訂單狀況資料表");

                entity.HasOne(d => d.CPayType)
                    .WithMany(p => p.TOrders)
                    .HasForeignKey(d => d.CPayTypeId)
                    .HasConstraintName("FK_tOrders_tPayType");
            });

            modelBuilder.Entity<TOrderDetail>(entity =>
            {
                entity.HasKey(e => e.COrderDetailId)
                    .HasName("PK_訂單明細資料表");

                entity.ToTable("tOrderDetails");

                entity.Property(e => e.COrderDetailId).HasColumnName("cOrderDetailID");

                entity.Property(e => e.CFeedBackStatus).HasColumnName("cFeedBackStatus");

                entity.Property(e => e.COrderId).HasColumnName("cOrderID");

                entity.Property(e => e.CPriceAtTheTime)
                    .HasColumnType("money")
                    .HasColumnName("cPriceAtTheTime");

                entity.Property(e => e.CProductId).HasColumnName("cProductID");

                entity.Property(e => e.CQuantity).HasColumnName("cQuantity");

                entity.Property(e => e.CReview)
                    .HasMaxLength(50)
                    .HasColumnName("cReview");

                entity.Property(e => e.CScores).HasColumnName("cScores");

                entity.HasOne(d => d.COrder)
                    .WithMany(p => p.TOrderDetails)
                    .HasForeignKey(d => d.COrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_訂單明細資料表_訂單資料表");

                entity.HasOne(d => d.CProduct)
                    .WithMany(p => p.TOrderDetails)
                    .HasForeignKey(d => d.CProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tOrderDetails_tProducts");
            });

            modelBuilder.Entity<TOrderStatus>(entity =>
            {
                entity.HasKey(e => e.COrderStatusId)
                    .HasName("PK_訂單狀況資料表");

                entity.ToTable("tOrderStatus");

                entity.Property(e => e.COrderStatusId).HasColumnName("cOrderStatusID");

                entity.Property(e => e.COrderStatusName)
                    .HasMaxLength(50)
                    .HasColumnName("cOrderStatusName");
            });

            modelBuilder.Entity<TPayType>(entity =>
            {
                entity.HasKey(e => e.CPayTypeId);

                entity.ToTable("tPayType");

                entity.Property(e => e.CPayTypeId).HasColumnName("cPayTypeID");

                entity.Property(e => e.CPayTypeName)
                    .HasMaxLength(50)
                    .HasColumnName("cPayTypeName");
            });

            modelBuilder.Entity<TProduct>(entity =>
            {
                entity.HasKey(e => e.CProductId)
                    .HasName("PK_產品資料表");

                entity.ToTable("tProducts");

                entity.Property(e => e.CProductId).HasColumnName("cProductID");

                entity.Property(e => e.CCategoryId).HasColumnName("cCategoryID");

                entity.Property(e => e.CDescription).HasColumnName("cDescription");

                entity.Property(e => e.CEatTimeId).HasColumnName("cEatTimeID");

                entity.Property(e => e.CFinishedTime).HasColumnName("cFinishedTime");

                entity.Property(e => e.CIsBreakFast).HasColumnName("cIsBreakFast");

                entity.Property(e => e.CIsDinner).HasColumnName("cIsDinner");

                entity.Property(e => e.CIsLunch).HasColumnName("cIsLunch");

                entity.Property(e => e.CIsOnSaleId).HasColumnName("cIsOnSaleID");

                entity.Property(e => e.CPicture)
                    .HasMaxLength(255)
                    .HasColumnName("cPicture");

                entity.Property(e => e.CPrice)
                    .HasColumnType("money")
                    .HasColumnName("cPrice");

                entity.Property(e => e.CProductName)
                    .HasMaxLength(50)
                    .HasColumnName("cProductName");

                entity.Property(e => e.CProductTagId).HasColumnName("cProductTagID");

                entity.Property(e => e.CQuantity).HasColumnName("cQuantity");

                entity.Property(e => e.CQuantityControl).HasColumnName("cQuantityControl");

                entity.HasOne(d => d.CCategory)
                    .WithMany(p => p.TProducts)
                    .HasForeignKey(d => d.CCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tProducts_tProductCategories");

                entity.HasOne(d => d.CProductTag)
                    .WithMany(p => p.TProducts)
                    .HasForeignKey(d => d.CProductTagId)
                    .HasConstraintName("FK_tProducts_tProductTag");
            });

            modelBuilder.Entity<TProductCategory>(entity =>
            {
                entity.HasKey(e => e.CProductCategoryId)
                    .HasName("PK_產品種類資料表");

                entity.ToTable("tProductCategories");

                entity.Property(e => e.CProductCategoryId).HasColumnName("cProductCategoryID");

                entity.Property(e => e.CCategoryName)
                    .HasMaxLength(50)
                    .HasColumnName("cCategoryName");
            });

            modelBuilder.Entity<TProductTag>(entity =>
            {
                entity.HasKey(e => e.CProductTagId);

                entity.ToTable("tProductTag");

                entity.Property(e => e.CProductTagId).HasColumnName("cProductTagID");

                entity.Property(e => e.CProductTagName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cProductTagName")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using Microsoft.EntityFrameworkCore;
using AppointmentJournal.AppCore;

#nullable disable

namespace AppointmentJournal.AppDatabase
{
    public partial class AppointmentJournalContext : DbContext, IAppointmentJournalContext
    {
        public AppointmentJournalContext()
        {
        }
        
        public AppointmentJournalContext(
            DbContextOptions<AppointmentJournalContext> options,
            IConfig config)
            : base(options)
        {
            Database.SetConnectionString(config.SqlSettings.AppConnectionString);
            Database.EnsureCreated();
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServicesCategory> ServicesCategories { get; set; }
        public virtual DbSet<WorkDay> WorkDays { get; set; }
        public virtual DbSet<WorkDaysTimeSpan> WorkDaysTimeSpans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddressValue).IsRequired();

                entity.Property(e => e.ProducerId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("ProducerID");
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasIndex(e => e.ServiceId, "IX_Appointments_ServiceID");

                entity.HasIndex(e => e.WorkDayTimeSpanId, "IX_Appointments_WorkDayID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConsumerId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("ConsumerID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.Property(e => e.WorkDayTimeSpanId).HasColumnName("WorkDayTimeSpanID");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointments_Services");

                entity.HasOne(d => d.WorkDayTimeSpan)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.WorkDayTimeSpanId)
                    .HasConstraintName("FK_Appointments_WorkDaysTimeSpans");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasIndex(e => e.CategoryId, "IX_Services_CategoryID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ProducerId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("ProducerID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Services_ServicesCategories");
            });

            modelBuilder.Entity<ServicesCategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<WorkDay>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ProducerId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("ProducerID");
            });

            modelBuilder.Entity<WorkDaysTimeSpan>(entity =>
            {
                entity.HasIndex(e => e.AddressId, "IX_WorkDaysTimeSpans_AddressID");

                entity.HasIndex(e => e.ServiceId, "IX_WorkDaysTimeSpans_ServiceID");

                entity.HasIndex(e => e.WorkDayId, "IX_WorkDaysTimeSpans_WorkDayID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.BeginTime).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.WorkDayId).HasColumnName("WorkDayID");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.WorkDaysTimeSpans)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_WorkDaysTimeSpans_Addresses");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.WorkDaysTimeSpans)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_WorkDaysTimeSpans_Services");

                entity.HasOne(d => d.WorkDay)
                    .WithMany(p => p.WorkDaysTimeSpans)
                    .HasForeignKey(d => d.WorkDayId)
                    .HasConstraintName("FK_WorkDaysTimeSpans_WorkDays");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
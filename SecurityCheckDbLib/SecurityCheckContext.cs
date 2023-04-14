using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SecurityCheckDbLib;

public partial class SecurityCheckContext : DbContext
{
    public SecurityCheckContext()
    {
    }

    public SecurityCheckContext(DbContextOptions<SecurityCheckContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Criticality> Criticalities { get; set; }

    public virtual DbSet<Criticism> Criticisms { get; set; }

    public virtual DbSet<CriticismType> CriticismTypes { get; set; }

    public virtual DbSet<CustomerSurvey> CustomerSurveys { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Questionnaire> Questionnaires { get; set; }

    public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }

    public virtual DbSet<UserName> UserNames { get; set; }

    public virtual DbSet<UserQuestionnaire> UserQuestionnaires { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(LocalDB)\\mssqllocaldb;attachdbfilename=C:\\Users\\kogle\\Desktop\\HTL#\\2022-2023\\SYP\\Projekt\\ProjektWithDb\\Infotech\\SecurityCheckDbLib\\SecurityCheckDatabase.mdf;integrated security=True;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.ToTable("Answer");

            entity.Property(e => e.AnswerText)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Answer_Question");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryText)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Criticality>(entity =>
        {
            entity.ToTable("Criticality");

            entity.Property(e => e.CriticalityText)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Criticism>(entity =>
        {
            entity.ToTable("Criticism");

            entity.Property(e => e.CriticismText)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.CriticismType).WithMany(p => p.Criticisms)
                .HasForeignKey(d => d.CriticismTypeId)
                .HasConstraintName("FK_Criticism_CriticismType");

            entity.HasOne(d => d.Question).WithMany(p => p.Criticisms)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Criticism_Question");
        });

        modelBuilder.Entity<CriticismType>(entity =>
        {
            entity.ToTable("CriticismType");

            entity.Property(e => e.CriticismTypeText)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CustomerSurvey>(entity =>
        {
            entity.ToTable("CustomerSurvey");

            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Questionnaire).WithMany(p => p.CustomerSurveys)
                .HasForeignKey(d => d.QuestionnaireId)
                .HasConstraintName("FK_CustomerSurvey_Questionnaire");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Question");

            entity.Property(e => e.QuestionText)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Category).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Question_Category");

            entity.HasOne(d => d.Criticality).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CriticalityId)
                .HasConstraintName("FK_Question_Criticality");
        });

        modelBuilder.Entity<Questionnaire>(entity =>
        {
            entity.ToTable("Questionnaire");

            entity.HasIndex(e => e.QuestionnaireName, "UQ__Question__3042E5497FF1B8D5").IsUnique();

            entity.Property(e => e.QuestionnaireName)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SurveyQuestion>(entity =>
        {
            entity.ToTable("SurveyQuestion");

            entity.HasOne(d => d.CustomerSurvey).WithMany(p => p.SurveyQuestions)
                .HasForeignKey(d => d.CustomerSurveyId)
                .HasConstraintName("FK_SurveyQuestion_CustomerSurvey");

            entity.HasOne(d => d.Question).WithMany(p => p.SurveyQuestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_SurveyQuestion_Question");
        });

        modelBuilder.Entity<UserName>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.HasIndex(e => e.Username1, "UQ__UserName__536C85E4710471DA").IsUnique();

            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("Password_hash");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Password_salt");
            entity.Property(e => e.Username1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Username");
        });

        modelBuilder.Entity<UserQuestionnaire>(entity =>
        {
            entity.ToTable("UserQuestionnaire");

            entity.HasOne(d => d.Questionnaire).WithMany(p => p.UserQuestionnaires)
                .HasForeignKey(d => d.QuestionnaireId)
                .HasConstraintName("FK_UserQuestionnaire_Questionnaire");

            entity.HasOne(d => d.User).WithMany(p => p.UserQuestionnaires)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserQuestionnaire_UserNames");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

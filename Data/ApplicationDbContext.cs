using Microsoft.EntityFrameworkCore;
using nexus.Models;
using Oracle.EntityFrameworkCore;
using Oracle.EntityFrameworkCore.Metadata;

namespace nexus.Data
{
    /// <summary>
    /// Contexto do Entity Framework para a aplicação MindTrack
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Tabela de usuários
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; } = null!;

        /// <summary>
        /// Tabela de humor
        /// </summary>
        public DbSet<Humor> Humores { get; set; } = null!;

        /// <summary>
        /// Tabela de sprints
        /// </summary>
        public DbSet<Sprint> Sprints { get; set; } = null!;

        /// <summary>
        /// Tabela de alertas de IA
        /// </summary>
        public DbSet<AlertaIA> AlertasIA { get; set; } = null!;

        /// <summary>
        /// Tabela de hábitos
        /// </summary>
        public DbSet<Habito> Habitos { get; set; } = null!;

        /// <summary>
        /// Tabela de badges
        /// </summary>
        public DbSet<Badge> Badges { get; set; } = null!;

        /// <summary>
        /// Tabela de relação usuário-badge
        /// </summary>
        public DbSet<UsuarioBadge> UsuarioBadges { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar Oracle - sem schema padrão
            modelBuilder.HasDefaultSchema(null);
            
            // Configurar para usar nomes de tabelas e colunas em maiúsculas (padrão Oracle)
            // O Oracle converte nomes sem aspas para maiúsculas automaticamente

            // Configuração da entidade Usuario para Oracle (conforme script MindTrack)
            // Oracle converte nomes sem aspas para MAIÚSCULAS, então usamos maiúsculas aqui
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("T_MT_USUARIOS");
                entity.HasKey(e => e.IdUsuario);
                var idUsuarioProperty = entity.Property(e => e.IdUsuario)
                    .HasColumnName("ID_USUARIO");
                OraclePropertyBuilderExtensions.UseIdentityColumn(idUsuarioProperty);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100).HasColumnName("NOME");
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150).HasColumnName("EMAIL");
                entity.Property(e => e.SenhaHash).IsRequired().HasMaxLength(255).HasColumnName("SENHA_HASH");
                entity.Property(e => e.Perfil).IsRequired().HasMaxLength(20).HasColumnName("PERFIL");
                entity.Property(e => e.DataCadastro).IsRequired().HasColumnName("DATA_CADASTRO").HasDefaultValueSql("CURRENT_DATE");
                entity.Property(e => e.Empresa).HasMaxLength(100).HasColumnName("EMPRESA");

                // Índice único
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuração da entidade Humor para Oracle
            modelBuilder.Entity<Humor>(entity =>
            {
                entity.ToTable("T_MT_HUMOR");
                entity.HasKey(e => e.IdHumor);
                var idHumorProperty = entity.Property(e => e.IdHumor)
                    .HasColumnName("ID_HUMOR");
                OraclePropertyBuilderExtensions.UseIdentityColumn(idHumorProperty);
                entity.Property(e => e.IdUsuario).IsRequired().HasColumnName("ID_USUARIO");
                entity.Property(e => e.DataRegistro).IsRequired().HasColumnName("DATA_REGISTRO").HasDefaultValueSql("CURRENT_DATE");
                entity.Property(e => e.NivelHumor).IsRequired().HasColumnName("NIVEL_HUMOR");
                entity.Property(e => e.NivelEnergia).IsRequired().HasColumnName("NIVEL_ENERGIA");
                entity.Property(e => e.Comentario).HasMaxLength(255).HasColumnName("COMENTARIO");

                // Relacionamento com Usuario
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Humores)
                      .HasForeignKey(e => e.IdUsuario)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração da entidade Sprint para Oracle
            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.ToTable("T_MT_SPRINTS");
                entity.HasKey(e => e.IdSprint);
                var idSprintProperty = entity.Property(e => e.IdSprint)
                    .HasColumnName("ID_SPRINT");
                OraclePropertyBuilderExtensions.UseIdentityColumn(idSprintProperty);
                entity.Property(e => e.IdUsuario).IsRequired().HasColumnName("ID_USUARIO");
                entity.Property(e => e.NomeSprint).IsRequired().HasMaxLength(100).HasColumnName("NOME_SPRINT");
                entity.Property(e => e.DataInicio).IsRequired().HasColumnName("DATA_INICIO");
                entity.Property(e => e.DataFim).HasColumnName("DATA_FIM");
                entity.Property(e => e.Produtividade).HasColumnName("PRODUTIVIDADE");
                entity.Property(e => e.TarefasConcluidas).HasColumnName("TAREFAS_CONCLUIDAS");
                entity.Property(e => e.Commits).HasColumnName("COMMITS");

                // Relacionamento com Usuario
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Sprints)
                      .HasForeignKey(e => e.IdUsuario)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice único composto
                entity.HasIndex(e => new { e.IdUsuario, e.NomeSprint }).IsUnique();
            });

            // Configuração da entidade AlertaIA para Oracle
            modelBuilder.Entity<AlertaIA>(entity =>
            {
                entity.ToTable("T_MT_ALERTAS_IA");
                entity.HasKey(e => e.IdAlerta);
                var idAlertaProperty = entity.Property(e => e.IdAlerta)
                    .HasColumnName("ID_ALERTA");
                OraclePropertyBuilderExtensions.UseIdentityColumn(idAlertaProperty);
                entity.Property(e => e.IdUsuario).IsRequired().HasColumnName("ID_USUARIO");
                entity.Property(e => e.DataAlerta).IsRequired().HasColumnName("DATA_ALERTA").HasDefaultValueSql("CURRENT_DATE");
                entity.Property(e => e.TipoAlerta).IsRequired().HasMaxLength(50).HasColumnName("TIPO_ALERTA");
                entity.Property(e => e.Mensagem).HasMaxLength(255).HasColumnName("MENSAGEM");
                entity.Property(e => e.NivelRisco).IsRequired().HasColumnName("NIVEL_RISCO");

                // Relacionamento com Usuario
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.AlertasIA)
                      .HasForeignKey(e => e.IdUsuario)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração da entidade Habito para Oracle
            modelBuilder.Entity<Habito>(entity =>
            {
                entity.ToTable("T_MT_HABITOS");
                entity.HasKey(e => e.IdHabito);
                var idHabitoProperty = entity.Property(e => e.IdHabito)
                    .HasColumnName("ID_HABITO");
                OraclePropertyBuilderExtensions.UseIdentityColumn(idHabitoProperty);
                entity.Property(e => e.IdUsuario).IsRequired().HasColumnName("ID_USUARIO");
                entity.Property(e => e.TipoHabito).IsRequired().HasMaxLength(50).HasColumnName("TIPO_HABITO");
                entity.Property(e => e.DataHabito).IsRequired().HasColumnName("DATA_HABITO").HasDefaultValueSql("CURRENT_DATE");
                entity.Property(e => e.Pontuacao).IsRequired().HasColumnName("PONTUACAO").HasDefaultValue(0);

                // Relacionamento com Usuario
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Habitos)
                      .HasForeignKey(e => e.IdUsuario)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração da entidade Badge para Oracle
            modelBuilder.Entity<Badge>(entity =>
            {
                entity.ToTable("T_MT_BADGES");
                entity.HasKey(e => e.IdBadge);
                var idBadgeProperty = entity.Property(e => e.IdBadge)
                    .HasColumnName("ID_BADGE");
                OraclePropertyBuilderExtensions.UseIdentityColumn(idBadgeProperty);
                entity.Property(e => e.NomeBadge).IsRequired().HasMaxLength(50).HasColumnName("NOME_BADGE");
                entity.Property(e => e.Descricao).HasMaxLength(150).HasColumnName("DESCRICAO");
                entity.Property(e => e.PontosRequeridos).IsRequired().HasColumnName("PONTOS_REQUERIDOS");
            });

            // Configuração da entidade UsuarioBadge para Oracle
            modelBuilder.Entity<UsuarioBadge>(entity =>
            {
                entity.ToTable("T_MT_USUARIO_BADGES");
                entity.HasKey(e => new { e.IdUsuario, e.IdBadge });
                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
                entity.Property(e => e.IdBadge).HasColumnName("ID_BADGE");
                entity.Property(e => e.DataConquista).IsRequired().HasColumnName("DATA_CONQUISTA").HasDefaultValueSql("CURRENT_DATE");

                // Relacionamentos
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.UsuarioBadges)
                      .HasForeignKey(e => e.IdUsuario)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Badge)
                      .WithMany(b => b.UsuarioBadges)
                      .HasForeignKey(e => e.IdBadge)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

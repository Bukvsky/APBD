using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dodaj Entity Framework
builder.Services.AddDbContext<KolokwiumDbContext>(options =>
    options.UseSqlServer("Server=localhost;Database=KolokwiumDB;Trusted_Connection=true;"));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

app.Run();

// ENTITY MODELS
[Table("Institutions")]
public class Institution
{
    [Key]
    [Column("InstitutionId")]
    public int InstitutionId { get; set; }
    
    [Required]
    [Column("Name")]
    [MaxLength(200)]
    public string Name { get; set; }
    
    [Column("FoundedYear")]
    public int FoundedYear { get; set; }
    
    // Navigation properties
    public virtual ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();
}

[Table("Artifacts")]
public class Artifact
{
    [Key]
    [Column("ArtifactId")]
    public int ArtifactId { get; set; }
    
    [Required]
    [Column("Name")]
    [MaxLength(200)]
    public string Name { get; set; }
    
    [Column("OriginDate")]
    public DateTime OriginDate { get; set; }
    
    [Column("InstitutionId")]
    public int InstitutionId { get; set; }
    
    // Navigation properties
    [ForeignKey("InstitutionId")]
    public virtual Institution Institution { get; set; }
    
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}

[Table("Projects")]
public class Project
{
    [Key]
    [Column("ProjectId")]
    public int ProjectId { get; set; }
    
    [Required]
    [Column("Objective")]
    [MaxLength(500)]
    public string Objective { get; set; }
    
    [Column("StartDate")]
    public DateTime StartDate { get; set; }
    
    [Column("EndDate")]
    public DateTime? EndDate { get; set; }
    
    [Column("ArtifactId")]
    public int ArtifactId { get; set; }
    
    // Navigation properties
    [ForeignKey("ArtifactId")]
    public virtual Artifact Artifact { get; set; }
    
    public virtual ICollection<StaffAssignment> StaffAssignments { get; set; } = new List<StaffAssignment>();
}

[Table("Staff")]
public class Staff
{
    [Key]
    [Column("StaffId")]
    public int StaffId { get; set; }
    
    [Required]
    [Column("FirstName")]
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [Column("LastName")]
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [Column("HireDate")]
    public DateTime HireDate { get; set; }
    
    // Navigation properties
    public virtual ICollection<StaffAssignment> StaffAssignments { get; set; } = new List<StaffAssignment>();
}

[Table("StaffAssignments")]
public class StaffAssignment
{
    [Key]
    [Column("AssignmentId")]
    public int AssignmentId { get; set; }
    
    [Column("ProjectId")]
    public int ProjectId { get; set; }
    
    [Column("StaffId")]
    public int StaffId { get; set; }
    
    [Required]
    [Column("Role")]
    [MaxLength(100)]
    public string Role { get; set; }
    
    // Navigation properties
    [ForeignKey("ProjectId")]
    public virtual Project Project { get; set; }
    
    [ForeignKey("StaffId")]
    public virtual Staff Staff { get; set; }
}

// DTO MODELS
public class ProjectResponseDto
{
    public int ProjectId { get; set; }
    public string Objective { get; set; }
    public string StartDate { get; set; }
    public string? EndDate { get; set; }
    public ArtifactInfoDto Artifact { get; set; }
    public List<StaffAssignmentDto> StaffAssignments { get; set; }
}

public class ArtifactInfoDto
{
    public string Name { get; set; }
    public string OriginDate { get; set; }
    public InstitutionInfoDto Institution { get; set; }
}

public class InstitutionInfoDto
{
    public int InstitutionId { get; set; }
    public string Name { get; set; }
    public int FoundedYear { get; set; }
}

public class StaffAssignmentDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string HireDate { get; set; }
    public string Role { get; set; }
}

public class ArtifactRequestDto
{
    public ArtifactDataDto Artifact { get; set; }
    public ProjectDataDto Project { get; set; }
}

public class ArtifactDataDto
{
    public int ArtifactId { get; set; }
    public string Name { get; set; }
    public string OriginDate { get; set; }
    public int InstitutionId { get; set; }
}

public class ProjectDataDto
{
    public int ProjectId { get; set; }
    public string Objective { get; set; }
    public string StartDate { get; set; }
    public string? EndDate { get; set; }
}

// DBCONTEXT
public class KolokwiumDbContext : DbContext
{
    public KolokwiumDbContext(DbContextOptions<KolokwiumDbContext> options) : base(options)
    {
    }

    public DbSet<Institution> Institutions { get; set; }
    public DbSet<Artifact> Artifacts { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<StaffAssignment> StaffAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja Institution
        modelBuilder.Entity<Institution>(entity =>
        {
            entity.HasKey(e => e.InstitutionId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.FoundedYear).IsRequired();
        });

        // Konfiguracja Artifact
        modelBuilder.Entity<Artifact>(entity =>
        {
            entity.HasKey(e => e.ArtifactId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.OriginDate).IsRequired();
            
            // Relacja Artifact -> Institution
            entity.HasOne(e => e.Institution)
                  .WithMany(i => i.Artifacts)
                  .HasForeignKey(e => e.InstitutionId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Konfiguracja Project
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId);
            entity.Property(e => e.Objective).IsRequired().HasMaxLength(500);
            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.EndDate).IsRequired(false);
            
            // Relacja Project -> Artifact
            entity.HasOne(e => e.Artifact)
                  .WithMany(a => a.Projects)
                  .HasForeignKey(e => e.ArtifactId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Konfiguracja Staff
        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.HireDate).IsRequired();
        });

        // Konfiguracja StaffAssignment
        modelBuilder.Entity<StaffAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(100);
            
            // Relacja StaffAssignment -> Project
            entity.HasOne(e => e.Project)
                  .WithMany(p => p.StaffAssignments)
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Relacja StaffAssignment -> Staff
            entity.HasOne(e => e.Staff)
                  .WithMany(s => s.StaffAssignments)
                  .HasForeignKey(e => e.StaffId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Indeks unikalny dla ProjectId + StaffId
            entity.HasIndex(e => new { e.ProjectId, e.StaffId })
                  .IsUnique();
        });
    }
}

// MAPPING EXTENSIONS
public static class MappingExtensions
{
    public static ProjectResponseDto ToProjectResponseDto(this Project project)
    {
        return new ProjectResponseDto
        {
            ProjectId = project.ProjectId,
            Objective = project.Objective,
            StartDate = project.StartDate.ToString("yyyy-MM-dd"),
            EndDate = project.EndDate?.ToString("yyyy-MM-dd"),
            Artifact = project.Artifact.ToArtifactInfoDto(),
            StaffAssignments = project.StaffAssignments
                .Select(sa => sa.ToStaffAssignmentDto())
                .ToList()
        };
    }

    public static ArtifactInfoDto ToArtifactInfoDto(this Artifact artifact)
    {
        return new ArtifactInfoDto
        {
            Name = artifact.Name,
            OriginDate = artifact.OriginDate.ToString("yyyy-MM-dd"),
            Institution = artifact.Institution.ToInstitutionInfoDto()
        };
    }

    public static InstitutionInfoDto ToInstitutionInfoDto(this Institution institution)
    {
        return new InstitutionInfoDto
        {
            InstitutionId = institution.InstitutionId,
            Name = institution.Name,
            FoundedYear = institution.FoundedYear
        };
    }

    public static StaffAssignmentDto ToStaffAssignmentDto(this StaffAssignment staffAssignment)
    {
        return new StaffAssignmentDto
        {
            FirstName = staffAssignment.Staff.FirstName,
            LastName = staffAssignment.Staff.LastName,
            HireDate = staffAssignment.Staff.HireDate.ToString("yyyy-MM-dd"),
            Role = staffAssignment.Role
        };
    }

    public static Artifact ToEntity(this ArtifactDataDto dto)
    {
        return new Artifact
        {
            ArtifactId = dto.ArtifactId,
            Name = dto.Name,
            OriginDate = DateTime.Parse(dto.OriginDate),
            InstitutionId = dto.InstitutionId
        };
    }

    public static Project ToEntity(this ProjectDataDto dto, int artifactId)
    {
        return new Project
        {
            ProjectId = dto.ProjectId,
            Objective = dto.Objective,
            StartDate = DateTime.Parse(dto.StartDate),
            EndDate = string.IsNullOrEmpty(dto.EndDate) ? null : DateTime.Parse(dto.EndDate),
            ArtifactId = artifactId
        };
    }
}

// CONTROLLER
[ApiController]
[Route("api")]
public class ProjectsController : ControllerBase
{
    private readonly KolokwiumDbContext _context;

    public ProjectsController(KolokwiumDbContext context)
    {
        _context = context;
    }

    // GET /api/projects/{id}
    [HttpGet("projects/{id}")]
    public async Task<ActionResult<ProjectResponseDto>> GetProject(int id)
    {
        try
        {
            var project = await _context.Projects
                .Include(p => p.Artifact)
                    .ThenInclude(a => a.Institution)
                .Include(p => p.StaffAssignments)
                    .ThenInclude(sa => sa.Staff)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            var projectDto = project.ToProjectResponseDto();
            return Ok(projectDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST /api/artifacts
    [HttpPost("artifacts")]
    public async Task<ActionResult> CreateArtifactAndProject([FromBody] ArtifactRequestDto request)
    {
        if (request?.Artifact == null || request?.Project == null)
        {
            return BadRequest("Both artifact and project data are required.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // Sprawdź czy institution istnieje
            var institutionExists = await _context.Institutions
                .AnyAsync(i => i.InstitutionId == request.Artifact.InstitutionId);
            
            if (!institutionExists)
            {
                return BadRequest($"Institution with ID {request.Artifact.InstitutionId} does not exist.");
            }

            // Sprawdź czy artefakt już istnieje
            var artifactExists = await _context.Artifacts
                .AnyAsync(a => a.ArtifactId == request.Artifact.ArtifactId);
            
            if (artifactExists)
            {
                return Conflict($"Artifact with ID {request.Artifact.ArtifactId} already exists.");
            }

            // Sprawdź czy projekt już istnieje
            var projectExists = await _context.Projects
                .AnyAsync(p => p.ProjectId == request.Project.ProjectId);
            
            if (projectExists)
            {
                return Conflict($"Project with ID {request.Project.ProjectId} already exists.");
            }

            // Dodaj artefakt
            var artifact = request.Artifact.ToEntity();
            _context.Artifacts.Add(artifact);
            await _context.SaveChangesAsync();

            // Dodaj projekt
            var project = request.Project.ToEntity(request.Artifact.ArtifactId);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Zatwierdź transakcję
            await transaction.CommitAsync();

            return CreatedAtAction(
                nameof(GetProject),
                new { id = request.Project.ProjectId },
                new
                {
                    message = "Artifact and project created successfully",
                    artifactId = request.Artifact.ArtifactId,
                    projectId = request.Project.ProjectId
                });
        }
        catch (FormatException ex)
        {
            await transaction.RollbackAsync();
            return BadRequest($"Invalid date format: {ex.Message}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Pomocnicza końcówka do sprawdzenia połączenia z bazą danych
    [HttpGet("health")]
    public async Task<ActionResult> HealthCheck()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            return Ok(new { status = canConnect ? "Database connection successful" : "Database connection failed", timestamp = DateTime.Now });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = "Database connection failed", error = ex.Message });
        }
    }

    // Dodatkowe końcówki pomocnicze
    [HttpGet("institutions")]
    public async Task<ActionResult<List<Institution>>> GetInstitutions()
    {
        try
        {
            var institutions = await _context.Institutions.ToListAsync();
            return Ok(institutions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("artifacts")]
    public async Task<ActionResult<List<Artifact>>> GetArtifacts()
    {
        try
        {
            var artifacts = await _context.Artifacts
                .Include(a => a.Institution)
                .ToListAsync();
            return Ok(artifacts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

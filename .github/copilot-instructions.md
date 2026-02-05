# AI Copilot Instructions for SuperAbp Exam Project

## Project Architecture

This is an **online exam management system** built with **ABP Framework** and **.NET**, featuring:
- **ASP.NET Core Backend** (`aspnet-core/`) with ABP layered architecture
- **Angular Frontend**: Management (`angular-admin/`) using NG-ALAIN and User (`angular-web/`) using ABP Angular

### Backend Module Structure (ABP DDD Layering)

```
Domain → Application → HttpApi
  ↓           ↓           ↓
Entities    AppServices   Controllers
Repositories DTOs        Routes
DomainServices
```

**Key Projects:**
- `SuperAbp.Exam.Domain` - Business entities, aggregates, domain logic, repositories
- `SuperAbp.Exam.Application` - Application services, DTOs, auto-mapper profiles
- `SuperAbp.Exam.HttpApi` - REST API controllers exposed to frontend
- `SuperAbp.Exam.EntityFrameworkCore` - DB context, EF Core migrations
- `SuperAbp.Exam.Admin.Application` - Admin-specific application services
- `SuperAbp.Exam.Admin.HttpApi` - Admin-specific HTTP API controllers
- `SuperAbp.Exam.AuthServer` - OpenIddict authentication server
- `SuperAbp.Exam.DbMigrator` - Database migration utility
- `SuperAbp.Exam.BackgroundServices` - Background jobs (e.g., exam submission handling)

### Frontend Structure

**Management (`angular-admin/`):**
- Built with **NG-ALAIN** (enterprise admin framework based on ng-zorro-antd)
- Uses Less for styling
- Features: Rich forms, data tables, charts, role-based UI
- Proxy generation in `src/app/proxy/` (auto-generated, DO NOT EDIT)

**User (`angular-web/`):**
- Standard ABP Angular
- Uses SCSS for styling
- Simple exam-taking interface

## Critical Workflows

### Backend Development

```bash
# Build entire backend
cd aspnet-core
dotnet build

# Run tests
dotnet test

# Install ABP libs (required for each Host project)
cd src/SuperAbp.Exam.HttpApi.Host
abp install-libs
cd ../SuperAbp.Exam.Admin.HttpApi.Host
abp install-libs

# Run specific host
# AuthServer (OpenIddict authorization service - REQUIRED for login)
dotnet run --project src/SuperAbp.Exam.AuthServer

# User API
dotnet run --project src/SuperAbp.Exam.HttpApi.Host

# Admin API
dotnet run --project src/SuperAbp.Exam.Admin.HttpApi.Host
```

**Important:** AuthServer must be running for authentication/authorization to work. Login, OAuth flows, and token validation depend on it.

### Frontend Development

```bash
# Management admin
cd angular-admin
yarn install
npm run build        # High-memory build: node --max_old_space_size=8000
npm run start        # Dev server with proxy
npm run test         # Karma unit tests

# User web
cd angular-web
yarn install
ng serve              # Dev server
npm run build:prod    # Production build
```

### Proxy Generation (Critical)

**NEVER manually edit files in `src/app/proxy/`** - they are auto-generated from backend APIs.

To regenerate after backend changes:
```bash
cd angular-admin  # or angular-web
ng g @abp/ng.schematics:proxy
```

### Testing Patterns

**Backend Tests** use this standard pattern (see `KnowledgePointAdminAppServiceTests.cs`):

```csharp
public abstract class MyEntityAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly MyEntityRepository _repository;
    private readonly IMyEntityAppService _appService;

    protected MyEntityAppServiceTests()
    {
        _repository = GetRequiredService<MyEntityRepository>();
        _appService = GetRequiredService<IMyEntityAppService>();
    }

    [Fact]
    public async Task Should_Create()
    {
        // Arrange
        var input = new CreateDto { Name = "Test" };

        // Act
        var id = await _appService.CreateAsync(input);

        // Assert
        var entity = await _repository.GetAsync(id);
        entity.ShouldNotBeNull();
        entity.Name.ShouldBe(input.Name);  // Use Shouldly assertions
    }
}
```

**Always inject services using `GetRequiredService<T>()` in test constructors.**

## Code Conventions

### Backend ABP Patterns

**1. Application Services:**
```csharp
[Authorize(ExamPermissions.MyEntity.Default)]
public class MyEntityAppService : ExamAppService, IMyEntityAppService
{
    private readonly IMyEntityRepository _repository;

    public MyEntityAppService(IMyEntityRepository repository)
    {
        _repository = repository;
    }

    public virtual async Task<PagedResultDto<MyEntityDto>> GetListAsync(GetMyEntitiesInput input)
    {
        var query = await _repository.GetQueryableAsync();
        // Use LINQ for filtering/paging
        return new PagedResultDto<MyEntityDto>(
            totalCount,
            ObjectMapper.Map<List<MyEntity>, List<MyEntityDto>>(list)
        );
    }
}
```

**2. Domain Managers** (for complex business logic):
```csharp
public class MyEntityManager : DomainService
{
    private readonly IMyEntityRepository _repository;
    private readonly ILocalEventBus _eventBus;

    public MyEntityManager(IMyEntityRepository repository, ILocalEventBus eventBus)
    {
        _repository = repository;
        _eventBus = eventBus;
    }

    public virtual async Task<MyEntity> CreateAsync(/* params */)
    {
        // Business validation
        var entity = new MyEntity(/* params */);
        await _repository.InsertAsync(entity);
        return entity;
    }
}
```

**3. Domain Entities:**
```csharp
public class MyEntity : FullAuditedAggregateRoot<Guid>
{
    public virtual string Name { get; protected set; }

    protected MyEntity() { }  // For EF Core

    public MyEntity(Guid id, string name) : base(id)
    {
        Name = name;
    }
}
```

### Angular Admin Patterns

**1. Component Structure (NG-ALAIN):**
```typescript
@Component({
  selector: 'app-my-entity',
  templateUrl: './my-entity.component.html',
  styleUrls: ['./my-entity.component.less']
})
export class MyEntityComponent extends STPageBase {
  query = {
    filter: '',
    skipCount: 0,
    maxResultCount: 10
  };

  @ViewChild('st', { static: true })
  st!: STComponent;

  getData(): void {
    this.schemas.forEach((item) => (item.rec = true));
    this.http
      .get(`api/app/my-entity`, { params: this.query })
      .subscribe((res) => {
        this.st.reset({
          data: res.items,
          total: res.totalCount
        });
      });
  }
}
```

**2. Proxy Usage:**
```typescript
import { MyEntityService } from '@proxy/volo/abp/my-entity';

// Auto-generated service from backend
constructor(private myEntityService: MyEntityService) {}

async loadItems() {
  const result = await this.myEntityService.getList({ ... });
  this.items = result.items;
}
```

## Project-Specific Patterns

### Dual Application Separation

This project has **two application layers**:
- `SuperAbp.Exam.Application` - User-facing APIs (exams, questions, training)
- `SuperAbp.Exam.Admin.Application` - Admin management APIs

**When adding features:**
- User features → Create in `SuperAbp.Exam.Application` and `SuperAbp.Exam.HttpApi`
- Admin features → Create in `SuperAbp.Exam.Admin.Application` and `SuperAbp.Exam.Admin.HttpApi`
- Domain entities and repositories are **shared** between both

### OpenIddict Configuration

Authentication is configured in `SuperAbp.Exam.Domain/OpenIddict/OpenIddictDataSeedContributor.cs`.

Clients are seeded from `appsettings.json` under `OpenIddict:Applications`:
- `Exam_App` - User Angular web app
- `Exam_Admin_App` - Admin Angular web app
- `Exam_Swagger` - Swagger UI for user API
- `Exam_Admin_Swagger` - Swagger UI for admin API

### Multi-tenancy

Multi-tenancy is controlled by `MultiTenancyConsts.IsEnabled`. When disabled, the system operates in single-tenant mode.

### Symlink Management (Angular Admin)

The admin project uses symlinks for shared resources. Run these PowerShell scripts **from `angular-web/scripts/`**:
- `.\setup-symlinks.ps1` - Create symlinks (run after `yarn install`)
- `.\remove-symlinks.ps1` - Remove symlinks (run before git commit)

**Always remove symlinks before committing to avoid issues in CI/CD.**

## Key Files & Directories

### Backend
- `aspnet-core/src/SuperAbp.Exam.Domain/` - Entities, aggregates, domain services
- `aspnet-core/src/SuperAbp.Exam.Application/` - User-facing AppServices, DTOs
- `aspnet-core/src/SuperAbp.Exam.Admin.Application/` - Admin AppServices, DTOs
- `aspnet-core/test/SuperAbp.Exam.Application.Tests/` - Application service tests
- `aspnet-core/test/SuperAbp.Exam.TestBase/` - Test base classes with data seeding

### Frontend
- `angular-admin/src/app/` - Admin UI components and services
- `angular-admin/src/app/proxy/` - **AUTO-GENERATED** API proxies (do not edit)
- `angular-web/src/app/` - User exam-taking UI
- `angular-web/src/app/proxy/` - **AUTO-GENERATED** API proxies (do not edit)

## Integration Points

### Frontend → Backend Communication

1. **Angular apps call backend APIs via auto-generated proxy services**
2. **Authentication flow:** Angular → OpenIddict AuthServer → Backend APIs
3. **SignalR:** Used for real-time updates (e.g., exam progress)

### Background Jobs

Exam submissions are processed asynchronously via `IBackgroundJobManager`. See `SuperAbp.Exam.Jobs.SubmittedUserExam` for job implementation patterns.

### Event Bus

Domain events are published via `ILocalEventBus` for decoupled communication (e.g., exam completion triggers notification).

## Deployment Notes

### CI/CD Workflows

- `.github/workflows/build-and-test.yml` - Build and test on PR
- `.github/workflows/deploy-backend.yml` - Publish .NET projects (run `abp install-libs` for each host)
- `.github/workflows/deploy-frontend.yml` - Build Angular apps

### Build Artifacts

Backend is published to `aspnet-core/publish/` with:
- `AdminApi/` - Admin API host
- `Api/` - User API host
- `AuthServer/` - OpenIddict auth server
- `BackgroundServices/` - Background job processor
- `DbMigrator/` - Database migration tool

Frontend builds to:
- `angular-admin/dist/exam-admin/browser/` → `frontend/admin/`
- `angular-web/dist/exam-web/browser/` → `frontend/web/`

## Common Pitfalls

1. **Forgetting to run `abp install-libs`** - Causes build failures in host projects
2. **Editing auto-generated proxy files** - Changes will be overwritten on next generation
3. **Not removing symlinks before committing** - Breaks CI/CD builds
4. **Mixing admin/user application code** - Keep them in respective projects
5. **Not using `GetRequiredService<T>()` in tests** - Services won't be properly injected
6. **Missing `[Authorize]` attributes** - Services become publicly accessible

## Language & Localization

- Default language: **Simplified Chinese (zh-CN)**
- Supported languages configured in `ExamDomainModule.cs`
- Frontend i18n files in `angular-admin/src/assets/i18n/`
- Backend localization in standard ABP `.json` resource files

## AI Assistant Behavior Guidelines

When helping complete features in this codebase:

1. **Minimal Documentation** - Only include:
   - ✅ Brief comments explaining complex logic
   - ✅ Class/method summaries for public APIs
   
   Do NOT generate:
   - ❌ README files
   - ❌ Line-by-line comments
   - ❌ Installation/setup guides
   - ❌ Feature description documents
   - ❌ Inline XML documentation strings

2. **Focus on Code** - Provide:
   - Working implementation code
   - Clean, self-explanatory code
   - Refactoring suggestions if asked

3. **Assume Knowledge** - User is familiar with:
   - ABP Framework patterns
   - Project structure and conventions
   - Domain-driven design concepts
   - Testing patterns

**Example Response Format:**
```typescript
// ✅ Good: Brief comment for complex logic
export class MyComponent {
  constructor(private myService: MyService) {
    this.myService = myService;
  }

  async loadData() {
    this.data = await this.myService.getData();
  }
}

// ❌ Bad: Line-by-line comments
export class MyComponent {
  // Constructor injection
  constructor(private myService: MyService) {
    // Assign to property
    this.myService = myService;
  }

  // Load data method
  async loadData() {
    // Call service and assign
    this.data = await this.myService.getData();
  }
}
```

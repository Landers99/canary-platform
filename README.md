# Canary Platform — Feature Flags with Auto-Canary & SLO Guardrails (WIP)

> Roll out features safely with percentage rollouts, SLO-guarded canaries, and automatic rollback.
> Tech: .NET 8 (ASP.NET Core), Postgres, Docker, Terraform, OpenTelemetry (coming).

### [Portfolio](https://landers99.github.io/) | [Work Sample](https://github.com/Landers99/canary-platform/blob/main/work-samples-v0.zip)

<!-- Badges (update the URLs to your repo/actions) -->

![CI](https://github.com/Landers99/canary-platform/actions/workflows/ci.yml/badge.svg)
![TF Validate](https://github.com/Landers99/canary-platform/actions/workflows/terraform-plan.yml/badge.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## Quickstart

**Prereqs:** Docker, .NET 8, Terraform 1.6+

```bash
# Clone
git clone https://github.com/Landers99/canary-platform.git
cd canary-platform

# Run local stack
docker compose -f ops/docker-compose.yml up --build -d
curl -s http://localhost:8080/healthz

# Build & test
dotnet restore && dotnet build -c Release && dotnet test -c Release

# Terraform (dev)
cd infra/terraform/envs/dev
terraform fmt -check && terraform init && terraform validate
```

---

## Project structure

```
canary-platform/
  apps/
    control-plane/            # ASP.NET Core Web API (Day 1: /healthz + /flags/{key} stub)
    control-plane.tests/      # xUnit test for /healthz
  sdks/
    dotnet/                   # .NET SDK (stubs today)
  infra/
    terraform/
      modules/                # vpc, rds-postgres (skeletons)
      envs/
        dev/                  # dev environment (fmt/validate/plan)
  ops/
    docker-compose.yml        # local stack
  .github/
    workflows/
      ci.yml                  # build/test/format; add terraform-plan.yml when ready
  docs/
    ADR-000-template.md       # Day 2
  README.md
  canary-platform.sln
```

---

## Configuration (env vars)

* `ConnectionStrings__Db`: `Host=db;Port=5432;Database=canary;Username=canary;Password=canary`
* `ASPNETCORE_URLS`: `http://0.0.0.0:8080` (for container binding)

---

## Terraform (Day-1 skeleton)

> If you have AWS creds, you can run `plan`. Otherwise just do `fmt/init/validate` (the CI job will still be useful).

```bash
cd infra/terraform/envs/dev
terraform fmt -check
terraform init
terraform validate
# Optional with creds:
terraform plan -var="db_username=canary" -var="db_password=canary1234"
```

---

## Day-1 Acceptance Checklist

* [x] **Repo public** with solution, compose, and minimal endpoints.
* [x] `docker compose up` succeeds; `GET /healthz` returns `200 {"status":"ok"}`.
* [x] `dotnet test` green (health test).
* [ ] **CI** (Actions) runs build/test/format on PR & push.
* [ ] **Terraform** `validate` (and `plan` if creds) succeeds.
* [ ] README quickstart works copy-paste.

---

## Troubleshooting

**❌ `failed to read dockerfile: open Dockerfile: no such file or directory`**
Create `apps/control-plane/Dockerfile` and make your compose build context point to it:

```yaml
build:
  context: ..
  dockerfile: apps/control-plane/Dockerfile
```

Sample Dockerfile:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
WORKDIR /src/apps/control-plane
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out /p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet","ControlPlane.dll"]
```

**❌ `bind: address already in use` on 5432**
Remove `db.ports` entirely (preferred) or map a different host port: `ports: ["55432:5432"]`.

**❌ 404 on `/healthz` in Docker**
Rebuild with cache busting:
`docker compose -f ops/docker-compose.yml up --build -d --force-recreate`

**❌ Test can’t find `Program`**
Ensure `public partial class Program { }` is at the bottom of `apps/control-plane/Program.cs` and that tests reference the project:

```bash
dotnet add apps/control-plane.tests reference apps/control-plane
```

---

## Roadmap (next 72 hours)

* **Day 2:** ADR-000 (requirements, API, data model, scaling); diagram; CI badge.
* **Day 3:** Minimal Postgres schema + `/flags/{key}` read path; add integration test.
* **Day 4:** SDK v0.1 (.NET) with polling + local cache; plug into sample app.

## Changelog

* [Week 1](https://www.linkedin.com/posts/jonathan-l-smith_dotnet-featureflags-observability-activity-7361074014951264259-ipuz?utm_source=share&utm_medium=member_desktop&rcm=ACoAACOvxHQBRnMKyd0QWMmrAI7PPtOupDOLX4U)

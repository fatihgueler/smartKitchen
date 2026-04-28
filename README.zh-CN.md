# SmartKitchen

语言: [English](README.md) | [Deutsch](README.de.md) | [Русский](README.ru.md) | [中文](README.zh-CN.md)

SmartKitchen 是一个基于 .NET 8 的全栈应用，用于管理核心厨房业务流程。它将菜谱管理、食材与库存管理、每周餐食计划、购物清单自动生成以及订单管理整合到一个 Blazor Server 界面和 ASP.NET Core Web API 中。

该仓库采用分层解决方案结构，将前端、API、领域模型和基础设施清晰分离。当前用户界面语言为德语。

## 项目概览

SmartKitchen 围绕实际厨房运营流程设计，主要覆盖以下场景：

- 创建和维护菜谱
- 管理食材和库存水平
- 制定每周餐食计划
- 根据餐食计划和现有库存生成购物清单
- 跟踪订单并在仪表盘中汇总关键数据

API 使用 Entity Framework Core 和 SQLite，并在启动时自动应用未执行的迁移。项目还包含初始种子数据，因此本地开发启动后即可直接使用。

## 核心功能

- 仪表盘，展示关键指标、最近订单和当天餐食
- 菜谱管理，包含准备时间、烹饪时间、份量、难度和预估成本
- 食材目录和库存管理
- 低库存与即将过期项目监控
- 每周餐食计划
- 基于餐食计划并扣除当前库存后生成购物清单
- 订单管理
- 开发环境下通过 Swagger 提供 API 文档

## 架构

```text
Blazor Server UI (SmartKitchen)
        |
        v
ASP.NET Core Web API (SmartKitchen.API)
        |
        v
Entity Framework Core + SQLite
        |
        v
Domain 和 Infrastructure 项目
```

该解决方案采用分层架构，而不是单一的单体项目文件。这样可以让界面、API、领域模型和持久化层分别演进。

## 解决方案结构

```text
SmartKitchen/
|- SmartKitchen.csproj               # Blazor Server 前端
|- Components/                      # Razor 组件与页面
|- SmartKitchen.API/                # ASP.NET Core Web API
|- SmartKitchen.Domain/             # 领域实体
|- SmartKitchen.Application/        # 应用层
|- SmartKitchen.Infrastructure/     # EF Core DbContext 与迁移
|- wwwroot/                         # 静态资源
`- SmartKitchen.sln                 # 解决方案入口
```

## 技术栈

| 领域 | 技术 |
| --- | --- |
| 前端 | Blazor Server (.NET 8) |
| 后端 | ASP.NET Core Web API |
| 持久化 | Entity Framework Core |
| 数据库 | SQLite |
| API 文档 | Swagger / OpenAPI |
| 工具链 | .NET SDK 8 |

## 本地开发

### 前置要求

- .NET 8 SDK

### 恢复依赖

```powershell
dotnet restore .\SmartKitchen.sln
```

### 启动 API

```powershell
dotnet run --project .\SmartKitchen.API\SmartKitchen.API.csproj
```

### 启动前端

```powershell
dotnet run --project .\SmartKitchen.csproj
```

### 默认本地地址

- Frontend: `http://localhost:5037`
- API: `http://localhost:5011`
- Swagger: `http://localhost:5011/swagger`

如果你通过 Rider 或 Visual Studio 启动整个解决方案，应将前端和 API 配置为同时启动的项目。

## 配置说明

当前前端在 `Program.cs` 中直接配置了固定的 API 基础地址：

- `http://localhost:5011`

如果你修改了 API 端口，也需要同步更新这里的地址。

## 数据库与种子数据

- 本地持久化使用 SQLite
- 连接字符串指向 `SmartKitchen.db`
- EF Core 迁移文件位于 `SmartKitchen.Infrastructure/Migrations`
- API 启动时会自动应用尚未执行的迁移
- 种子数据包含示例食材、菜谱和库存记录

本地数据库文件已通过 `.gitignore` 排除，不会提交到仓库。

## API 范围

当前 API 暴露以下端点：

- `api/dashboard`
- `api/recipes`
- `api/ingredients`
- `api/inventory`
- `api/mealplans`
- `api/orders`
- `api/shoppinglist`

开发环境下可通过 Swagger 浏览并手动测试这些端点。

## 构建

```powershell
dotnet build .\SmartKitchen.sln
```

## 仓库说明

- IDE 元数据已从版本控制中排除
- 构建产物已从版本控制中排除
- 本地 SQLite 文件已从版本控制中排除

## 当前状态

该仓库已经包含主要应用结构以及适用于本地开发的一组可用功能。它可以作为后续扩展业务逻辑、加强 API 校验、加入自动化测试以及完善部署流程的稳定基础。

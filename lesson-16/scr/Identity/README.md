# Identity

## Содержание

1. [Общее описание проекта](##Общее-описание-проекта)
1. [Настройки окружения](##Настройки-окружения)
1. [Описание API](##Описание-API)
1. [Миграции](##Миграции)

---

##Общее описание проекта

Identity-сервер на основе [IdentityServer4](https://docs.identityserver.io/)

##Настройки окружения

Для работы проекта нужно настроить следующие переменные окружения:
```
// подключение к БД Postgres
"POSTGRESQL_SERVICE_HOS": "localhost",
"POSTGRESQL_SERVICE_PORT": "7654",
"USERS_PG_DATABASE": "otus-identity",
"USERS_PG_USERNAME": "postgres",
"USERS_PG_PASSWORD": "qweqwe123",
"USE_SSL_PG_CONNECTION": "false",

"MIGRATION_MODE": "true", // запуск миграции
"ORCHESTRATOR_TYPE": "K8S" // указание среды контейнера 
```

##Описание API
###REST API
Полное описание REST API можно посмотреть при помощи Swagger по ссылке: `/swagger`.

##Миграции
Проекту для работы необходимы 3 контекста:
1. `ApplicationDbContext` - the Entity Framework database context used for identity.
1. `ConfigurationDbContext` - DbContext for the IdentityServer configuration data.
1. `PersistedGrantDbContext` - DbContext for the IdentityServer operational data.

Для создания миграций нужно: 
1. Настроить параметры в конструкторе `Identity.Settings.AppSetting`
```c#
    internal class AppSettings : IAppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            //ConnectionString = GetDbConnectionString(configuration); 
            //IsMigrationService = configuration.GetValueOrThrow<bool>("MIGRATION_MODE");
            //IsInKubernetes = configuration.IsInKubernetes();
            ConnectionString = "Host=localhost;Port=7654;Database=otus-identity;Username=postgres;Password=\"qweqwe123\";"; 
        }
    ...
    }
```

1. Выполнить команды:

```shell
# ApplicationDbContext migration creating
.../scr> dotnet ef migrations add <MIGRATION_NAME> --project Identity --context ApplicationDbContext

# ConfigurationDbContext migration creating
.../scr> dotnet ef migrations add <MIGRATION_NAME> --project Identity --context ConfigurationDbContext --output-dir Migrations/ConfigurationDb

# PersistedGrantDbContext migration creating
.../scr> dotnet ef migrations add <MIGRATION_NAME> --project Identity --context PersistedGrantDbContext --output-dir Migrations/PersistedGrantDb

```

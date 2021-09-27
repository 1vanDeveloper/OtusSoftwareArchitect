# Исходный проект

## Шпаргалки

### Настройка `dotnet` - окружения Entity Framework в терминале в корне проекта

```shell
> dotnet new tool-manifest
> dotnet tool install --local dotnet-ef
```

### Создание миграции

1. В *.Host проекте в конструкторе `AppSettings(IConfiguration configuration)` закомментируйте все строки кроме `UsersDbConnectionString`

```C#
public AppSettings(IConfiguration configuration)
{
    //UsersDbConnectionString = GetDbConnectionString(configuration); 
    //IsMigrationService = configuration.GetValue<bool>("MIGRATION_MODE");
    //IdentityServerUrl = configuration.GetValue<string>("IDENTITY_SERVER_URL");
    UsersDbConnectionString = "Host=localhost;Port=7654;Database=otus-users;Username=postgres;Password=\"qweqwe123\";"; 
}
```

2. В терминале *.Host проекта выполние команду

```shell
> dotnet ef migrations add {migration_name} --project ../{project_name}.Domain
```
# Публикация на nuget.org (Trusted Publishing)

Долгоживущий **API key не нужен**: [Trusted Publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishers) + GitHub OIDC.

## 1. Политика на nuget.org

1. Войти на [nuget.org](https://www.nuget.org/) (аккаунт, который будет владеть `AIGuiders.Cdp.Core`, напр. **LonelySoul**).
2. **Профиль** → **Trusted Publishing** → GitHub:
   - **Repository owner:** `KarataevDmitry`
   - **Repository:** `cdp-core`
   - **Workflow file:** `nuget-publish.yml` (только имя файла)
   - **Environment:** пусто (если workflow без `environment:`)

## 2. Запуск

- **Тег:** `v0.1.0` → пакет `0.1.0`
- **Вручную:** Actions → **Publish to NuGet** → version `0.1.0`

## 3. Локально (проверка pack)

```bash
dotnet pack Cdp.Core.csproj -c Release -o nupkg
```

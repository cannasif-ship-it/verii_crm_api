# Deployment (Jenkins / Production)

## Required Configuration

This API fails fast on startup if critical secrets are missing.

Set these as environment variables in Jenkins (recommended) or provide an external config file at runtime.

### Connection Strings

- `ConnectionStrings__DefaultConnection`
- `ConnectionStrings__ErpConnection`

### JWT

- `JwtSettings__SecretKey` (must be set; no fallback)
- `JwtSettings__Issuer` (optional; defaults to `CmsWebApi`)
- `JwtSettings__Audience` (optional; defaults to `CmsWebApiUsers`)
- `JwtSettings__ExpiryMinutes` (optional; defaults to config)

### Data Protection (SMTP password encryption)

If you store encrypted SMTP passwords, you must keep DataProtection keys stable across restarts/deploys:

- `DataProtection__KeyPath` = a persistent directory (shared volume)

If the key ring changes, previously stored encrypted values cannot be decrypted.

## Bootstrap Admin (first install only)

If the database has **zero users**, you can create the first admin user by setting:

- `BootstrapAdmin__Email`
- `BootstrapAdmin__Password`
- `BootstrapAdmin__RoleId` (optional, default `3`)

After the first successful login, remove these variables.

## Dev-only Admin Login

Removed. Use `POST /api/auth/login` or Bootstrap Admin for first install.

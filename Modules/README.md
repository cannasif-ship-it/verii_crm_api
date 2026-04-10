# CRM Modular Migration

This directory is the new target architecture for `verii_crm_api`.

Migration rules for the current transition:

- New code should prefer `Modules/<Feature>` over legacy technical folders.
- Each feature module is organized as `Api`, `Application`, `Domain`, and `Infrastructure`.
- Shared cross-cutting concerns live under `Shared`.
- Legacy code remains in place until a module is migrated safely and verified.
- `_old` is reserved for fully retired legacy folders after their replacements are stable.

Planned first migration candidates:

1. `Definitions`
2. `System`
3. `Customer`
4. `Identity`
5. `AccessControl`

High-risk modules that should move after the foundation is stable:

1. `Quotation`
2. `Demand`
3. `Order`
4. `Approval`
5. `Integrations`
6. `Reporting`

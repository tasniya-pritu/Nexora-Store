# Nexora

Nexora is an ASP.NET Core MVC electronics store demo. It includes product browsing and filtering, product details and recommendations, a session-based cart and wishlist, checkout, order tracking and invoices, product reviews, and an admin area for product management.

## Requirements

- .NET 7 SDK

## Run locally

From the repository root:

```powershell
dotnet run --launch-profile http
```

Open [http://localhost:5267](http://localhost:5267). To use the HTTPS launch profile instead, run `dotnet run --launch-profile https`.

## Admin login

The admin login is configured using environment variables. Set both variables before starting the app; do not put real credentials in source files or commit them to the repository.

```powershell
$env:Admin__Username = "your-admin-username"
$env:Admin__Password = "your-strong-password"
dotnet run --launch-profile http
```

Then select **Admin** in the store header. If either variable is missing, admin login is not available.

## Notes

- The project targets `net7.0` in `GadgetX Store.csproj`.
- Product, order, and review data are held in application memory; changes are not persisted after the app restarts.
- This repository does not include a license file. Unless a license is added, no permission to reuse, modify, or distribute the code is granted by default.

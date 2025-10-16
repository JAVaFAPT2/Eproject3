<!-- a797104c-4286-487d-b23c-da88d8cd0bcd 7a46dcf2-d16e-41c0-94c2-18eaf0961bf9 -->
# Backend updates: VehicleModels search, soft delete, JSON POST

## Scope

- VehicleModels only
- Merge GET list and search into one endpoint with nullable `search`
- Soft delete VehicleModel
- Create model via JSON body; uploads continue via VehiclePhotosController

## Changes

### 1) Merge GET + search

- File: `VehicleShowroomManagement/src/WebAPI/Controllers/VehicleModelsController.cs`
- Ensure existing `[HttpGet]` accepts `search?: string` (already added), remove separate `search` GET or deprecate.
- File: `.../Application/Features/VehicleModels/Queries/GetVehicleModels/GetVehicleModelsQuery.cs`
- Confirm `Search?: string` exists (done) and propagate.
- File: `.../Queries/GetVehicleModels/GetVehicleModelsQueryHandler.cs`
- Already filters by `Search` if provided. Keep pagination.

### 2) Soft delete VehicleModel

- Domain: `VehicleShowroomManagement/src/Domain/Entities/VehicleModel.cs`
- Add `DateTimeOffset? DeletedAt { get; private set; }`
- Add method `MarkDeleted()` and guard in constructors/operations if needed.
- Application: create command `DeleteVehicleModelCommand` to mark soft delete.
- Files under `.../Features/VehicleModels/Commands/DeleteVehicleModel/*`
- Handler: fetch by id, set `DeletedAt = UtcNow`, persist.
- Infrastructure Repository: ensure `GetAllAsync` and queries exclude `DeletedAt != null`.
- Update repository or handler to filter out deleted models.
- WebAPI: `VehicleModelsController`
- Change `HttpDelete("{modelNumber}")` to call soft delete command.

### 3) POST VehicleModel JSON-only

- WebAPI: `VehicleModelsController`
- Replace `[Consumes("multipart/form-data")]` Create with `[FromBody] CreateVehicleModelRequest` (JSON).
- Remove in-method file handling and Cloudinary upload logic from Create.
- Keep upload flows in `VehiclePhotosController` only.
- Application: `CreateVehicleModelCommand` stays; ensure `ImageUrl`/primary photo is not required.

### 4) Cleanup and deprecation

- `VehicleModelsController`: remove or mark `[HttpGet("search")]` as obsolete and redirect to main GET with `search` param.
- Ensure all GET handlers exclude soft-deleted items.

## Notes

- No changes to `Vehicles` or `Users` per selection.
- Frontend will continue to call `GET /api/vehiclemodels?search=...` and separate photo uploads.
- Backward compatibility: return same DTOs; just omit soft-deleted models.

### To-dos

- [ ] Consolidate VehicleModels GET and remove deprecated search endpoint
- [ ] Add DeletedAt and MarkDeleted to VehicleModel entity
- [ ] Add DeleteVehicleModelCommand and handler for soft delete
- [ ] Exclude soft-deleted models in list and get-by-id queries
- [ ] Change CreateVehicleModel to JSON body; remove multipart handling
- [ ] Update README/ENV docs for new usage and deprecations
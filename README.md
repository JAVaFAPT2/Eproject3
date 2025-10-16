# Eproject3

## Backend API Updates

- VehicleModels
  - GET `/api/vehiclemodels`: supports optional `search` param; deprecated `/api/vehiclemodels/search` removed.
  - Soft delete enabled via `DELETE /api/vehiclemodels/{modelNumber}`; soft-deleted models excluded from GET, by-id, and by-slug queries.
  - Create via JSON-only: `POST /api/vehiclemodels` with body fields (`modelNumber?`, `name`, `price`, `description?`, `parentId?`, `level?`, `slug?`).
  - Upload photos via VehiclePhotos endpoints after creation.
  - Anonymous (guest) read access enabled for GET endpoints.

- Vehicles
  - GET `/api/vehicles`: merged list+search with optional filters (`searchTerm`, `status`, `modelNumber`, `seats`, `fuelType`, `minPrice`, `maxPrice`, paging).
  - Deprecated `/api/vehicles/search` returns guidance to use the main GET.

- Photos (VehiclePhotos)
  - GET `/api/vehicle-models/{modelNumber}/photos` and `GET /api/photos/{photoId}` are public.
  - Upload stays on dedicated endpoints; requires auth.

- Specs (VehicleSpecs)
  - GET `/api/vehicle-models/{modelNumber}/specs` and `GET /api/specs/{specId}` are public.

## Frontend Notes

- Use `GET /api/vehiclemodels?search=...` for search or omit `search` to get all.
- After creating a model via JSON, call photo upload endpoints to attach images.
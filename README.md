# Eproject3

## Backend API Updates

### **Standardized Response Structure** ✅
All paginated list endpoints now return consistent structure:
```json
{
  "items": [...],
  "totalCount": 0,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 0
}
```
**Applied to**: `/api/users`, `/api/vehicle-models`, `/api/vehicles`, `/api/purchase-orders`, `/api/orders`, `/api/service-orders`, `/api/vehicle-models/{id}/photos`, `/api/vehicle-models/{id}/specs`

- VehicleModels
  - GET `/api/vehicle-models`: unified endpoint with optional filters `search`, `parentModelNumber`, `seats`, `fuelType`; previous `/api/vehicle-models/search` behavior is routed via this endpoint.
  - Soft delete enabled via `DELETE /api/vehiclemodels/{modelNumber}`; soft-deleted models excluded from GET, by-id, and by-slug queries.
  - Create via JSON-only: `POST /api/vehiclemodels` with body fields (`modelNumber?`, `name`, `price`, `description?`, `parentId?`, `level?`, `slug?`).
  - Upload photos via VehiclePhotos endpoints after creation.
  - Anonymous (guest) read access enabled for GET endpoints.

- Vehicles
  - GET `/api/vehicles`: merged list+search with optional filters (`searchTerm`, `status` as 1/2/3, `modelNumber`, `seats`, `fuelType`, `minPrice`, `maxPrice`, paging).
  - Deprecated `/api/vehicles/search` → use the main GET.
 - Status Enums (Unified numeric parsing)
  - Vehicle: 1=Available, 2=Reserved, 3=Sold
  - Order: 1=Pending, 2=Confirmed, 3=Completed
  - ServiceOrder: 1=InProgress, 2=Completed, 3=Cancelled
  - PurchaseOrder (filtering): 1=Pending, 2=Completed, 3=Cancelled

- Orders
  - `POST /api/orders/{id}/assign-vehicle` requires `{ vehicleId?, dealerId }`; omitting `vehicleId` auto-picks first available by model.
  - `PUT /api/orders/{id}/status` to change status (1/2/3). `POST /complete` removed.

- Purchase Orders
  - `PUT /api/purchase-orders/{id}/status` to change status (1/2/3). `POST /complete` removed.

- Service Orders
  - `PUT /api/service-orders/{id}/status` accepts `{ status, licensePlate? }`; on Completed, sets license plate if provided.

- Dashboard
  - `GET /api/dashboard/top-vehicles` defaults to current month when no dates.
  - Revenue and Customer analytics return last 6 months series.

- Photos (VehiclePhotos)
  - GET `/api/vehicle-models/{modelNumber}/photos` and `GET /api/photos/{photoId}` are public.
  - Upload stays on dedicated endpoints; requires auth.

- Specs (VehicleSpecs)
  - GET `/api/vehicle-models/{modelNumber}/specs` and `GET /api/specs/{specId}` are public.

## Frontend Notes

- Use `GET /api/vehiclemodels?search=...` for search or omit `search` to get all.
- After creating a model via JSON, call photo upload endpoints to attach images.
- **All list responses now use `items` property** - no more checking different property names per endpoint.
<!-- 69058063-b98d-44a5-bf6c-a99552378056 353e72a8-32e2-4031-b143-b18064e59f23 -->
# Variant Model Hierarchy & API Revamp

## Scope

Implement a two-level `VehicleModel` hierarchy with `parentId` (Level-1: 911; Level-2: 911-thuong), keep `Vehicle` as physical units (VIN), generate unique slugs for Level-2 models, move photos to model-level, simplify orders (customer-only create, dealer assignment later), update service orders to set license plates, remove billing docs, and extend dashboards.

## Data Model Updates

- VehicleModel (Domain)
  - Add: `ParentId` (nullable), `Level` (1 or 2), `Slug` (unique, Level-2), remove `Brand`, remove `ImageUrl`.
  - Indexes: `Slug` unique (Level-2), `ParentId`.
- VehiclePhoto
  - Keep only `ModelId` (remove `VehicleId`).
- Vehicle
  - Keep: `VehicleId`, `ModelNumber` (points to Level-2 model), remove `ReceiptDate` (field from API), `LicensePlate` set later by Service Order.
- Specs
  - Attach specs to Level-2 VehicleModel (seats, fuelType, etc.) to support search.

## Endpoints Changes

- Users
  - PUT `/api/users/{id}`: body `{ isActive: boolean }` only.
- Vehicle Models
  - POST `/api/vehicle-models` (multipart): accept `data` JSON (no brand/imageUrl) + `files` for photos; photos saved under modelId.
  - GET `/api/vehicle-models/slug/{slug}`: fetch Level-2 model by slug.
  - GET `/api/vehicle-models/search` (Level-2): filters by `parentModelNumber`, `seats`, `fuelType` (from specs).
- Vehicles
  - POST `/api/vehicles`: remove `receiptDate`; do not require `licensePlate`.
  - GET `/api/vehicles/{id}` unchanged; add GET `/api/vehicles/slug/{slug}` to avoid exposing id for Level-2 model details (or attach slug in related returns where needed).
- Photos
  - POST `/api/vehicle-models/{modelNumber}/photos/upload`: upload files; store with `modelId`.
- Purchase Orders
  - POST `/api/purchase-orders`: remove `expectedDeliveryDate`.
  - POST `/api/purchase-orders/{id}/lines`: lines refer to Level-2 model (variant) identifier (modelNumber level-2) and quantity; on complete, create `Vehicle` items for that variant.
- Orders
  - POST `/api/orders`: created by customer only; no `dealerId`, no `note`, `appointmentDate = null`; include `modelNumber` (Level-2) or `vehicleVariantId` synonym.
  - POST `/api/orders/{id}/assign-vehicle`: assign by Level-2 model/variant; pick a specific available `Vehicle` of that Level-2 model.
- Service Orders
  - PUT `/api/service-orders/{id}/status`: when finalizing (Completed), accept `licensePlate` to set on the assigned `Vehicle`.
- Billing Documents
  - Remove Billing Document APIs.
- Dashboard
  - Add `GET /api/dashboard/overview`: totals: profit = (vehicle sale + service) − purchase cost (from PO), count employees, customers who purchased, completed orders, Level-2 models, vehicles.
  - GET `/api/dashboard/top-vehicles`: aggregate by Level-2 model.
  - GET `/api/dashboard/recent-orders`: include vehicle and Level-2 model.

## Implementation Steps

1. Domain & Persistence
   - Update `VehicleModel` entity + mapping (ParentId, Level, Slug); add slug generator utility; add indexes.
   - Update `VehiclePhoto` to reference `ModelId` only; migrate usages.
   - Ensure `Vehicle` creation ignores licensePlate, receiptDate in commands/models.
   - Move/ensure specs stored on Level-2 `VehicleModel`.

2. Application Layer
   - Commands/Queries for VehicleModels: create (multipart), get by slug, search Level-2 with filters.
   - Update PO complete logic to spawn Vehicles by Level-2 model lines.
   - Update Orders: create by customer (no dealerId, note), assign-vehicle by variant (Level-2), status changes.
   - Update ServiceOrders status handler to set `licensePlate` when Completed.

3. WebAPI Layer
   - Controllers: adjust routes/actions and DTOs per endpoints above.
   - Multipart handling for model photo uploads (`data` + `files`).
   - Add new routes: `/api/vehicle-models/slug/{slug}`, `/api/vehicle-models/search`, `/api/vehicles/slug/{slug}`.
   - Remove BillingDocument controllers/routes; update docs.

4. Dashboard
   - Implement queries for overview metrics and top/recent endpoints per new aggregation rules.

5. Documentation
   - Update `API_DOCUMENTATION.md` and `COMPLETE_API_ENDPOINTS.md` with new shapes and examples.

## Notes

- Slug uniqueness enforced only for Level-2 models; pattern: `{parentName}-{variantName}` (e.g., `911-mui-tran`).
- Backward compatibility: keep existing GET by id routes; add slug-based routes.
- Ensure CORS still allows credentials.

### To-dos

- [x] Update VehicleModel (parentId, level, slug), VehiclePhoto link to modelId only
- [x] Add slug generation, get-by-slug, level-2 search with filters
- [x] Adjust Vehicles create (no receiptDate, no licensePlate), add GET by slug
- [x] Create model photo upload endpoint (multipart)
- [x] Change PO lines to Level-2 model and complete creates Vehicles
- [x] Revise Orders: customer-only create; assign vehicle by variant
- [x] Service order status Completed sets vehicle licensePlate
- [ ] Remove billing docs routes and references
- [ ] Implement overview, top-vehicles (level-2), recent-orders with variant
- [ ] Revise API docs and endpoints examples accordingly


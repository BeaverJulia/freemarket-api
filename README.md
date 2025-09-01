# Shop Cart API
Shopping API - cart with discounts updates and more

## Features

### MVP (Must Have) (4h)
- ✅ Create shop cart
- ✅ Add item, add multiple items, add multiple of same item
- ✅ Remove item
- ✅ Get totals: without VAT and with 20% VAT
- ✅ Add shipping cost 
- ✅ Swagger/OpenAPI
- ✅ Unit tests for pricing/discount/VAT calculations

### Stretch Goals (Nice to Have)
- 🔄 Discounted catalog items & discount code (excludes already-discounted items)
- 🔄 Integration tests for a couple of endpoint happy paths
- 🔄 Basic validation and error problem details (RFC 7807)

## Quick Start

### Prerequisites
- .NET 9 SDK

### Run the API
```bash
# Navigate to the solution directory
cd freemarket-api

# Restore packages and build
dotnet restore && dotnet build

# Run the API
dotnet run --project src/Shop.Api

# Or run from the API directory
cd src/Shop.Api
dotnet run
```

The API will be available at:
- **API**: https://localhost:7001 (or http://localhost:5001)
- **Swagger**: https://localhost:7001/swagger

## API Endpoints

### Cart Management
- `POST /api/v1/carts` - Create a new cart
- `GET /api/v1/carts/{id}` - Get cart details
- `POST /api/v1/carts/{id}/items` - Add single item
- `POST /api/v1/carts/{id}/items/batch` - Add multiple items
- `DELETE /api/v1/carts/{id}/items/{productId}` - Remove item(s)

### Cart Operations
- `POST /api/v1/carts/{id}/discount-code` - Apply discount code
- `PUT /api/v1/carts/{id}/shipping` - Set shipping country
- `GET /api/v1/carts/{id}/totals` - Get cart totals

### Products
- `GET /api/v1/products` - Get all products
- `GET /api/v1/products/{id}` - Get product by ID

### Health
- `GET /api/v1/health` - Health check

## Example Usage

### 1. Create a Cart
```bash
curl -X POST "https://localhost:7001/api/v1/carts" \
  -H "Content-Type: application/json" \
  -d '{"country": "GB"}'
```

### 2. Add Items
```bash
curl -X POST "https://localhost:7001/api/v1/carts/{cart-id}/items/batch" \
  -H "Content-Type: application/json" \
  -d '{
    "items": [
      {"productId": "11111111-1111-1111-1111-111111111111", "quantity": 2},
      {"productId": "22222222-2222-2222-2222-222222222222", "quantity": 1}
    ]
  }'
```

### 3. Apply Discount Code
```bash
curl -X POST "https://localhost:7001/api/v1/carts/{cart-id}/discount-code" \
  -H "Content-Type: application/json" \
  -d '{"code": "SAVE10"}'
```

### 4. Get Totals
```bash
curl -X GET "https://localhost:7001/api/v1/carts/{cart-id}/totals"
```

## Seed Data

### Products
- **Apple** (APP-001): £0.50
- **Bread** (BREAD-001): £1.20 (10% off)
- **Milk** (MILK-001): £0.90
- **Coffee** (COFFEE-001): £5.00

### Discount Codes
- **SAVE10**: 10% off eligible (non-discounted) items
- **FREESHIP**: Free shipping for GB orders

## Pricing Rules

### Per-Item Pricing
- If item has `IsDiscounted = true`, apply `DiscountPercent` to unit price
- Otherwise, item is eligible for discount codes

### Discount Code Rules
- **SAVE10**: 10% off line totals only
- **FREESHIP**: Sets shipping to £0 for GB
- Cannot stack multiple percentage codes (first wins)

### VAT Calculation
- **UK (GB)**: 20% VAT on goods after discount codes
- **EU/Rest**: 0% VAT (configurable)
- VAT = Round(0.20 × (SubtotalExVatAll - DiscountCodeSavings))

### Shipping
- **UK**: £4.99 (free over £50 subtotal ex-VAT)
- **EU**: £9.99
- **Rest**: £14.99
- **FREESHIP code**: £0 for GB orders

## Testing

### Run Tests
```bash
# Run all tests
dotnet test

# Run specific project tests
dotnet test src/Shop.Application.Tests/
dotnet test src/Shop.Api.Tests/
```

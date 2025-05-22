# Catalog API

## Endpoints

### Get Available Plans
**Endpoint:** `/catalog/plans`  
**Method:** `GET`  
**Description:** Retrieves a list of available plans and their details.  
**Query Parameters:**
- `type` (string, optional) - Filter plans by type (`on_demand`, `recurring`).
- `service` (string, optional) - Filter by specific services included in the plan.

---

### Get Plan Details
**Endpoint:** `/catalog/plans/{plan_id}`  
**Method:** `GET`  
**Description:** Retrieves details of a specific plan, including payment options and included services.  
**Parameters:**  
- None (the `plan_id` in the URL identifies the plan).

---

### Create a New Plan
**Endpoint:** `/catalog/plans`  
**Method:** `POST`  
**Description:** Creates a new plan with specified services and payment configurations.  
**Parameters:**
- `name` (string, required) - Name of the plan.
- `description` (string, optional) - Description of the plan.
- `type` (string, required) - Type of payment for the plan. Values: `on_demand`, `recurring`.
- `price` (float, required) - Price of the plan.
- `services` (array, required) - List of services included in the plan:
  - `service_id` (string, required) - ID of the service.
  - `service_name` (string, required) - Name of the service.

---

### Create a Package
**Endpoint:** `/catalog/packages`  
**Method:** `POST`  
**Description:** Creates a package by combining multiple services into one offering.  
**Parameters:**
- `name` (string, required) - Name of the package.
- `description` (string, optional) - Description of the package.
- `price` (float, required) - Price of the package.
- `services` (array, required) - List of services included in the package:
  - `service_id` (string, required) - ID of the service.
  - `service_name` (string, required) - Name of the service.

---

### Get Packages
**Endpoint:** `/catalog/packages`  
**Method:** `GET`  
**Description:** Retrieves all existing packages and their details.

---

### Update Plan
**Endpoint:** `/catalog/plans/{plan_id}`  
**Method:** `PUT`  
**Description:** Updates details of an existing plan.  
**Parameters:**
- Any fields from the plan creation parameters (`name`, `description`, `type`, `price`, `services`) can be provided to update specific attributes.

---

### Delete Plan
**Endpoint:** `/catalog/plans/{plan_id}`  
**Method:** `DELETE`  
**Description:** Deletes an existing plan.

---

## Example Responses

### Successful Response for `GET /catalog/plans`
```json
{
  "status": "success",
  "plans": [
    {
      "plan_id": "plan123",
      "name": "Premium Plan",
      "type": "recurring",
      "price": 49.99,
      "services": [
        {
          "service_id": "service1",
          "service_name": "Streaming Service"
        },
        {
          "service_id": "service2",
          "service_name": "Cloud Storage"
        }
      ]
    },
    {
      "plan_id": "plan456",
      "name": "Basic On-Demand Plan",
      "type": "on_demand",
      "price": 19.99,
      "services": [
        {
          "service_id": "service3",
          "service_name": "E-book Access"
        }
      ]
    }
  ]
}
```

### Successful Response for POST /catalog/plans
```json
{
  "status": "success",
  "message": "Plan created successfully.",
  "plan_id": "plan789"
}
```

### Successful Response for POST /catalog/packages
```json
{
  "status": "success",
  "message": "Package created successfully.",
  "package_id": "pkg123"
}
```

### Error Response
```json
{
  "status": "error",
  "message": "An error occurred while processing the request."
}
```
# Customer Data Management API

## Endpoints

### Create Customer
**Endpoint:** `/customers`  
**Method:** `POST`  
**Description:** Creates a new customer record.  
**Parameters:**
- `name` (string, required) - Full name of the customer.
- `email` (string, required) - Email address of the customer.
- `phone` (string, optional) - Phone number of the customer.
- `address` (object, optional) - Address details:
  - `street` (string, optional) - Street name.
  - `number` (string, optional) - House or building number.
  - `city` (string, optional) - City name.
  - `state` (string, optional) - State name.
  - `postal_code` (string, optional) - Postal code.

---

### Get Customer Details
**Endpoint:** `/customers/{customer_id}`  
**Method:** `GET`  
**Description:** Retrieves the details of a specific customer.  
**Parameters:**
- None (the `customer_id` in the URL uniquely identifies the customer).

---

### Update Customer
**Endpoint:** `/customers/{customer_id}`  
**Method:** `PUT`  
**Description:** Updates the details of an existing customer.  
**Parameters:**
- Any fields from the customer data model (e.g., `name`, `email`, `phone`, `address`) can be sent to update specific attributes.

---

### Delete Customer
**Endpoint:** `/customers/{customer_id}`  
**Method:** `DELETE`  
**Description:** Deletes a customer record.  
**Parameters:**
- None (the `customer_id` in the URL uniquely identifies the customer).

---

### Search Customers
**Endpoint:** `/customers/search`  
**Method:** `GET`  
**Description:** Searches for customers based on filters.  
**Query Parameters:**
- `name` (string, optional) - Filter by name.
- `email` (string, optional) - Filter by email.
- `phone` (string, optional) - Filter by phone number.
- `city` (string, optional) - Filter by city.

---

## Example Responses

### Successful Response for `POST` or `PUT` Endpoints
```json
{
  "status": "success",
  "message": "Customer data saved successfully.",
  "customer_id": "cus12345"
}
```

### Successful Response for GET Endpoint (/customers/{customer_id})
```json
{
  "customer_id": "cus12345",
  "name": "John Doe",
  "email": "johndoe@example.com",
  "phone": "555-1234",
  "address": {
    "street": "Main Street",
    "number": "123",
    "city": "New York",
    "state": "NY",
    "postal_code": "10001"
  }
}
```

### Successful Response for Search Endpoint (/customers/search)
```json
{
  "status": "success",
  "customers": [
    {
      "customer_id": "cus12345",
      "name": "John Doe",
      "email": "johndoe@example.com",
      "phone": "555-1234"
    },
    {
      "customer_id": "cus67890",
      "name": "Jane Smith",
      "email": "janesmith@example.com",
      "phone": "555-5678"
    }
  ]
}
```

### Error Response
```json
{
  "status": "error",
  "message": "An error occurred while processing the request."
}
```
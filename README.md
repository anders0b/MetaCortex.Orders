# OrderService API Documentation

### Base URL
`http://<your-server-url>/api/orders`

---

## 1. Create an Order

- **Endpoint**: `POST /api/orders`
- **Description**: Creates a new order.
- **Request Body**:  
  - **Content-Type**: `application/json`
  - **Body Example**:  
    ```json
    {
        "orderDate": "2024-12-03T14:30:00",
        "customerId": "12345",
        "paymentStatus": true,
        "shippingStatus": false,
        "products": ["Product1", "Product2"]
    }
    ```

- **Response**:
  - **Status Code**: `201 Created`
  - **Location**: `/api/orders/{orderId}`
  - **Response Body Example**:
    ```json
    {
        "orderId": "64d2f6ae3f15c9a2e47a9f01",
        "orderDate": "2024-12-03T14:30:00",
        "customerId": "12345",
        "paymentStatus": true,
        "shippingStatus": false,
        "products": ["Product1", "Product2"]
    }
    ```

- **Error Response**:
  - **Status Code**: `400 Bad Request`
  - **Response Body**:  
    ```json
    {
        "message": "Order cannot be null"
    }
    ```

---

## 2. Get Order by ID

- **Endpoint**: `GET /api/orders/{orderId}`
- **Description**: Retrieves an order by its ID.
- **Request Parameters**:
  - `orderId`: The unique identifier of the order to retrieve.

- **Response**:
  - **Status Code**: `200 OK`
  - **Response Body Example**:
    ```json
    {
        "orderId": "64d2f6ae3f15c9a2e47a9f01",
        "orderDate": "2024-12-03T14:30:00",
        "customerId": "12345",
        "paymentStatus": true,
        "shippingStatus": false,
        "products": ["Product1", "Product2"]
    }
    ```

- **Error Response**:
  - **Status Code**: `404 Not Found`
  - **Response Body**:
    ```json
    {
        "message": "Order not found"
    }
    ```

---

## 3. Get All Orders

- **Endpoint**: `GET /api/orders`
- **Description**: Retrieves a list of all orders.
- **Response**:
  - **Status Code**: `200 OK`
  - **Response Body Example**:
    ```json
    [
        {
            "orderId": "64d2f6ae3f15c9a2e47a9f01",
            "orderDate": "2024-12-03T14:30:00",
            "customerId": "12345",
            "paymentStatus": true,
            "shippingStatus": false,
            "products": ["Product1", "Product2"]
        },
        {
            "orderId": "64d2f6ae3f15c9a2e47a9f02",
            "orderDate": "2024-12-04T10:15:00",
            "customerId": "67890",
            "paymentStatus": false,
            "shippingStatus": false,
            "products": ["Product3"]
        }
    ]
    ```

---

## 4. Delete an Order

- **Endpoint**: `DELETE /api/orders/{orderId}`
- **Description**: Deletes an order by its ID.
- **Request Parameters**:
  - `orderId`: The unique identifier of the order to delete.

- **Response**:
  - **Status Code**: `204 No Content`
  - **Response Body**: Empty (No content).

- **Error Response**:
  - **Status Code**: `404 Not Found`
  - **Response Body**:
    ```json
    {
        "message": "Order not found"
    }
    ```

---

## 5. Update an Order

- **Endpoint**: `PUT /api/orders/{orderId}`
- **Description**: Updates an existing order by its ID.
- **Request Parameters**:
  - `orderId`: The unique identifier of the order to update.

- **Request Body**:  
  - **Content-Type**: `application/json`
  - **Body Example**:  
    ```json
    {
        "orderDate": "2024-12-03T14:30:00",
        "customerId": "12345",
        "paymentStatus": true,
        "shippingStatus": true,
        "products": ["Product1", "Product2", "Product4"]
    }
    ```

- **Response**:
  - **Status Code**: `200 OK`
  - **Response Body Example**:
    ```json
    {
        "orderId": "64d2f6ae3f15c9a2e47a9f01",
        "orderDate": "2024-12-03T14:30:00",
        "customerId": "12345",
        "paymentStatus": true,
        "shippingStatus": true,
        "products": ["Product1", "Product2", "Product4"]
    }
    ```

- **Error Responses**:
  - **400 Bad Request**: If the order body is null or invalid.
    ```json
    {
        "message": "Order cannot be null"
    }
    ```
  - **404 Not Found**: If the order with the specified `orderId` is not found.
    ```json
    {
        "message": "Order not found"
    }
    ```

---

## Summary

- **POST /api/orders** - Creates a new order.
- **GET /api/orders/{orderId}** - Retrieves an order by ID.
- **GET /api/orders** - Retrieves all orders.
- **DELETE /api/orders/{orderId}** - Deletes an order by ID.
- **PUT /api/orders/{orderId}** - Updates an existing order by ID.

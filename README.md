# Order API Specification

## Overview
This document describes the API for managing orders in the system. The API allows clients to create, retrieve, update, and delete orders.

## Endpoints

### `POST /api/orders`
**Summary**: Create a new order.

#### Request Body
- **Content-Type**: `application/json`
- **Schema**: [Order](#order-schema)

#### Responses
- **201 Created**: Order created successfully.
  - **Body**: [Order](#order-schema)
- **400 Bad Request**: Invalid input (order cannot be null).

---

### `GET /api/orders`
**Summary**: Retrieve all orders.

#### Responses
- **200 OK**: A list of orders.
  - **Body**: Array of [Order](#order-schema)

---

### `GET /api/orders/{orderId}`
**Summary**: Retrieve a specific order by its ID.

#### Path Parameters
- `orderId` (string): The ID of the order to retrieve.

#### Responses
- **200 OK**: The requested order.
  - **Body**: [Order](#order-schema)
- **404 Not Found**: Order not found.

---

### `PUT /api/orders/{orderId}`
**Summary**: Update an existing order.

#### Path Parameters
- `orderId` (string): The ID of the order to update.

#### Request Body
- **Content-Type**: `application/json`
- **Schema**: [Order](#order-schema)

#### Responses
- **200 OK**: Order updated successfully.
  - **Body**: [Order](#order-schema)
- **400 Bad Request**: Invalid input (order cannot be null).
- **404 Not Found**: Order not found.

---

### `DELETE /api/orders/{orderId}`
**Summary**: Delete an order.

#### Path Parameters
- `orderId` (string): The ID of the order to delete.

#### Responses
- **204 No Content**: Order deleted successfully.
- **404 Not Found**: Order not found.

---

## Schemas

### Order Schema
| Field                 | Type               | Description                                    |
| --------------------- | ------------------ | ---------------------------------------------- |
| `id`                  | string             | Unique identifier for the order.               |
| `orderDate`           | string (date-time) | Date and time when the order was placed.       |
| `customerId`          | string             | ID of the customer who placed the order.       |
| `paymentMethod`       | string             | Payment method used for the order.             |
| `isPaid`              | boolean            | Indicates whether the order has been paid.     |
| `vipStatus`           | boolean            | Indicates whether the customer has VIP status. |
| `products`            | array              | List of products in the order.                 |
| `products[].id`       | string             | Unique identifier for the product.             |
| `products[].name`     | string             | Name of the product.                           |
| `products[].price`    | number (decimal)   | Price of the product.                          |
| `products[].quantity` | integer            | Quantity of the product in the order.          |

### Product Schema
| Field      | Type             | Description                        |
| ---------- | ---------------- | ---------------------------------- |
| `id`       | string           | Unique identifier for the product. |
| `name`     | string           | Name of the product.               |
| `price`    | number (decimal) | Price of the product.              |
| `quantity` | integer          | Available quantity of the product. |

---

## Notes
- Ensure all required fields are included in requests.
- Use appropriate HTTP status codes to handle errors and success cases.
- For sensitive operations, consider implementing authentication and authorization.

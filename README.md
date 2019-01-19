# Basket Service
Basket Service is a ASP.NET Core WebApi to manage baskets for a customer. It only holds the basket with a list of items information in a memory store (can be replaced with another suitable store after the prototype).

No meta-data information about customers or items is being stored, this information can either be populated from other service and or this service can be extended to add that information in the appropriate classes. I have used strings as keys for the purpose of this prototype.

This service allows customer to perform following operations
1. GET - Get Basket /api/{customerId}/basket
2. POST - Create Basket /api/{cutomerId}/basket
3. POST - Add Item /api/{customerId}/basket/items
4. PUT - Change Item Quantity /api/{customerId}/basket/items/{itemId}
5. DELETE - Remove Item from Basket /api/{customerId}/basket/items/{itemId}
6. DELETE - Clear Basket /api/{customerId}/basket/items

# Basket Service SDK:
A basic client library is provided to perform all the supported operations given the base url of Basket Service.
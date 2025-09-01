namespace Shop.Domain.Exceptions;

public class CartNotFoundException(Guid cartId) : Exception($"Cart with ID {cartId} not found");

public class ProductNotFoundException(Guid productId) : Exception($"Product with ID {productId} not found");

public class InvalidDiscountCodeException(string code) : Exception($"Invalid discount code: {code}");

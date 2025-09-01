using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Domain.DTOs;
using Shop.Domain.Exceptions;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CartsController(ICartService cartService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status201Created)]
    public ActionResult<ShopCart> CreateCart([FromBody] CreateCartRequest request)
    {
        var cart = cartService.CreateCart(request.Country);
        return CreatedAtAction(nameof(GetCart), new { id = cart.Id }, cart);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> GetCart(Guid id)
    {
        var cart = cartService.GetCart(id);
        if(cart == null)
        {
            throw new CartNotFoundException(id);
        }
        
        return Ok(cart);
    }

    [HttpPost("{id}/items")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> AddItem(Guid id, [FromBody] AddItemRequest request)
    {
        var cart = cartService.AddItem(id, request.ProductId, request.Quantity);
        return Ok(cart);
    }

    [HttpPost("{id}/items/batch")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> AddMultipleItems(Guid id, [FromBody] AddMultipleItemsRequest request)
    {
        var cart = cartService.AddMultipleItems(id, request.Items);
        return Ok(cart);
    }

    [HttpDelete("{id}/items/{productId}")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> RemoveItem(Guid id, Guid productId, [FromQuery] int? quantity = null)
    {
        var cart = cartService.RemoveItem(id, productId, quantity);
        if (cart == null)
        {
            throw new CartNotFoundException(id);
        }
        
        return Ok(cart);
    }

    [HttpPost("{id}/discount-code")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> ApplyDiscountCode(Guid id, [FromBody] ApplyDiscountCodeRequest request)
    {
        var cart = cartService.ApplyDiscountCode(id, request.Code);
        return Ok(cart);
    }

    [HttpPut("{id}/shipping")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> SetShipping(Guid id, [FromBody] SetShippingRequest request)
    {
        var cart = cartService.SetShipping(id, request.Country);
        return Ok(cart);
    }

    [HttpGet("{id}/totals")]
    [ProducesResponseType(typeof(CartTotalsResponse), StatusCodes.Status200OK)]
    public ActionResult<CartTotalsResponse> GetTotals(Guid id)
    {
        var totals = cartService.GetTotals(id);
        return Ok(totals);
    }
}

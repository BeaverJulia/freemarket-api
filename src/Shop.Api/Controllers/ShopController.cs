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
        try
        {
            var cart = cartService.CreateCart(request.Country);
            return CreatedAtAction(nameof(GetCart), new { id = cart.Id }, cart);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid country code",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> GetCart(Guid id)
    {
        var cart = cartService.GetCart(id);
        if(cart == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Cart not found",
                Detail = $"Cart with ID {id} was not found",
                Status = StatusCodes.Status404NotFound
            });
        }
        
        return Ok(cart);
    }

    [HttpPost("{id}/items")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> AddItem(Guid id, [FromBody] AddItemRequest request)
    {
        try
        {
            var cart = cartService.AddItem(id, request.ProductId, request.Quantity);
            return Ok(cart);
        }
        catch (CartNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Cart not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Product not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpPost("{id}/items/batch")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> AddMultipleItems(Guid id, [FromBody] AddMultipleItemsRequest request)
    {
        try
        {
            var cart = cartService.AddMultipleItems(id, request.Items);
            return Ok(cart);
        }
        catch (CartNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Cart not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Product not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpDelete("{id}/items/{productId}")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> RemoveItem(Guid id, Guid productId, [FromQuery] int? quantity = null)
    {
        try
        {
            var cart = cartService.RemoveItem(id, productId, quantity);
            if (cart == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Cart not found",
                    Detail = $"Cart with ID {id} was not found",
                    Status = StatusCodes.Status404NotFound
                });
            }
            
            return Ok(cart);
        }
        catch (CartNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Cart not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }

    [HttpPost("{id}/discount-code")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> ApplyDiscountCode(Guid id, [FromBody] ApplyDiscountCodeRequest request)
    {
        try
        {
            var cart = cartService.ApplyDiscountCode(id, request.Code);
            return Ok(cart);
        }
        catch (CartNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Cart not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (InvalidDiscountCodeException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid discount code",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpPut("{id}/shipping")]
    [ProducesResponseType(typeof(ShopCart), StatusCodes.Status200OK)]
    public ActionResult<ShopCart> SetShipping(Guid id, [FromBody] SetShippingRequest request)
    {
        try
        {
            var cart = cartService.SetShipping(id, request.Country);
            return Ok(cart);
        }
        catch (CartNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Cart not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid country code",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpGet("{id}/totals")]
    [ProducesResponseType(typeof(CartTotalsResponse), StatusCodes.Status200OK)]
    public ActionResult<CartTotalsResponse> GetTotals(Guid id)
    {
        try
        {
            var totals = cartService.GetTotals(id);
            return Ok(totals);
        }
        catch (CartNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Cart not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }
}

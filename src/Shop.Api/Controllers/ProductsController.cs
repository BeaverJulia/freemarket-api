using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Entities;
using Shop.Infrastructure.Interfaces;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ICatalogStore _catalogStore;
    
    public ProductsController(ICatalogStore catalogStore)
    {
        this._catalogStore = catalogStore;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        var products = _catalogStore.GetAll();
        return Ok(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product> GetProduct(Guid id)
    {
        var product = _catalogStore.GetById(id);
        if (product == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Produc not found",
                Detail = $"Product with ID {id} was not found",
                Status = StatusCodes.Status404NotFound
            });
        }
        
        return Ok(product);
    }
}

using Application.Common.DTOs;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Queries.GetProductById;
using Application.Products.Queries.GetProductsBrandsAndTypes;
using Application.Products.Queries.GetProductsWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public sealed class ProductsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProductsWithPagination([FromQuery] GetProductsWithPaginationQuery query, CancellationToken cancellationToken)
        {
            var products = await Mediator.Send(query, cancellationToken);

            return products.Items!;
        }

        //[HttpGet]
        //public async Task<ActionResult<List<ProductDto>>> GetProducts(CancellationToken cancellationToken)
        //{
        //   return await Mediator.Send(new GetProductsQuery(), cancellationToken);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id, CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetProductByIdQuery() { Id = id }, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateProduct(CreateProductCommand command, CancellationToken cancellationToken)
        {
            return await Mediator.Send(command, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, UpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (command.Id != id)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteProductCommand() { Id = id }, cancellationToken);

            return NoContent();
        }

        [HttpGet("filters")]
        public async Task<ActionResult> GetFilters(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetProductsBrandsAndTypesQuery(), cancellationToken));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.PublicApi.Catalogs.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace Microsoft.eShopWeb.PublicApi.Catalogs;
[Route("api/[controller]")]
[ApiController]
public class ListProdLocalidad : ControllerBase
{
    private static List<LocalidadDto> _localidadesDto = new List<LocalidadDto>
    {
        new LocalidadDto { Id = 1, Localidad = "Quito"},
        new LocalidadDto { Id = 2, Localidad = "Guayaquil" }
    };

    private static List<ProductosLocalidadDto> _productosLocalidadesDto = new List<ProductosLocalidadDto>
    {
        new ProductosLocalidadDto { Id = 1 , LocalidadId = 1, ProductName = "ROSLYN RED SHEET"},
        new ProductosLocalidadDto { Id = 2 , LocalidadId = 1, ProductName = "NET BOT BLACK SWEATSHIRT"},
        new ProductosLocalidadDto { Id = 3 , LocalidadId = 1, ProductName = ".NET BLUE SWEATSHIRT"},
        new ProductosLocalidadDto { Id = 4 , LocalidadId = 2, ProductName = "CUP<T> WHITE MUG"},
        new ProductosLocalidadDto { Id = 5 , LocalidadId = 2, ProductName = " ROSLYN RED T-SHIRT"}
    };

    [HttpGet]
    [Route("AllLocalid")]
    [SwaggerOperation(
        Summary = "Localidades",
        Description = "Localidades",
        OperationId = "",
        Tags = new[] { "Localidades" })
    ]
    public ActionResult<IEnumerable<LocalidadDto>> GetAllLocalid()
    {
        return _localidadesDto;
    }

    [HttpGet]
    [Route("AllProductos")]
    [SwaggerOperation(
        Summary = "Products",
        Description = "Products",
        OperationId = "",
        Tags = new[] { "Products" })
    ]
    public ActionResult<IEnumerable<ProductosLocalidadDto>> GetAllProducts()
    {
        return _productosLocalidadesDto;
    }

    [HttpGet]
    [Route("ByLocalidad/{idLocalidad:int}")]
    [SwaggerOperation(
        Summary = "Products by Localidad",
        Description = "Products by Localidad",
        OperationId = "loc.listprod",
        Tags = new[] { "ListPorduLoca" })
    ]
    public ActionResult<IEnumerable<ProductsByLocalidadDto>> GetProductsByLocalidad(int idLocalidad)
    {
        LocalidadDto? localidadExist = _localidadesDto.FirstOrDefault(l => l.Id == idLocalidad);

        if (localidadExist is null)
        {
            return Forbid();
        }

        return _productosLocalidadesDto
            .Where(p => p.LocalidadId == idLocalidad)
            .Select(p => new ProductsByLocalidadDto
            {
                ProductName = p.ProductName,
                Localidad = localidadExist.Localidad,
            })
            .ToList();
    }
}

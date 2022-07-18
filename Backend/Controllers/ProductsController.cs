using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.DBModels;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Services.IProductService _productService;

        public ProductsController(Services.IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public ActionResult<IEnumerable<Models.SystemModels.Product>> GetProducts()
        {
            return Ok(_productService.GetAll());
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Models.SystemModels.Product> PostProduct([FromBody] object product)
        {
            var p = JsonConvert.DeserializeObject<Services.ProductWrapper>(product.ToString());
            return _productService.CreateProduct(p);
        }

        
    }
}

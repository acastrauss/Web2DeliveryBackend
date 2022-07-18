using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductWrapper
    {
        public Models.SystemModels.Product product { get; set; } = new Models.SystemModels.Product();
        public int adminId { get; set; }

        public ProductWrapper() { }
    }

    public interface IProductService
    {
        IEnumerable<Models.SystemModels.Product> GetAll();
        Models.SystemModels.Product CreateProduct(ProductWrapper productWrapper);
    }
}

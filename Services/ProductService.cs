using Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private Models.IDBModels.ICRUD _DBCrud;/*= new DataLayer.MSSQLDB.CRUD.MSSQLProductCRUD();*/
        private readonly Models.IDBModels.IConversion _DBConvert; /*= new DataLayer.MSSQLDB.Conversion.MSSQLConversion();*/

        public ProductService(Models.IDBModels.IConversion convert, Models.IDBModels.CRUDServiceResolver crudResolver) 
        {
            _DBConvert = convert;
            _DBCrud = crudResolver(Models.IDBModels.CRUDServiceType.Product);
        }

        public Product CreateProduct(ProductWrapper productWrapper)
        {
            var p = productWrapper;
            var pdb = _DBConvert.ConvertProductDB(p.product);
            ((DataLayer.DBModels.Product)pdb).AddedBy = p.adminId;
            var prod = _DBConvert.ConvertProductSystem(_DBCrud.Create(pdb));

            return prod;
        }

        public IEnumerable<Product> GetAll()
        {
            var prodsDB = _DBCrud.ReadAll();
            List<Models.SystemModels.Product> products = new List<Models.SystemModels.Product>();
            prodsDB.ToList().ForEach(d =>
            {
                products.Add(_DBConvert.ConvertProductSystem(d));
            });

            return products;
        }
    }
}

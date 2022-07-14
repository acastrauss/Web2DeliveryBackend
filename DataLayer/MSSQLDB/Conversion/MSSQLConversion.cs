using Microsoft.EntityFrameworkCore;
using Models.IDBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.MSSQLDB.Conversion
{
    public class MSSQLConversionException : Exception
    {
        public override string Message => "Bad model for conversion"; 
    }

    public class MSSQLConversion : Models.IDBModels.IConversion
    {
        public Models.IDBModels.IDBModel ConvertAdminDB(Models.SystemModels.Admin model)
        {
            var delivs = new List<DBModels.Deliverer>();
            model.Deliverers.ToList().ForEach(d => delivs.Add((DBModels.Deliverer)this.ConvertDelivereDB(d)));

            var prods = new List<DBModels.Product>();
            model.Products.ToList().ForEach(p => prods.Add((DBModels.Product)this.ConvertProductDB(p)));

            return new DBModels.Admin()
            {
                Deliverers = delivs,
                Products = prods,
                UserId = (int)model.Id
            };
        }

        public Models.SystemModels.Admin ConvertAdminSystem(Models.IDBModels.IDBModel model)
        {
            var dbAdmin = model as DBModels.Admin;
            if(dbAdmin == null)
            {
                throw new MSSQLConversionException();
            }

            var delivs = new List<Models.SystemModels.Deliverer>();
            dbAdmin.Deliverers.ToList().ForEach(d => this.ConvertDelivererSystem(d));
            var prods = new List<Models.SystemModels.Product>();
            dbAdmin.Products.ToList().ForEach(p => this.ConvertProductSystem(p));

            return new Models.SystemModels.Admin(
                    dbAdmin.User.Id,
                    dbAdmin.User.Username.Trim(),
                    dbAdmin.User.Email.Trim(),
                    dbAdmin.User.Password.Trim(),
                    dbAdmin.User.FirstName.Trim(),
                    dbAdmin.User.LastName.Trim(),
                    dbAdmin.User.DateOfBirth,
                    dbAdmin.User.Address.Trim(),
                    dbAdmin.User.PicturePath.Trim(),
                    delivs,
                    prods
                );
        }

        public Models.IDBModels.IDBModel ConvertDelivereDB(Models.SystemModels.Deliverer model)
        {
            var purchs = new List<DBModels.Purchase>();
            model.Purchases.ForEach(p => this.ConvertPurchaseDB(p));

            return new DBModels.Deliverer()
            {
                ApprovalStatus = (int)model.Status,
                Purchases = purchs,
                UserId = (int)model.Id
            };
        }

        public Models.SystemModels.Deliverer ConvertDelivererSystem(Models.IDBModels.IDBModel model)
        {
            var deliverer = model as DBModels.Deliverer;

            if(deliverer == null)
            {
                throw new MSSQLConversionException();
            }

            Models.SystemModels.Purchase currentPurch = null;
            var purchs = new List<Models.SystemModels.Purchase>();
            deliverer.Purchases.ToList().ForEach(p => {
                this.ConvertPurchaseSystem(p);

                if(Models.SystemModels.PurhaseStatus.ACCEPTED == (Models.SystemModels.PurhaseStatus)p.Status)
                {
                    currentPurch = this.ConvertPurchaseSystem(p);
                }
            });

            return new Models.SystemModels.Deliverer(
                deliverer.User.Id,
                deliverer.User.Username.Trim(),
                deliverer.User.Email.Trim(),
                deliverer.User.Password.Trim(),
                deliverer.User.FirstName.Trim(),
                deliverer.User.LastName.Trim(),
                deliverer.User.DateOfBirth,
                deliverer.User.Address.Trim(),
                deliverer.User.PicturePath.Trim(),
                (Models.SystemModels.ApprovalStatus)deliverer.ApprovalStatus,
                purchs,
                currentPurch
            );
        }

        public IDBModel ConvertIUserDB(Models.SystemModels.IUser model)
        {
            return new DBModels.Iuser()
            {
                Id = (int)model.Id,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
                PicturePath = "Nista",
                Username = model.Username,
                UserType = (int)model.UType
            };
        }

        public Models.SystemModels.IUser ConvertIUserSystem(IDBModel model)
        {
            var userDb = model as DBModels.Iuser;

            if (userDb == null)
            {
                throw new MSSQLConversionException();
            }

            return new Models.SystemModels.IUser()
            {
                Address = userDb.Address.Trim(),
                DateOfBirth = userDb.DateOfBirth,
                Email = userDb.Email.Trim(),
                FirstName = userDb.FirstName.Trim(),
                Id = userDb.Id,
                LastName = userDb.LastName.Trim(),
                Password = "", // do not send password
                PicturePath = userDb.PicturePath.Trim(),
                Username = userDb.Username.Trim(),
                UType = (Models.SystemModels.UserType)userDb.UserType
            };
        }

        public Models.IDBModels.IDBModel ConvertProductDB(Models.SystemModels.Product model)
        {
            return new DBModels.Product()
            {
                Id = model.Id,
                Ingredients = model.Ingredients,
                Name = model.Name,
                Price = model.Price
            };
        }

        public Models.SystemModels.Product ConvertProductSystem(Models.IDBModels.IDBModel model)
        {
            var product = model as DBModels.Product;

            if (product == null)
            {
                throw new MSSQLConversionException();
            }

            var prod = new Models.SystemModels.Product(
                product.Name.Trim(), (float)product.Price, product.Ingredients.Trim()
            );
            prod.Id = product.Id;
            return prod;
        }

        public Models.IDBModels.IDBModel ConvertPurchaseDB(Models.SystemModels.Purchase model)
        {
            var prods = new List<DBModels.Product>();
            model.PurchaseItems.ForEach(p => prods.Add((DBModels.Product)this.ConvertProductDB(p)));

            // check for cons of id errors
            var consOf = new List<DBModels.ConsistOf>();
            model.PurchaseItems.ForEach(p => consOf.Add(new DBModels.ConsistOf()
            {
                ProductId = p.Id,
                PurchaseId = model.Id
            }));

            return new DBModels.Purchase()
            {
                Comment = model.Comment,
                Id = model.Id,
                Status = (int)model.Status,
                TotalPrice = model.TotalPrice,
                ConsistOfs = consOf,
                CreatedAt = model.CreatedAt,
                DeliveredAt = model.DeliveredAt,
                DeliverToAddress = model.Address
            };
        }

        public Models.IDBModels.IDBModel ConvertPurchaserDB(Models.SystemModels.Purchaser model)
        {
            var purchs = new List<DBModels.Purchase>();
            model.PreviousPurchases.ForEach(p => purchs.Add((DBModels.Purchase)this.ConvertPurchaseDB(p)));

            return new DBModels.Purchaser()
            {
                UserId = (int)model.Id,
                Purchases = purchs
            };
        }

        public Models.SystemModels.Purchaser ConvertPurchaserSystem(Models.IDBModels.IDBModel model)
        {
            var purchaser = model as DBModels.Purchaser;

            if (purchaser == null)
            {
                throw new MSSQLConversionException();
            }

            var prevPurchs = new List<Models.SystemModels.Purchase>();
            Models.SystemModels.Purchase currPurch = null;
            purchaser.Purchases.ToList().ForEach(p =>
            {
                prevPurchs.Add(this.ConvertPurchaseSystem(p));
            
                if((Models.SystemModels.PurhaseStatus)p.Status == Models.SystemModels.PurhaseStatus.ORDERED)
                {
                    currPurch = this.ConvertPurchaseSystem(p);
                }
            });

            return new Models.SystemModels.Purchaser(
                purchaser.User.Id,
                purchaser.User.Username.Trim(),
                purchaser.User.Email.Trim(),
                purchaser.User.Password.Trim(),
                purchaser.User.FirstName.Trim(),
                purchaser.User.LastName.Trim(),
                purchaser.User.DateOfBirth,
                purchaser.User.Address.Trim(),
                purchaser.User.PicturePath.Trim(),
                prevPurchs,
                currPurch
                );
        }

        public Models.SystemModels.Purchase ConvertPurchaseSystem(Models.IDBModels.IDBModel model)
        {
            var purchase = model as DBModels.Purchase;

            if (purchase == null)
            {
                throw new MSSQLConversionException();
            }

            var prods = new List<Models.SystemModels.Product>();

            using (var _context = new DataLayer.DBModels.DeliveryDBContext())
            {
                var tempP = _context.Purchases.Include(x => x.ConsistOfs).ThenInclude(x => x.Product).Where(x => x.Id == purchase.Id);
                tempP.ToList().ForEach(x => x.ConsistOfs.ToList().ForEach(c => prods.Add(ConvertProductSystem(c.Product))));
            }

            //purchase.ConsistOfs.ToList().ForEach(co =>
            //{
            //    prods.Add(this.ConvertProductSystem(co.Product));
            //});

            var pur = new Models.SystemModels.Purchase(
                purchase.Id,
                prods,
                (float)purchase.TotalPrice,
                purchase.Comment.Trim(),
                purchase.DeliverToAddress.Trim(),
                (Models.SystemModels.PurhaseStatus)purchase.Status
                );

            pur.DeliveredAt = purchase.DeliveredAt;
            pur.CreatedAt = purchase.CreatedAt;
            return pur;
        }
    }
}

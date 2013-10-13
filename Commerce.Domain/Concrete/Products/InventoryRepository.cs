using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Application.Database;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Products;

namespace Commerce.Application.Concrete.Products
{
    public class InventoryRepository : IInventoryRepository
    {
        PushMarketContext Context { get; set; }

        public InventoryRepository(PushMarketContext context)
        {
            this.Context = context;
        }

        private Product ProductWithColorsAndSizes(int id)
        {
            return
                this.Context.Products
                        .Include(x => x.ThumbnailImageBundle)
                        .Include(x => x.Sizes)
                        .Include(x => x.Colors)
                        .Include(x => x.Colors.Select(color => color.ProductImageBundle))
                        .First(x => x.Id == id && x.IsDeleted == false);
        }

        private List<ProductSku> ActiveInventory(int id, bool includeChildren)
        {
            var product = this.ProductWithColorsAndSizes(id);
            return this.ActiveInventory(product, includeChildren);
        }
        
        private IQueryable<ProductSku> DataSet(bool includeChildren)
        {
            if (includeChildren)
            {
                return Context.ProductSkus
                                    .Include(x => x.Size)
                                    .Include(x => x.Product.ThumbnailImageBundle)
                                    .Include(x => x.Color.ColorImageBundle)
                                    .Where(x => x.IsDeleted == false);
            }
            else
            {
                return Context.ProductSkus.Where(x => x.IsDeleted == false);
            }
        }

        private List<ProductSku> ActiveInventory(Product product, bool includeChildren = false)
        {
            var dataset = DataSet(includeChildren).Where(x => x.Product.Id == product.Id);

            if (!product.Colors.Any() && product.Sizes.Any())
            {
                return dataset
                    .OrderBy(x => x.Size.Order)
                    .ToList();
            }
            if (product.Colors.Any() && !product.Sizes.Any())
            {
                return dataset
                    .OrderBy(x => x.Color.Order)
                    .ToList();
            }
            if (product.Colors.Any() && product.Sizes.Any())
            {
                return dataset
                    .OrderBy(x => x.Color.Order)
                    .ThenBy(x => x.Size.Order)
                    .ToList();
            }

            return dataset.ToList();
        }

        public List<ProductSku> RetreiveByProductId(int id, bool includeChildren)
        {
            return this.ActiveInventory(id, includeChildren);
        }

        public ProductSku RetreiveBySkuCode(string skuCode)
        {
            return this.DataSet(false).FirstOrDefault(x => x.SkuCode == skuCode);
        }

        public void UpdateInStock(int id, int inventoryTotal)
        {
            var sku = this.Context.ProductSkus.FirstOrDefault(x => x.IsDeleted == false && x.Id == id);
            if (sku != null)
            {
                sku.InStock = inventoryTotal;
            }
        }

        public int TotalInStock(int productId)
        {
            var inventory = this.Context.ProductSkus.Where(x => x.Product.Id == productId && x.IsDeleted == false);
            if (!inventory.Any())
            {
                return 0;
            }

            var total = inventory.Sum(x => x.InStock);
            return total;
        }

        public void Wipe(int productId)
        {
            foreach (var sku in this.ActiveInventory(productId, false))
            {
                sku.IsDeleted = true;
            }
        }

        public void Generate(int productId)
        {
            var product = this.ProductWithColorsAndSizes(productId);
            var skus = this.ActiveInventory(productId, true);

            if (!product.Sizes.Any() && !product.Colors.Any())
            {
                if (!skus.Any(x => x.Size == null && x.Color == null))
                {
                    this.Context.ProductSkus.Add(ProductSku.Factory(product, null, null));
                }
                return;
            }

            if (product.Sizes.Any() && !product.Colors.Any())
            {
                foreach (var size in product.Sizes)
                {
                    if (skus.All(x => x.Size != size))
                    {
                        this.Context.ProductSkus.Add(ProductSku.Factory(product, null, size));
                    }
                }
                return;
            }

            if (!product.Sizes.Any() && product.Colors.Any())
            {
                foreach (var color in product.Colors)
                {
                    if (skus.All(x => x.Color != color))
                    {
                        this.Context.ProductSkus.Add(ProductSku.Factory(product, color, null));
                    }
                }
                return;
            }

            foreach (var color in product.Colors)
            {
                foreach (var size in product.Sizes)
                {
                    if (!skus.Any(x => x.Size == size && x.Color == color))
                    {
                        this.Context.ProductSkus.Add(ProductSku.Factory(product, color, size));
                    }
                }
            }
        }

        public void UpdateSkuCode(int productId, string newSkuCode)
        {
            var inventory = this.Context.ProductSkus.Where(x => x.IsDeleted == false && x.Product.Id == productId);
            var product = this.Context.Products.First(x => x.Id == productId);
            var oldBaseSkuCode = product.SkuCode;

            foreach (var sku in inventory)
            {
                sku.OriginalSkuCode = sku.OriginalSkuCode.Replace(oldBaseSkuCode, newSkuCode);
                sku.SkuCode = sku.OriginalSkuCode;
            }
        }

        public void DeleteByColor(int productId, int productColorId)
        {
            this.ActiveInventory(productId, true)
                    .Where(x => x.Color.Id == productColorId)
                    .ToList()
                    .ForEach(sku => sku.IsDeleted = true);
        }

        public void DeleteBySize(int productId, int productSizeId)
        {
            this.ActiveInventory(productId, true)
                    .Where(x => x.Size.Id == productSizeId)
                    .ToList()
                    .ForEach(sku => sku.IsDeleted = true);
        }
    }
}

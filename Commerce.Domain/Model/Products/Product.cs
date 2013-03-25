using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Persist.Model.Lists;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Model.Products
{
    public class Product
    {
        public Product()
        {
            Images = new List<ProductImage>();
            DateCreated = DateTime.Now;
            LastModified = DateTime.Now;
        }

        // Info Tab
        public int Id { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string SEO { get; set; }
        public string Synopsis { get; set; }
        public string Description { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }     
        
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public bool Active { get; set; }

        public List<ProductColor> Colors { get; set; }
        public List<ProductSize> Sizes { get; set; }

        public List<ProductImage> Images { get; set; }
        public bool AssignImagesToColors { get; set; }
        public ImageBundle ThumbnailImageBundle { get; set; }

        // back-end
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }


        public ImageBundle GetProductThumbnail()
        {
            if (!this.Images.Any())
            {
                return null;
            }

            if (this.AssignImagesToColors == true)
            {
                if (this.Images.Any(x => x.ProductColor != null))
                {
                    var image = this.Images
                        .Where(x => x.ProductColor != null)
                        .OrderBy(x => x.ProductColor.Order)
                        .ThenBy(x => x.Order)
                        .FirstOrDefault()
                        .ImageBundle;
                    return image;
                }
                else
                {
                    var image = this.Images
                        .OrderBy(x => x.Order)
                        .FirstOrDefault()
                        .ImageBundle;
                    return image;
                }
            }
            else
            {
                var image = this.Images
                    .OrderBy(x => x.Order)
                    .FirstOrDefault()
                    .ImageBundle;
                return image;
            }
        }

        public void SetThumbnailImages()
        {
            this.ThumbnailImageBundle = this.GetProductThumbnail();

            if (this.AssignImagesToColors && this.Colors.Count() == 0)
            {
                this.AssignImagesToColors = false;
            }

            if (this.AssignImagesToColors)
            {
                foreach (var color in this.Colors)
                {
                    var image = this.Images
                        .Where(x => x.ProductColor != null && x.ProductColor.Id == color.Id)
                        .OrderBy(x => x.Order)
                        .FirstOrDefault();

                    if (image == null)
                    {
                        color.ProductImageBundle = this.ThumbnailImageBundle;
                    }
                    else
                    {
                        color.ProductImageBundle = image.ImageBundle;
                    }
                }
            }
            else
            {
                var image = this.GetProductThumbnail();
                foreach (var color in this.Colors)
                {
                    color.ProductImageBundle = this.ThumbnailImageBundle;
                }
            }
        }
    }
}
using Fodraszat.Models;
using Fodraszat.Models.RequestObjects.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fodraszat.Services
{
    public class InvoiceService
    {
        public static JobObject UpdateJob(JobObject job)
        {
            if (job == null)
            {
                return null;
            }
            
            var data = DataService.GetJobById(job.JobId);
            if (data == null)
            {
                return null;
            }
                
            var discount = Math.Min(100, Math.Max(0, job.Discount));

            job.Discount = discount;
            job.Price = data.Price;

            return job;
        }

        public static MaterialObject UpdateMaterial(MaterialObject material)
        {
            if (material == null)
                return null;
            var data = DataService.GetMaterialById(material.Id);
            if(data == null)
            {
                return null;
            }
            material.Price = data.UnitPrice * material.Quantity;

            return material;
        }

        public static PurchasedProductObject UpdatePurchase(PurchasedProductObject purchase)
        {
            if (purchase == null)
                return null;
            var data = DataService.GetProductById(purchase.Id);
            if (data == null)
            {
                return null;
            }
            purchase.Price = data.UnitPrice * purchase.Quantity;

            return purchase;
        }

        public static int GetSalary(JobObject jobObject, Jobs job)
        {
            if (jobObject == null || job == null)
            {
                return 0;
            }

            double discount = 1 - (double)Math.Min(100, Math.Max(0, jobObject.Discount)) / 100;
            var salary = jobObject.Price * discount - job.Overhead;

            return (int)salary;
        }

        public static int GetJobTotalPriceWithDiscount(IEnumerable<JobObject> jobObjects)
        {
            if (!jobObjects?.Any() ?? true)
            {
                return 0;
            }

            int total = jobObjects.Sum(t => GetJobPriceWithDiscount(t));

            return total;
        }

        public static int GetJobPriceWithDiscount(JobObject jobObject)
        {
            if (jobObject == null)
            {
                return 0;
            }

            double discount = 1 - (double)Math.Min(100, Math.Max(0, jobObject.Discount)) / 100;
            var priceWithDiscount = jobObject.Price * discount;

            return (int)priceWithDiscount;
        }
    }
}
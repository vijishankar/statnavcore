using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatNav.WebApplication.BLL
{
    public static class VariantLogic
    {
        public static IQueryable<Variant> FilterVariants(IQueryable<Variant> variants, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                variants = variants.Where(x => x.VariantName.ToLower().Contains(searchString.ToLower()));
            }
            return variants;
        }
        public static List<Variant> SortVariants(List<Variant> variantList, string sortOrder)
        {
            IOrderedEnumerable<Variant> sortedList;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedList = variantList.OrderByDescending(x => x.VariantName);
                    break;
                case "Id":
                    sortedList = variantList.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    sortedList = variantList.OrderByDescending(x => x.Id);
                    break;
                default:
                    sortedList = variantList.OrderBy(x => x.VariantName);
                    break;
            }
            return sortedList.ToList();
        }
    }
}
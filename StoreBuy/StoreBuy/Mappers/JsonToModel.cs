using Newtonsoft.Json;
using StoreBuy.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace StoreBuy.Mappers
{
    class JsonToModel
    {
       static  List<ItemCategory> R10Categories = new List<ItemCategory>();
        static List<ItemCatalogueModel> R10Items = new List<ItemCatalogueModel>();


        public static List<ItemCategory> MapFields(String CategoryResponse)
        {
            R10Categories.Clear();
            dynamic CategoryJSON = JsonConvert.DeserializeObject<dynamic>(CategoryResponse);

            foreach (var item in CategoryJSON[0].HierarchyCategories)
            {

                if (item.Id != "UnknownHierarchy")
                {
                    var category = item.HierarchyCategories;
                    foreach (var subcategory in category)
                    {
                        ItemCategory tempCategory = new ItemCategory();
                        tempCategory.CategoryId = subcategory.Id;
                        tempCategory.CategoryName = subcategory.Descriptions[0].Value;
                        if(subcategory.Id==82831)
                        tempCategory.R10CategoryImage = "fruits.jpg";
                        else if (subcategory.Id == 82819)
                            tempCategory.R10CategoryImage = "beverages.png";
                       else if (subcategory.Id == 82841)
                            tempCategory.R10CategoryImage = "leafy.png";
                        else if (subcategory.Id == 84353)
                            tempCategory.R10CategoryImage = "fees.png";
                       else if (subcategory.Id == 84356)
                            tempCategory.R10CategoryImage = "wfm.png";
                       else if (subcategory.Id == 84354)
                            tempCategory.R10CategoryImage = "gift.jpg";
                        R10Categories.Add(tempCategory);
                    }


                }
            }
            return R10Categories;
        }


        public static List<ItemCatalogueModel> MapItemFields(String CategoryResponse)
        {
            R10Items.Clear();
            dynamic CategoryJSON = JsonConvert.DeserializeObject<dynamic>(CategoryResponse);
            dynamic ac = CategoryJSON[0];
            foreach (var item in CategoryJSON)
            {
                if (item.Descriptions != null && item.Prices != null)
                {
                    ItemCatalogueModel tempCategory = new ItemCatalogueModel();
                    tempCategory.ItemId = item.MainId;
                    tempCategory.ItemDescription = item.Descriptions[0].Value;
                    tempCategory.ItemName = item.Descriptions[1].Value;
                    tempCategory.Price = item.Prices[0].Value;
                    R10Items.Add(tempCategory);
                }
                   
            }
            return R10Items;
        }

    }
}


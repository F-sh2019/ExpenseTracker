using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Model
{
    class CategoryIcon
    {
        string GlyphValue { get; set;}
        string IconName { get; set; }

        public CategoryIcon(string glyphValue, string iconName)
        {
            GlyphValue = glyphValue;
            IconName = iconName;
        }

        public static List<CategoryIcon> getAllIcons()
        {
            List<CategoryIcon> icons = new List<CategoryIcon>();
            icons.Add(new CategoryIcon("&#xe902;", "HomeIcon"));
            icons.Add(new CategoryIcon("&#xe93a;", "ShoppingIcon"));
            icons.Add(new CategoryIcon("&#xe9af;", "TravelIcon"));
            icons.Add(new CategoryIcon("&#xe9a3;", "FoodIcon"));
            icons.Add(new CategoryIcon("&#xe911;", "EntertainmentIcon"));
            icons.Add(new CategoryIcon("&#xe921;", "EducationIcon"));
            icons.Add(new CategoryIcon("&#xe922;", "BillsIcon"));
            icons.Add(new CategoryIcon("&#xe99f;", "GiftIcon"));
                      
            return icons;
        }
    }
}

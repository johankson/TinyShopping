using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using TinyShopping.Api.Data;
using TinyShopping.Api.Data.Models;
using TinyShopping.Api.Extensions;

namespace TinyShopping.Api.Controllers
{
    [Route("api/lists")]
    public class ShoppingListController : Controller
    {
        ShoppingDbContext db;

        public ShoppingListController(ShoppingDbContext db)
        {
            this.db = db;
        }

        [SwaggerOperation("GetShoppingLists")]
        [HttpGet(Name = "GetShoppingLists")]
        public IEnumerable<ShoppingList> GetLists()
        {
            var lists = db.Lists;
            var items = db.Items;

            foreach (var list in lists)
            {
                list.NumberOfItems = items.Count(e => e.ListId == list.ID);
                list.NumberOfCompletedItems = items.Count(e => e.ListId == list.ID && e.Completed == true);
            }

            return lists;
        }

        [SwaggerOperation("GetShoppingList")]
        [HttpGet("{id}", Name = "GetShoppingList")]
        public ShoppingList GetList(int id)
        {
            return db.Lists.FirstOrDefault(d => d.ID == id);
        }

        // [SwaggerOperation("AddShoppingList")]
        // [HttpPost(Name = "AddShoppingList")]
        // public ShoppingList Add([FromBody]ShoppingList listData)
        // {
        //     db.Lists.Add(listData);
        //     db.SaveChangesAsync();
        //     return listData;
        // }

        [SwaggerOperation("UpdateShoppingList")]
        [HttpPut(Name = "UpdateShoppingList")]
        public ShoppingList Update([FromBody]ShoppingList listData)
        {
            var list = db.Lists.FirstOrDefault(d => d.ID == listData.ID);
            if (list != null) {
                listData.MemberviseCopyTo(list);
            }
            else {
                list = listData;
                db.Lists.Add(list);
            }
            db.SaveChanges();
            return list;
        }

        [SwaggerOperation("DeleteShoppingList")]
        [HttpDelete("{id}", Name = "DeleteShoppingList")]
        public bool Delete(int id)
        {
            var item = db.Lists.FirstOrDefault(d => d.ID == id);
            if (item!=null) {
                db.Lists.Remove(item);
                db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Items
        [SwaggerOperation("GetListItems")]
        [HttpGet("{listid}/items", Name = "GetListItems")]
        public IEnumerable<Item> GetItemsList(int listid)
        {
            return db.GetListItems(listid);
        }
    }
}

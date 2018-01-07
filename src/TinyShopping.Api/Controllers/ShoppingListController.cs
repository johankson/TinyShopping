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

        [SwaggerOperation("AddShoppingList")]
        [HttpPost(Name = "AddShoppingList")]
        public void Add([FromBody]ShoppingList listData)
        {
            db.Lists.Add(listData);
            db.SaveChangesAsync();
        }

        [SwaggerOperation("UpdateShoppingList")]
        [HttpPut("{id}", Name = "UpdateShoppingList")]
        public void Update(int id, [FromBody]ShoppingList listData)
        {
            var list = db.Lists.FirstOrDefault(d => d.ID == id);
            listData.MemberviseCopyTo(list);
            db.SaveChangesAsync();
        }

        [SwaggerOperation("DeleteShoppingList")]
        [HttpDelete("{id}", Name = "DeleteShoppingList")]
        public void Delete(int id)
        {
            var item = db.Lists.FirstOrDefault(d => d.ID == id);
            db.Lists.Remove(item);
            db.SaveChangesAsync();
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

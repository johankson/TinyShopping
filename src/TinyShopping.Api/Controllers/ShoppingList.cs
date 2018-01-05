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
    [Route("api/[controller]")]
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
            return db.Lists;
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
        public IEnumerable<Item> GetItemsList(int id)
        {
            return db.GetListItems(id);
        }

        [SwaggerOperation("GetListItem")]
        [HttpGet("{listid}/items/{id}", Name = "GetListItem")]
        public Item GetItemList(int id)
        {
            return db.Items.FirstOrDefault(d => d.ID == id);
        }

        [SwaggerOperation("AddListItem")]
        [HttpPost("{listid}/items", Name = "AddListItem")]
        public void AddItem([FromBody]Item itemData)
        {
            db.Items.Add(itemData);
            db.SaveChangesAsync();
        }

        [SwaggerOperation("UpdateListItem")]
        [HttpPut("{listid}/items/{id}", Name = "UpdateListItem")]
        public void UpdateItem(int id, [FromBody]Item itemData)
        {
            var item = db.Lists.FirstOrDefault(d => d.ID == id);
            itemData.MemberviseCopyTo(item);
            db.SaveChangesAsync();
        }

        [SwaggerOperation("DeleteListItem")]
        [HttpDelete("{listid}/items/{id}", Name = "DeleteListItem")]
        public void DeleteItem(int id)
        {
            var item = db.Items.FirstOrDefault(d => d.ID == id);
            db.Items.Remove(item);
            db.SaveChangesAsync();
        }
    }
}

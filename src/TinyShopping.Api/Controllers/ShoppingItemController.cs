using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using TinyShopping.Api.Data;
using TinyShopping.Api.Data.Models;
using TinyShopping.Api.Extensions;

namespace TinyShopping.Api.Controllers
{
    [Route("api/items")]
    public class ShoppingItemController : Controller
    {
        ShoppingDbContext db;

        public ShoppingItemController(ShoppingDbContext db)
        {
            this.db = db;
        }
        [SwaggerOperation("GetListItem")]
        [HttpGet("{id}", Name = "GetListItem")]
        public Item GetItemList(int id)
        {
            return db.Items.FirstOrDefault(d => d.ID == id);
        }

        // [SwaggerOperation("AddListItem")]
        // [HttpPost("items", Name = "AddListItem")]
        // public Item AddItem([FromBody]Item itemData)
        // {
        //     db.Items.Add(itemData);
        //     db.SaveChangesAsync();
        //     return itemData;
        // }

        [SwaggerOperation("UpdateListItem")]
        [HttpPut(Name = "UpdateListItem")]
        public Item UpdateItem([FromBody]Item itemData)
        {
            var item = db.Items.FirstOrDefault(d => d.ID == itemData.ID);
            if (item != null) {
                itemData.MemberviseCopyTo(item);
            }
            else {
                item = itemData;
                db.Items.Add(item);
            }
            db.SaveChanges();
            return item;
        }

        [SwaggerOperation("DeleteListItem")]
        [HttpDelete("{id}", Name = "DeleteListItem")]
        public bool DeleteItem(int id)
        {
            var item = db.Items.FirstOrDefault(d => d.ID == id);
            if (item!=null) {
                db.Items.Remove(item);
                db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

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
        [HttpGet("items/{id}", Name = "GetListItem")]
        public Item GetItemList(int id)
        {
            return db.Items.FirstOrDefault(d => d.ID == id);
        }

        [SwaggerOperation("AddListItem")]
        [HttpPost("items", Name = "AddListItem")]
        public void AddItem([FromBody]Item itemData)
        {
            db.Items.Add(itemData);
            db.SaveChangesAsync();
        }

        [SwaggerOperation("UpdateListItem")]
        [HttpPut("items/{id}", Name = "UpdateListItem")]
        public void UpdateItem(int id, [FromBody]Item itemData)
        {
            var item = db.Lists.FirstOrDefault(d => d.ID == id);
            itemData.MemberviseCopyTo(item);
            db.SaveChangesAsync();
        }

        [SwaggerOperation("DeleteListItem")]
        [HttpDelete("items/{id}", Name = "DeleteListItem")]
        public void DeleteItem(int id)
        {
            var item = db.Items.FirstOrDefault(d => d.ID == id);
            db.Items.Remove(item);
            db.SaveChangesAsync();
        }
    }
}

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
    [Route("api/store")]
    public class StoreController : Controller
    {
        ShoppingDbContext db;

        public StoreController(ShoppingDbContext db)
        {
            this.db = db;
        }

        [SwaggerOperation("GetStores")]
        [HttpGet(Name = "GetStores")]
        public IEnumerable<ShoppingList> GetStores()
        {
            return db.Lists;
        }

        [SwaggerOperation("GetStore")]
        [HttpGet("{id}", Name = "GetStore")]
        public ShoppingList GetList(int id)
        {
            return db.Lists.FirstOrDefault(d => d.ID == id);
        }

        [SwaggerOperation("AddStore")]
        [HttpPost(Name = "AddStore")]
        public void Add([FromBody]ShoppingList listData)
        {
            db.Lists.Add(listData);
            db.SaveChangesAsync();
        }

        [SwaggerOperation("UpdateStore")]
        [HttpPut("{id}", Name = "UpdateStore")]
        public void Update(int id, [FromBody]ShoppingList listData)
        {
            var list = db.Lists.FirstOrDefault(d => d.ID == id);
            listData.MemberviseCopyTo(list);
            db.SaveChangesAsync();
        }

        [SwaggerOperation("DeleteStore")]
        [HttpDelete("{id}", Name = "DeleteStore")]
        public void Delete(int id)
        {
            var item = db.Lists.FirstOrDefault(d => d.ID == id);
            db.Lists.Remove(item);
            db.SaveChangesAsync();
        }


    }
}

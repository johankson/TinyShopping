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
        public IEnumerable<Store> GetStores()
        {
            return db.Stores;
        }

        [SwaggerOperation("GetStore")]
        [HttpGet("{id}", Name = "GetStore")]
        public Store GetList(int id)
        {
            return db.Stores.FirstOrDefault(d => d.ID == id);
        }

        // [SwaggerOperation("AddStore")]
        // [HttpPost(Name = "AddStore")]
        // public Store Add([FromBody]Store listData)
        // {
        //     db.Stores.Add(listData);
        //     db.SaveChangesAsync();
        //     return listData;
        // }

        [SwaggerOperation("UpdateStore")]
        [HttpPut("{id}", Name = "UpdateStore")]
        public Store Update([FromBody]Store listData)
        {
            var store = db.Stores.FirstOrDefault(d => d.ID == listData.ID);
            if (store!=null) {
                listData.MemberviseCopyTo(store);
            }
            else {
                store = listData;
                db.Stores.Add(store);
            }
            db.SaveChanges();
            return store;
        }

        [SwaggerOperation("DeleteStore")]
        [HttpDelete("{id}", Name = "DeleteStore")]
        public void Delete(int id)
        {
            var item = db.Stores.FirstOrDefault(d => d.ID == id);
            db.Stores.Remove(item);
            db.SaveChangesAsync();
        }


    }
}

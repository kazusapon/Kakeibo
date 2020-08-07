using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eri.Models
{
    class ConnectionInformation
    {

        public bool IsCountZero()
        {
            int cnt = GetConnectionInfoAllList().Count;
            return cnt == 0;
        }
        
        public bool IsRegisted()
        {

            var result = GetConnectionInfoSingleOrDefault();
            if (result == null ||  result.Ip_Address == "" || result.Ip_Address == null || result.Port < 1)
            {
                return false;
            }

            return true;
        }

        public string BuildURLHostname()
        {
            var result = this.GetConnectionInfoSingleOrDefault();

            StringBuilder sb = new StringBuilder();
            sb.Append("http://");
            sb.Append(result.Ip_Address);
            sb.Append(":");
            sb.Append(result.Port.ToString());
            sb.Append("/");

            return sb.ToString();
        }

        public Mst_ConnectionInfo GetConnectionInfoSingleOrDefault()
        {
            using (var db = new MyContext())
            {
                return db.Mst_ConnectionInfo.OrderBy(x => x.Id).SingleOrDefault();
            }
        }

        public async  Task<bool> InsertOrUpdateAsync(Mst_ConnectionInfo view)
        {
            if (view.Ip_Address == null || view.Ip_Address == "" || view.Port == 0)
            {
                return false;
            }

            if (IsCountZero())
            {
                await InsertConnetionInfo(view);
            }
            else
            {
                await UpdateConnetionInfo(view);
            }

            return true;
            
        }

        private List<Mst_ConnectionInfo> GetConnectionInfoAllList()
        {
            using (var db = new MyContext())
            {
                return db.Mst_ConnectionInfo.OrderBy(x => x.Id).ToList();
            }
        }

        private async Task InsertConnetionInfo(Mst_ConnectionInfo view)
        {
            using (var db = new MyContext())
            {
                db.Mst_ConnectionInfo.Add(new Mst_ConnectionInfo
                {
                    Ip_Address = view.Ip_Address,
                    Port = view.Port
                });

                await db.SaveChangesAsync();
            }
        }

        private async Task UpdateConnetionInfo(Mst_ConnectionInfo view)
        {
            using (var db = new MyContext())
            {
                var result = GetConnectionInfoSingleOrDefault();

                result.Ip_Address = view.Ip_Address;
                result.Port = view.Port;

                await db.SaveChangesAsync();
            }
        }
    }
}

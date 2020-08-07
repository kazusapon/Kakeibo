using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Eri.Models
{
    class Fixed
    {
        private MyContext _context;

        public Fixed(MyContext context)
        {
            this._context = context;
        }

        public void MstFiexdDelete()
        {
            if (this._context.Mst_Fixed.Count() == 0)
            {
                return;
            }

            var result = this._context.Mst_Fixed.ToList();
            this._context.RemoveRange(result);

            this._context.SaveChanges();
        }

        public async Task FixedMasterInsertAllAsync(List<Mst_Fixed> fixeds)
        {
            await this._context.Mst_Fixed.AddRangeAsync(fixeds);
            await this._context.SaveChangesAsync();
        }

    }
}

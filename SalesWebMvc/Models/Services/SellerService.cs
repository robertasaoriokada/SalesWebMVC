using SalesWebMvc.Data;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models.Services.Exceptions;
namespace SalesWebMvc.Models.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj =>obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                Seller obj = await _context.Seller.FindAsync(id);
                if (obj == null)
                {
                    return;

                }
                _context.Seller.Remove(obj);

                await _context.SaveChangesAsync();
            } catch(DbUpdateException)
            {
                throw new IntegrityException("Seller cannot be deleted beucause them have sales");
               
            }
            
        }

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");

            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
            
        }

       
    }
}

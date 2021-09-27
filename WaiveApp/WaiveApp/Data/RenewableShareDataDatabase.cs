using System.Collections.Generic;
using System.Threading.Tasks;
using WaiveApp.Models;

namespace WaiveApp.Data
{
    //public class RenewableShareDataDatabase
    //{
    //
    //    readonly SQLiteAsyncConnection database;
    //
    //    public RenewableShareDataDatabase(string dbPath)
    //    {
    //        database = new SQLiteAsyncConnection(dbPath);
    //        database.CreateTableAsync<RenewableShareData>().Wait();
    //    }
    //
    //    public Task<List<RenewableShareData>> GetRenewableShareDataAsync()
    //    {
    //        //Get all data.
    //        return database.Table<RenewableShareData>().ToListAsync();
    //    }
    //
    //    public Task<RenewableShareData> GetRenewableShareDataAsync(int id)
    //    {
    //        // Get a specific data.
    //        return database.Table<RenewableShareData>()
    //                        .Where(i => i.id == id)
    //                        .FirstOrDefaultAsync();
    //    }
    //
    //    public Task<int> SaveRenewableShareDataAsync(RenewableShareData data)
    //    {
    //        if (data.id != 0)
    //        {
    //            // Update an existing note.
    //            return database.UpdateAsync(data);
    //        }
    //        else
    //        {
    //            // Save a new note.
    //            return database.InsertAsync(data);
    //        }
    //    }
    //
    //    public Task<int> DeleteRenewableShareDataAsync(RenewableShareData data)
    //    {
    //        // Delete a note.
    //        return database.DeleteAsync(data);
    //    }
    //  
    //}
}

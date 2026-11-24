using SQLite;
using ProyectoFinal.Models;

namespace ProyectoFinal.Data
{
    public class DatabaseContext
    {
        private SQLiteAsyncConnection _database;

        private async Task Init()
        {
            if (_database is not null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Kutuni.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<Producto>();
        }

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            await Init();
            return await _database.Table<Producto>().ToListAsync();
        }

        public async Task<int> GuardarProductoAsync(Producto producto)
        {
            await Init();
            if (producto.Id != 0)
                return await _database.UpdateAsync(producto);
            else
                return await _database.InsertAsync(producto);
        }

        public async Task<int> EliminarProductoAsync(Producto producto)
        {
            await Init();
            return await _database.DeleteAsync(producto);
        }
    }
}
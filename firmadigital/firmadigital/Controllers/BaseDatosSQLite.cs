using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using firmadigital.Models;
using System.Threading.Tasks;

namespace firmadigital.Controllers
{
    public class BaseDatosSQLite
    {
        readonly SQLiteAsyncConnection db;

        //Constructor de la clase DataBaseSQLite
        public BaseDatosSQLite(String pathdb)
        {
            //Crear una conexion a la base de datos
            db = new SQLiteAsyncConnection(pathdb);

            //Creacion de la tabla personas dentro de SQLite
            db.CreateTableAsync<Firma>().Wait();
        }

        // Operaciones CRUD con SQLite
        //READ List Way
        public Task<List<Firma>> ObtenerListaFirmas()
        {
            return db.Table<Firma>().ToListAsync();
        }

        //READ one by one
        public Task<Firma> ObtenerFirma(int pcodigo)
        {
            return db.Table<Firma>()
                .Where(i => i.codigo == pcodigo)
                .FirstOrDefaultAsync();
        }

        //CREATE firma
        public Task<int> GrabarFirma(Firma firma)
        {
            if (firma.codigo != 0)
            {
                return db.UpdateAsync(firma);
            }
            else
            {
                return db.InsertAsync(firma);
            }
        }

        //Delete

        public Task<int> EliminarFirma(Firma firma)
        {
            return db.DeleteAsync(firma);

        }

    }
}

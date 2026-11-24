using SQLite;

namespace ProyectoFinal.Models;

public class Producto
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Artesano { get; set; }
    public decimal Precio { get; set; }
    public string Imagen { get; set; }

    [Ignore]
    public string ImagenRuta
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Imagen) && File.Exists(Imagen))
                return Imagen;

            return "dotnet_bot.png";
        }
    }
}
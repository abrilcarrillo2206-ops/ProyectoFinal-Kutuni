using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Views;

public partial class CatalogoPage : ContentPage
{
    private DatabaseContext _db = new DatabaseContext();
    private List<Producto> _todosLosProductos = new List<Producto>();

    // Lista compartida en memoria para los productos que el cliente va pidiendo
    public static List<Producto> CarritoPedido { get; set; } = new List<Producto>();

    public CatalogoPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _todosLosProductos = await _db.ObtenerProductosAsync();
        listaCatalogo.ItemsSource = _todosLosProductos;
    }

    // Evento TextChanged para el buscador en tiempo real
    private void OnBuscarTextChanged(object sender, TextChangedEventArgs e)
    {
        var texto = e.NewTextValue?.ToLower() ?? "";
        listaCatalogo.ItemsSource = _todosLosProductos
            .Where(p => p.Nombre.ToLower().Contains(texto) || p.Artesano.ToLower().Contains(texto))
            .ToList();
    }

    // Evento Clicked para agregar el producto al pedido
    private async void OnAgregarPedidoClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        var producto = boton?.BindingContext as Producto;

        if (producto != null)
        {
            CarritoPedido.Add(producto);
            await DisplayAlert("Agregado", $"Se agregó '{producto.Nombre}' a tu pedido.", "OK");
        }
    }
}
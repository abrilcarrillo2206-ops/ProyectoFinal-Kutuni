using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Views;

public partial class AdminPage : ContentPage
{
    private DatabaseContext _db = new DatabaseContext();
    private Producto _productoSeleccionado;

    public AdminPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarProductos();
    }

    private async Task CargarProductos()
    {
        listaAdmin.ItemsSource = await _db.ObtenerProductosAsync();
    }

    // Seleccionar foto desde la galería del teléfono
    private async void OnSeleccionarFotoClicked(object sender, EventArgs e)
    {
        try
        {
            var foto = await MediaPicker.Default.PickPhotoAsync();
            if (foto != null)
            {
                var rutaLocal = Path.Combine(FileSystem.AppDataDirectory, foto.FileName);
                using var streamOrigen = await foto.OpenReadAsync();
                using var streamDestino = File.OpenWrite(rutaLocal);
                await streamOrigen.CopyToAsync(streamDestino);

                txtImagen.Text = rutaLocal;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Aviso", "No se pudo cargar la foto: " + ex.Message, "OK");
        }
    }

    // CREATE y UPDATE (Evento Clicked)
    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
        {
            await DisplayAlert("Aviso", "Escribe al menos el nombre y el precio.", "OK");
            return;
        }

        var prod = _productoSeleccionado ?? new Producto();
        prod.Nombre = txtNombre.Text;
        prod.Artesano = string.IsNullOrWhiteSpace(txtArtesano.Text) ? "Artesano local" : txtArtesano.Text;
        prod.Precio = decimal.TryParse(txtPrecio.Text, out decimal precio) ? precio : 0;

        if (string.IsNullOrWhiteSpace(txtImagen.Text))
        {
            prod.Imagen = "morral";
        }
        else if (File.Exists(txtImagen.Text.Trim()))
        {
            prod.Imagen = txtImagen.Text.Trim();
        }
        else
        {
            prod.Imagen = txtImagen.Text.Trim().ToLower().Replace(".jpg", "").Replace(".png", "");
        }

        await _db.GuardarProductoAsync(prod);
        LimpiarFormulario();
        await CargarProductos();
        await DisplayAlert("Listo", "Artesanía guardada en SQLite.", "OK");
    }

    // DELETE (Evento Clicked)
    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (_productoSeleccionado == null)
        {
            await DisplayAlert("Aviso", "Toca primero un producto de la lista.", "OK");
            return;
        }

        await _db.EliminarProductoAsync(_productoSeleccionado);
        LimpiarFormulario();
        await CargarProductos();
        await DisplayAlert("Eliminado", "Producto borrado.", "OK");
    }

    // Seleccionar producto al tocar su tarjeta en Android
    private void OnTarjetaTocada(object sender, TappedEventArgs e)
    {
        if (sender is VisualElement elemento && elemento.BindingContext is Producto prod)
        {
            _productoSeleccionado = prod;
            txtNombre.Text = prod.Nombre;
            txtArtesano.Text = prod.Artesano;
            txtPrecio.Text = prod.Precio.ToString();
            txtImagen.Text = prod.Imagen;
        }
    }

    private void OnLimpiarClicked(object sender, EventArgs e)
    {
        LimpiarFormulario();
    }

    private void LimpiarFormulario()
    {
        _productoSeleccionado = null;
        txtNombre.Text = string.Empty;
        txtArtesano.Text = string.Empty;
        txtPrecio.Text = string.Empty;
        txtImagen.Text = string.Empty;
    }
}
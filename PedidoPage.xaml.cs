namespace ProyectoFinal.Views;

public partial class PedidoPage : ContentPage
{
    public PedidoPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ActualizarVistaPedido();
    }

    private void ActualizarVistaPedido()
    {
        listaPedido.ItemsSource = null;
        listaPedido.ItemsSource = CatalogoPage.CarritoPedido;

        decimal total = CatalogoPage.CarritoPedido.Sum(p => p.Precio);
        lblTotal.Text = $"Total a pagar: ${total:F2}";
    }

    // Evento Clicked para vaciar el carrito
    private void OnVaciarClicked(object sender, EventArgs e)
    {
        CatalogoPage.CarritoPedido.Clear();
        ActualizarVistaPedido();
    }

    // Evento Clicked para confirmar el pedido
    private async void OnConfirmarClicked(object sender, EventArgs e)
    {
        if (CatalogoPage.CarritoPedido.Count == 0)
        {
            await DisplayAlert("Aviso", "Tu pedido está vacío. Agrega artesanías desde el Catálogo.", "OK");
            return;
        }

        await DisplayAlert("¡Pedido Confirmado!", "Gracias por apoyar el talento local de Kutuni.", "OK");
        CatalogoPage.CarritoPedido.Clear();
        ActualizarVistaPedido();
    }
}
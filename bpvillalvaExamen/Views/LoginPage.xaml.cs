
namespace bpvillalvaExamen.Views;

public partial class LoginPage : ContentPage
{
    private readonly string[,] usuarios =
    {
        { "estudiante", "moviles" },
        { "suda", "2025" }
    };

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void IniciarSesion_Clicked(
        object sender, EventArgs e)
    {
        string usuario = UsuarioEntry.Text?.Trim() ?? "";
        string clave = ClaveEntry.Text ?? "";

        bool correcto = false;

        for (int i = 0; i < 2; i++)
        {
            if (usuarios[i, 0] == usuario &&
                usuarios[i, 1] == clave)
            {
                correcto = true;
                break;
            }
        }

        if (!correcto)
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "Aceptar");
            return;
        }

        await Navigation.PushAsync(new RegistroPage(usuario));
    }

}

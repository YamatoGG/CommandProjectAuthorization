using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using WebBaza;
using WebBaza.Classes;
using WpfApp3.Views;

namespace LoginWPF;

public class LoginViewModel : INotifyPropertyChanged
{
    private HttpClient _httpClient;
    public event EventHandler RequestClose;
    public string? Login
    {
        get;
        set => SetField(ref field, value);
    }

    public string? Password
    {
        get;
        set => SetField(ref field, value);
    }
    public string? Status
    {
        get;
        set => SetField(ref field, value);
    }
    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    private void OnRequestClose()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
    public LoginViewModel()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5196");
        LoginCommand = new LambdaCommand(async _ => await LoginAsync(), _ => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password));
        RegisterCommand = new LambdaCommand(async _ => await RegisterAsync(), _ => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password));
    }

    private async Task LoginAsync()
    {
        Status = null;
        var loginData = new Person() { Login = Login!, Password = Password! };
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/login", loginData);
            if (response.IsSuccessStatusCode)
            {
                var window = new ProductsWindow();
                window.Show();
                OnRequestClose();
            }
            else
            {
                Status = "Неверный логин или пароль";
            }
        }
        catch (Exception ex)
        {
            Status = $"Ошибка: {ex.Message}";
        }
    }
    private async Task RegisterAsync()
    {
        Status = null;
        var loginData = new Person() { Login = Login!, Password = Password! };
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/register", loginData);
            if (response.IsSuccessStatusCode)
            {
                var window = new ProductsWindow();
                window.Show();
                OnRequestClose();
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                Status = $"Ошибка регистрации: {errorMsg}";
            }
        }
        catch (Exception ex)
        {
            Status = $"Ошибка: {ex.Message}";
        }
    }
    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}
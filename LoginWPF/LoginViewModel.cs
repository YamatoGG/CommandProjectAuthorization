using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using WebBaza;
using WebBaza.Classes;

namespace LoginWPF;

public class LoginViewModel : INotifyPropertyChanged
{
    public event EventHandler RequestClose;
    private DataBaseContext _dataContext;
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
        _dataContext = new DataBaseContext(new DbContextOptions<DataBaseContext>());
        LoginCommand = new LambdaCommand(async _ => await LoginAsync(), _ => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password));
        RegisterCommand = new LambdaCommand(async _ => await RegisterAsync(), _ => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password));
    }

    private async Task LoginAsync()
    {
        Status = null;
        var user = await _dataContext.People.FirstOrDefaultAsync(u => u.Login == Login);
        if (user != null && user.Password == Password)
        {
            var window = new MainWindow();
            window.Show();
            OnRequestClose();
        }
        else
        {
            Status = "Неверный логин или пароль";
        }
    }
    private async Task RegisterAsync()
    {
        Status = null;
        if (await _dataContext.People.AnyAsync(u => u.Login == Login))
        {
            Status = "Этот логин уже занят";
            return;
        }
        var rnd = new Random();
        int newId;
        do
        {
            newId = rnd.Next();
        } while (_dataContext.People.Any(u => u.Id == newId));
        var newUser = new Person()
        {
            Id = newId,
            Login = Login!,
            Password = Password!
        };
        _dataContext.People.Add(newUser);
        await _dataContext.SaveChangesAsync();
        var window = new MainWindow();
        window.Show();
        OnRequestClose();
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
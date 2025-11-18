//Задача 15. Система нотификаций. Паттерн: Decorator. (Андрюшин)
//Создать систему уведомлений, где можно комбинировать различные 
//способы отправки (email, SMS, push) и добавлять дополнительные 
//функции (логирование, шифрование).

public interface INotification
{
    void Send(string message);
}

public class EmailNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"Отправка Email: {message}");
    }
}

public class SmsNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"Отправка SMS: {message}");
    }
}

public class PushNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"Отправка сообщения: {message}");
    }
}

public abstract class NotificationDecorator : INotification
{
    protected INotification Wrappee;

    protected NotificationDecorator(INotification wrappee)
    {
        Wrappee = wrappee;
    }

    public virtual void Send(string message)
    {
        Wrappee.Send(message);
    }
}

public class LoggingDecorator : NotificationDecorator
{
    public LoggingDecorator(INotification wrappee) : base(wrappee) { }

    public override void Send(string message)
    {
        Console.WriteLine($"Отправка сообщения: {message}");
        base.Send(message);
    }
}

public class EncryptionDecorator : NotificationDecorator
{
    public EncryptionDecorator(INotification wrappee) : base(wrappee) { }

    private string Encrypt(string message)
    {
        char[] chars = message.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
            chars[i] = (char)(chars[i] + 1);
        return new string(chars);
    }

    public override void Send(string message)
    {
        string encryptedMessage = Encrypt(message);
        Console.WriteLine("--=Шифрование=--");
        base.Send(encryptedMessage);
    }
}

//Пример

class Program
{
    static void Main()
    {
        INotification notification = new EmailNotification();
        notification = new EncryptionDecorator(notification);
        notification = new LoggingDecorator(notification);
        notification.Send("Ку-ку епта");
        Console.WriteLine("\n--=Логирование=--");
        INotification smsNotification = new SmsNotification();
        smsNotification = new LoggingDecorator(smsNotification);
        smsNotification.Send("Дарова корова");
        Console.WriteLine("\n--=Шифрование и логирование=--");
        INotification pushNotification = new PushNotification();
        pushNotification = new EncryptionDecorator(pushNotification);
        pushNotification = new LoggingDecorator(pushNotification);
        pushNotification.Send("Привет медвед");
    }
}
namespace Hastnama.Solico.Common.TemplateNotification
{
    public interface INotificationTemplateGenerator
    {
        string CreateConfirmCode(ConfirmCodeTemplate confirmCode);
        
    }
}
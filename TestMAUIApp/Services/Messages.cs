using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TestMAUIApp.Services
{
    public class RefreshMessage(bool value) : ValueChangedMessage<bool>(value)
    {
    }
}

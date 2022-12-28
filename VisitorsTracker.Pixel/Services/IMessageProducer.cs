namespace Pixel.Services;

public interface IMessageProducer
{
    public void SendMessage<T>(T message);
}

namespace WebHooks.Contracts;

public interface IWebHookCallManagerFactory
{
    IWebHookCallManager GetNew();
}
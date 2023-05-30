# YggdrAshill.Ragnarok.Unity

This framework is an extension of [YggdrAshill.Ragnarok](https://github.com/do-i-know-it/YggdrAshill.Ragnarok) for [Unity](https://unity.com/ja).

## Dependencies

This framework also depends on below.

- .NET Standard 2.0
- [Unity](https://unity.com/ja) 2020.3.x
- [YggdrAshill.Ragnarok](https://github.com/do-i-know-it/YggdrAshill.Ragnarok) 0.9.0

## Installation

This framework depends on Unity after Unity 2020.3 that supports path query parameter of git package.
Developers can install this framework with [UPM](https://docs.unity3d.com/Manual/Packages.html) like:

- Add `https://github.com/do-i-know-it/YggdrAshill.Ragnarok.Unity?path=Assets/YggdrAshill.Ragnarok.Unity` to Package Manager.
- Add `"com.yggdrashill.ragnarok.unity": "https://github.com/do-i-know-it/YggdrAshill.Ragnarok.Unity?path=Assets/YggdrAshill.Ragnarok.Unity"` to `Packages/manifest.json`.

## Usage

Basic usage for dependency injection is same as [YggdrAshill.Ragnarok](https://github.com/do-i-know-it/YggdrAshill.Ragnarok).

Using our extension, you can register dependencies as below:
```cs
interface ISender
{
    string Send();
}

interface IReceiver
{
    void Receive(string message);
}

class Service : IPreUpdatable
{
    private readonly ISender sender;
    private readonly IReciever receiver;

    [Inject]
    Service(ISender sender, IReceiver receiver)
    {
        this.sender = sender;
        this.receiver = receiver;
    }

    // Executed before Update function of MonoBeahviour.
    public void PreUpdate()
    {
        var message = sender.Send();

        receiver.Receive(message);
    }
}
```
using implementations as below:

```cs
class ConsoleSender : MonoBehaviour, ISender
{
    [SerializeField]
    InputField inputField;

    [InjectField]
    readonly string? announcement;

    public string Send()
    {
        if (announcement != null)
        {
            Console.Write($"{announcement}:");
        }

        return inputField.text;
    }
}

class ConsoleReceiver : MonoBehaviour, IReceiver
{
    [SerializeField]
    Text outputText;

    [InjectField]
    readonly string? header;

    public void Receive(string message)
    {
        outputText.text = header == null ? message : $"{header}: {message}";
    }
}
```

like:
```cs
sealed class ServiceEntryPoint : MonoEntryPoint
{
    protected override void Configure(IContainer container)
    {
        // Register ConsoleSender as ISender to instantiate per global scope.
        container.RegisterComponentOnNewGameObject<ConsoleSender>(Lifetime.Global)
            .WithFieldsInjected() // Enable field injection.
            .From("announcement", "Enter any text") // Add parameter to inject into fields.
            .As<ISender>();
        // Register ConsoleReceiver as IReceiver to instantiate per global scope.
        container.RegisterComponentOnNewGameObject<ConsoleReceiver>(Lifetime.Global)
            .WithFieldsInjected() // Enable field injection.
            .From("header", "Recieved") // Add parameter to inject into fields.
            .As<IReceiver>();
        // Register Service as entry point in unity event loop.
        container.Register<Service>(Lifetime.Global).AsImplementedInterfaces;
    }
}
```

You can resolve dependency registering `ServiceEntryPoint` to `GameObjectLifecycle` or `SceneLifecycle` or `ProjectLifecycle`.

## Known issues

Please see [issues](https://github.com/do-i-know-it/YggdrAshill.Ragnarok.Unity/issues).

## Future works

Please see [GitHub Project for road map](https://github.com/do-i-know-it/YggdrAshill.Ragnarok.Unity/projects/1).

- Auto wiring to use this framework in ease.
- Diagnostics in Editor.

## License

This framework is under the MIT License, see [LICENSE](./LICENSE.md).

## Remarks

This extension is a part of YggdrAshill framework.
Other frameworks will be produced soon for YggdrAshill.

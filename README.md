# YggdrAshill.Ragnarok.Unity

![GitHub](https://img.shields.io/github/license/do-i-know-it/YggdrAshill.Ragnarok.Unity)
![GitHub last commit](https://img.shields.io/github/last-commit/do-i-know-it/YggdrAshill.Ragnarok.Unity)
![GitHub repo size](https://img.shields.io/github/repo-size/do-i-know-it/YggdrAshill.Ragnarok.Unity)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/do-i-know-it/YggdrAshill.Ragnarok.Unity)
<!-- ![GitHub Release Date](https://img.shields.io/github/release-date/do-i-know-it/YggdrAshill.Ragnarok.Unity) -->

This framework is an extension of [YggdrAshill.Ragnarok](https://github.com/do-i-know-it/YggdrAshill.Ragnarok) for [Unity](https://unity.com/ja).

## Dependencies

This framework also depends on below.

- .NET Standard 2.1
- [Unity](https://unity.com/ja) 2021.3.x
- [YggdrAshill.Ragnarok](https://github.com/do-i-know-it/YggdrAshill.Ragnarok) 0.10.2

## Installation

This framework depends on Unity after Unity 2020.3 that supports path query parameter of git package.
Developers can install this framework with [UPM](https://docs.unity3d.com/Manual/Packages.html) like:

- Add `https://github.com/do-i-know-it/YggdrAshill.Ragnarok.Unity.git?path=Assets/YggdrAshill.Ragnarok.Unity` to Package Manager.
- Add `"com.yggdrashill.ragnarok.unity": "https://github.com/do-i-know-it/YggdrAshill.Ragnarok.Unity.git?path=Assets/YggdrAshill.Ragnarok.Unity"` to `Packages/manifest.json`.

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
    readonly ISender sender;
    readonly IReciever receiver;

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
    InputField? inputField;

    public string Send()
    {
        return inputField.text;
    }
}

class ConsoleReceiver : MonoBehaviour, IReceiver
{
    [SerializeField]
    Text? outputText;

    public void Receive(string message)
    {
        outputText.text = message;
    }
}
```

like:
```cs
sealed class ServiceInstallation : MonoInstallation
{
    public override void Install(IObjectContainer container)
    {
        // Register ConsoleSender as ISender to instantiate per global scope.
        container.RegisterComponentOnNewGameObject<ISender, ConsoleSender>(Lifetime.Global);
        // Register ConsoleReceiver as IReceiver to instantiate per global scope.
        container.RegisterComponentOnNewGameObject<IReceiver, ConsoleReceiver>(Lifetime.Global);
        // Register Service's interfaces for unity event loop.
        container.RegisterEntryPoint<Service>();
    }
}
```

You can resolve dependency registering `ServiceMonoInstallation` to `GameObjectLifecycle` or `SceneLifecycle` or `ProjectLifecycle`.

### __Entry Point for installation__

`MonoEntryPoint` like

- `ImplicitMonoEntryPoint`
- `ExplicitMonoEntryPoint`

is `MonoInstallation` collecting other `MonoInstallation`s to register dependencies in ease.

## Known issues

Please see [issues](https://github.com/do-i-know-it/YggdrAshill.Ragnarok.Unity/issues).

## Future works

Please see [GitHub Project for road map](https://github.com/do-i-know-it/YggdrAshill.Ragnarok.Unity/projects/1).

- Implementation for `IRootResolver` for Unity.
- Auto wiring to use this framework in ease.
- Diagnostics in Editor.

## License

This framework is under the MIT License, see [LICENSE](./LICENSE.md).

## Remarks

This extension is a part of YggdrAshill framework.
Other frameworks will be produced soon for YggdrAshill.

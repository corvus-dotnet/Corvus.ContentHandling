# Content Handling

A common problem in extensible applications is the need to dispatch an instance of a polymorphic type to a handler appropriate to that type.

For example, in a UI tool, we might define a UI element with an interface like this:

```cs --session "Dispatch to a handler" --region uielement --source-file ./Corvus.ContentHandling.DemoHandlerWithClasses/UIElements/IUIElement.cs --project ./Corvus.ContentHandling.DemoHandlerWithClasses/Corvus.ContentHandling.DemoHandlerWithClasses.csproj
```

We might find concrete implementations of that interface to define buttons and text boxes.

```cs --session "Dispatch to a handler" --region button --source-file ./Corvus.ContentHandling.DemoHandlerWithClasses/UIElements/Button.cs --project ./Corvus.ContentHandling.DemoHandlerWithClasses/Corvus.ContentHandling.DemoHandlerWithClasses.csproj
```

```cs --session "Dispatch to a handler" --region textbox --source-file ./Corvus.ContentHandling.DemoHandlerWithClasses/UIElements/TextBox.cs --project ./Corvus.ContentHandling.DemoHandlerWithClasses/Corvus.ContentHandling.DemoHandlerWithClasses.csproj
```

```cs --session "Dispatch to a handler" --source-file ./Corvus.ContentHandling.DemoHandlerWithClasses/Program.cs --project ./Corvus.ContentHandling.DemoHandlerWithClasses/Corvus.ContentHandling.DemoHandlerWithClasses.csproj
```
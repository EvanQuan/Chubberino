# Chubberino

## CPSC 501 Report

### Major Refactoring 1: Incorporate Dependency Injection

This major refactoring was split across three pull requests.
The first one, https://github.com/EvanQuan/Chubberino/pull/2, mainly alters all the Command classes, propagating out to the `CommandRepository`, and out to the `Bot` class.
The second pull request, https://github.com/EvanQuan/Chubberino/pull/6,   alters the `Bot` class.
The third pull request, https://github.com/EvanQuan/Chubberino/pull/7, alters the `Bot`, Repeater, ExtendedClient, `ExtendedClientFactory`, and `ISpinWait` classes.

The problem that these three pull requests attempted to resolve was more than just a code smell, it was a design problem. There was a lot of tightly coupled code, meaning there is a high dependence between concrete classes. This makes the code difficult to change and makes unit testing difficult and sometimes impossible. The main issue of tightly coupling I targeted was the difficulty and sometimes inability to unit test the code. In particular, I could not test the side effects of standard output, through `Console.WriteLine`, or to mock out time-related methods like `SpinWait.SpinUtil`, and `Thread.Sleep`. As these methods are called in essentially every class, it impeded my ability to test most of the codebase.

The refactoring that was applied was incorporating dependency injection into all the classes. The `IExtendedClient` dependency was already injected prior to this refactoring, but completing it for all problematic classes would set the codebase up for the next major refactoring.
The first step was recognizing which class dependencies needed to be injected. Classes that create side effects, require inputs from outside sources such as databases or web sockets or involve expensive operations are prime candidates. Static methods are also good candidates to inject as they are inherently tightly coupled to any method they are called within. As a console application, I found `Console.WriteLine` the major culprit, with time-related methods `SpinWait.SpinUntil` and `Thread.Sleep` as minor candidates.

The injection of `Console.WriteLine` can mostly be summarized by the commit https://github.com/EvanQuan/Chubberino/commit/ba228131302b06a50f93bfb8f83bfee3f26a7b91#diff-bc2233545313957b002b9c174228bdf7, changing every `Command` subclass, the `CommandRepository` class that holds the Commands all, and the `Bot` class that holds the `CommandRepository`. It's a fairly large commit but is necessarily large because all the included files needed to be changed together to keep the program compilable.  At it's simplest, all classes that used to call the static method `Console.WriteLine` now pass in a `TextWriter` through their constructors, and call the `TextWriter`'s instance method `WriteLine` instead. At the top where `Bot` is instantiated, in `Bot` line 17, `Console.Out` is finally injected in the constructor, where it propagates to the `CommandRepository` and all the Commands.

The injection of the SpinWait.SpinUntil is implemented using the facade design pattern. The static method was wrapped into a new class's instance method, as seen in https://github.com/EvanQuan/Chubberino/pull/6/commits/fe1f51dd2a40959d300aecc8c2057c859a669b64. The object can be injected into the constructors of the relevant classes that use it, in this case, `Bot`, as seen in https://github.com/EvanQuan/Chubberino/pull/6/commits/c41c1c470c4afc6301f2728f2b54d4d50974ce45.
The injection of Thread.Sleep was done by extending the `ISpinWait` interface to include a Sleep method in https://github.com/EvanQuan/Chubberino/pull/7/commits/16a1b774abb711bd06f3915be723c7829eb77bb0. As the `ISpinWait` instance is already injected in all the needed locations, the only change was to replace all static Thread.Sleep calls with the `ISpinWait.Sleep` call.

Other files that needed to be changed were existing unit tests that were affected by the constructor parameter changes. For example, `UsingCommandRepository`, `UsingDisableAll`, `UsingGreet` and many of the other test classes, now have a mocked `TextWriter` that is required to instantiate the classes under test as seen in https://github.com/EvanQuan/Chubberino/commit/ba228131302b06a50f93bfb8f83bfee3f26a7b91.

The changes were tested by creating unit tests that focused on the newly injected classes. All the Command classes have newly introduced unit tests that verify the `Console` output, such as https://github.com/EvanQuan/Chubberino/pull/2/commits/c2c5003206a959afeaa6584c214daf1dfea203e9, which was otherwise not possible without injecting the `Console`. Similarly, tests were added to `CommandRepository`, one example being https://github.com/EvanQuan/Chubberino/pull/2/commits/a6639603e736fcf7fd1940f518a4a7037cb5cfd6.

There are numerous benefits to dependency injections. With these static and side-effect producing methods now being replaced with injectable interfaces, these classes can now be mocked out and the related methods can now be tested, which is exactly what was done. By writing unit tests for console output, I will be saving a lot of time replacing manual testing. By mocking out time-related methods, I have reduced the test suite execution time drastically from over 30s to under 3s, while still being able to test for correctness.

The result of injecting interface instances enables me to set up for easier refactoring in the future, by allowing myself to swap out one concrete implementation with another class of the same interface.  Most importantly, as mentioned earlier, dependency injection prepares the codebase to use an Inversion of Control container, the next major refactor.

## Major Refactoring 2: Set up an Inversion of Control (IoC) Container

This major refactoring was encapsulated in the following pull request: https://github.com/EvanQuan/Chubberino/pull/4. Any class that didn't have a corresponding interface was altered to implement one, notably `Bot` and ComplimentGenerator. Classes that instantiated other classes directly through constructor calls were also altered, most notably `CommandRepository`, `Color` and `BotInfo`. Finally, the point of entry to the program, the Program class, was changed.

 As an application grows with its number of classes, the relationship between class dependencies becomes more complicated and tracking resources can become a pain. Unless a dependency tree is created, perhaps by navigating through the codebase manually, or through some automated tool, it can sometimes be difficult to tell if certain classes are unnecessarily being instantiated more than once, or if certain resources are not being disposed of properly.

There are two main refactorings to setting up the IoC container. The first was creating interfaces for major classes to be registered by the IoC container. The `IBot` https://github.com/EvanQuan/Chubberino/pull/4/commits/e57c2625aba5e36bc26b8c5cc954dffa5945ef9f and IComplimentGenerator https://github.com/EvanQuan/Chubberino/pull/4/commits/4ea2d4d34bd0fbbf922a12296d2f31768feda535 interfaces were created.
Secondly, direct constructor calls were replaced by adding instances through instance methods. For `CommandRepository`, all the direct Command constructor calls are replaced by an AddCommand method, in https://github.com/EvanQuan/Chubberino/pull/4/commits/79cfbd47fff8651b15c8c3f59b345342b99ea686#diff-bc2233545313957b002b9c174228bdf7. The same was done for `Color`, which replaced direct `ColorSelector` constructor calls with an `AddColorSelectorMethod`, in https://github.com/EvanQuan/Chubberino/pull/4/commits/61258329340ee1aebd2b1b1a51df14b2a57f0233. The `BotInfo` class, which used to instantiate two copies of IClientOptions internally, now have those instances passed in through the constructor, in https://github.com/EvanQuan/Chubberino/pull/4/files#diff-ad37ed546743393b2c8c4a00e80366e4.
Where classes are not instantiated at the start of the program, but an arbitrary number of times throughout runtime, new factory classes are created. The `ExtendedClientFactory` constructor call in Bot is replaced with an `ExtendedClientFactory` https://github.com/EvanQuan/Chubberino/pull/4/files#diff-c48fabc09fbaec790bcc1fae5f6d7ab7.
Finally, the IoC container is instantiated and configured at the composition root in Program.cs. The configuration was split across different commits as each class was worked on, but in the final result of the configuration can be seen here, https://github.com/EvanQuan/Chubberino/pull/4/files#diff-fcb7c4221764269eb1f126321816b02a.  Looking at line 24 for example, every time the IBot interface is referenced in the code, it is replaced by Bot class for the implementation, and one and only one instance of Bot is ever created. Line 31 replaces every reference of IRepeater with its own instance of Repeater. Further down at line 85 to 115, `CommandRepository` and `Color` now add their respective `ICommands` and `IColorSelectors` at the composition root instead of in their constructors. For `BotInfo`, lines 39 to 55 now setup the instantiation of those two `IClientOptions`, and by registering it as `SingleInstance`, we can be ensured that those options, along with `BotInfo`, only get instantiated once.

Most of the testing focused around the Bot class, where the new `ExtendedClientFactory` is used, and the `ExtendedFactory` plays a big role in the behaviour of the `Bot` and many of the `Commands`. Since the IoC container manages the instantiation of classes, particularly the Commands, Comand Repository, ExtendedClient and Bot, the behaviour of these classes should remain the same, as the logic of most methods should remain untouched. General unit tests for all these classes were added where unit test coverage did not exist. These tests provided confidence that the program still behaves as expected.

The code is better structured now with an IoC container and it now becomes far easier to manage objects. The IoC container can ensure that classes are reused where desired. It is also better to navigate the code to see code dependencies. At the composition root in Program, one can see how each class is used for which interfaces and how many times. For all other classes, one can see all the dependencies of a class by only looking at its constructor. These changes enable further refactoring by simply making future refactoring easier. Using dependency injection and an IoC container, all classes are now loosely coupled and have their dependencies automatically managed. For any future refactorings or new additions, it will be easier for me to reuse code, or propagate large changes without needing to change many classes.

## Minor Refactoring 3: Inline the BotInfo class into Bot

There is one pull request for the refactoring, namely https://github.com/EvanQuan/Chubberino/pull/5/files.

The Bot and `BotInfo` classes were the main altered classes. Other classes that have Bot as a dependency, including ExtendedClient, `ExtendedClientFactory`, and Mode were minorly altered. Program, which has the IoC to register Bot, was changed.

The bad code smell was that `BotInfo` was a lazy data class. As a data class, it only existed to have certain properties to set and get. It was also lazy in that it wasn't doing enough to warrant still existing (it was needed for 1 Command subclass only), and had to cost in maintaining it. Originally it was made because the Mode command needed to access certain properties about the Bot class, and so those properties were extracted out to a `BotInfo` singleton that Mode could communicate with. Now with the previous IoC container refactoring, the `BotInfo` singleton is unnecessary.

I used inline class, by merging `BotInfo` into Bot. The `BotInfo` class was deleted, and all the properties of `BotInfo` were added to Bot, in https://github.com/EvanQuan/Chubberino/pull/5/files#diff-d2ed8c60626ff81269f4fa4f7372fc7a. The propagating changes that were needed were removing all references of `BotInfo`, such as in `ExtendedClientFactory`, `Mode`, and existing tests. For those existing tests, where there was getting or setting values from `BotInfo`, some new test setup was added to the mocked Bot properties to replace that behaviour. The instantiation of the two ClientOptions is now delegated to the IoC container, through the Bot constructor in https://github.com/EvanQuan/Chubberino/pull/5/files#diff-fcb7c4221764269eb1f126321816b02a.

No new tests were needed to introduce. The Mode command, which was the sole reason for the `BotInfo` class, is already tested. All the public methods of Bot are already covered by tests, as well as any methods that involve reading from or writing to `BotInfo`.

The code is better structured now because it removed an unnecessary class that only made the code more convoluted.

The result of the refactoring might suggest there may be other lazy classes that are candidates to be inlined. If not, it acts as a reminder when adding future classes to make sure they provide enough value before committing to adding them.

## Minor Refactoring 4: Extract PyramidTracker class from TrackPyramids

The pull request for the refactoring is https://github.com/EvanQuan/Chubberino/pull/9.

The `TrackPyramids` class was altered and it's pre-existing test class WhenParsingForPyramids.

The bad smell was that `TrackPyramids.TwitchClient_OnMessageReceived` was a long method. It contained all the logic of the class, making it hard to understand.

Extract class was used to solve this. First, extract method was used for some of the repeated code or simple operations. Then, with a new `PyramidTracker` class, move method and move field were used for those methods and the mutated fields from TrackPyramid to `PyramidTracker`.

As a result, the `PyramidTracker` class was added to become responsible for the direct field manipulating operations.

The code was tested by adding tests to cover every `PyramidTracker` method. The pre-existing `WhenParsingForPyramids` test class was modified and polished in https://github.com/EvanQuan/Chubberino/pull/9/files#diff-28d546d6b918e5dd602c2bb367a5cbef, but for the most part, still stayed the same to ensure the TrackPyramids was still behaving as expected.

The code is better structured now because as much of the private behaviour is now open for direct testing through the `PyramidTracker` class. `PyramidTracker` can now be reused in other parts of the code if I add more features.

This refactoring suggests that similar class extraction can be done for other complicated and lengthy methods. It will improve testability, modularity, and code reuse.

## Minor Refactor 5: Extract Duplicated Code to Method

The pull request for the refactoring is in https://github.com/EvanQuan/Chubberino/pull/10.

There was duplicated code in the `Set`, `Add` and `Remove` methods. All three methods have the exact same logic with the only difference is they each call a different inner method, and have different error messages to the console.

To resolve the duplicated code, I used extract method by creating a higher-order method called `ApplyMetaCommand`, which implements this repeated logic, and has the inner command, and error messages as parameters, as shown in https://github.com/EvanQuan/Chubberino/pull/10/commits/1b3fdef31c78669a5206647b3ae9d6ab11f57d20.

The `CommandRepository` is already tested in the `WhenGettingSettings`, and `WhenSettingProperties` test classes, so rerunning those tests covered some of the refactorings. `WhenRemovingProperty` (https://github.com/EvanQuan/Chubberino/pull/10/commits/72bb8e2a44288ae8d6d41c51f5f9cab3dca2973a), and `WhenAddingProperty` (https://github.com/EvanQuan/Chubberino/pull/10/commits/564245bfe563744ca999318550af76f51cd71683) test classes were added to fill in the missing test coverage, and WhenGettingProperty (https://github.com/EvanQuan/Chubberino/pull/10/commits/25e0aa1958b54c0a00f92e2017d923982f57a981) as extra testing.

The code is better structured now by unifying all the repeated implementations into a single one. If more meta-commands are added in the future, they can now call `ApplyMetaCommand` to avoid needing to copy and paste the same implementation as before.

The result of this refactoring shows how higher-order methods can be used to reduce code duplication and suggest that there may be similar logic in other areas that can be extracted to higher-order methods.

<!-- End of the report. Below is original readme. -->

## Setup

### Clone repository

![Clone](/Images/Clone.png)

### Copy TwitchInfo.resx file

Execute `move_resx.sh`

![FileExplorer](/Images/move_resx.sh.png)

### Open `Chubberino.sln` solution file

![FileExplorer](/Images/FileExplorer.png)

### Go to the Solution Explorer to Open TwitchInfo.resx

![SolutionExplorer](/Images/SolutionExplorer.png)

### Add Fields

![AddFields](/Images/AddFields.png)

`BotToken`

[Get the OAuth access token for the bot account.](https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#oauth-client-credentials-flow)

[The easy way to get it.](https://twitchtokengenerator.com/)

- Choose Bot Chat Token

`BotUsername`

- The Twitch username that is using the bot.

`ClientId`

[Register your application on the Twitch developer site.](https://dev.twitch.tv/docs/authentication#registration)

`InitialChannelName`

- The Twitch username whose channel to initially join.

Microsoft.Playwright.PlaywrightException : Unexpected token "" while parsing selector ""
Call log:
  -   - waiting for #storybook-preview-iframe >> internal:control=enter-frame  >> html >> nth=0 >>  >> mir-tree-loading to be detached
   at Microsoft.Playwright.Transport.Connection.InnerSendMessageToServerAsync[T](ChannelOwner object, String method, Dictionary`2 dictionary, Boolean keepNulls) in /_/src/Playwright/Transport/Connection.cs:line 206
   at Microsoft.Playwright.Transport.Connection.WrapApiCallAsync[T](Func`1 action, Boolean isInternal) in /_/src/Playwright/Transport/Connection.cs:line 535
   at Mir.Playwright.Extensions.Core.Components.Tree.TreeComponent.WaitForReady() in C:\Users\bibanv\Repos\AutoTests\Mir.Playwright.Extensions.Core\Components\Tree\NewTree.cs:line 123
   at Mir.Playwright.Extensions.Core.Tests.Components.NewTree.CollapseNode() in C:\Users\bibanv\Repos\AutoTests\Tests\Mir.Playwright.Extensions.Core.Tests\Components\Tree.cs:line 130
   at NUnit.Framework.Internal.TaskAwaitAdapter.GenericAdapter`1.BlockUntilCompleted()
   at NUnit.Framework.Internal.MessagePumpStrategy.NoMessagePumpStrategy.WaitForCompletion(AwaitAdapter awaiter)
   at NUnit.Framework.Internal.AsyncToSyncAdapter.Await(Func`1 invoke)
   at NUnit.Framework.Internal.Commands.TestMethodCommand.RunTestMethod(TestExecutionContext context)
   at NUnit.Framework.Internal.Commands.TestMethodCommand.Execute(TestExecutionContext context)
   at NUnit.Framework.Internal.Commands.BeforeAndAfterTestCommand.<>c__DisplayClass1_0.<Execute>b__0()
   at NUnit.Framework.Internal.Commands.DelegatingTestCommand.RunTestMethodInThreadAbortSafeZone(TestExecutionContext context, Action action)

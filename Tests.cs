[TestFixture(Description = "Тесты для старого дерева")]
[Parallelizable]
public class LegacyTree : BaseTreeTest
{
  public LegacyTree() : base("tree-basic--basic")
  {
  }

  [Test(Description = "Проверка на сворачивание узла")]
  public override async Task CollapseNode()
  {
    var tree = Locator.FindMirComponent(MirSelector.LegacyTree).AsLegacyTree();
    await tree.WaitForReady();
    var label = "Рутовый объект 2";
    await tree.ExpandByPath(label);
    var model = await tree.GetNodeByPath(label);
    Assert.That(model.IsExpanded, Is.True);
    await tree.CollapseNode(label);
    model = await tree.GetNodeByPath(label);
    Assert.That(model.IsExpanded, Is.False);
  }

  [Test(Description = "Проверка на разворачивание узла по пути")]
  public override async Task ExpandByPath()
  {
    var tree = Locator.FindMirComponent(MirSelector.LegacyTree).AsLegacyTree();
    await tree.WaitForReady();
    await tree.ExpandByPath("Рутовый объект 2", "Рутовый объект 2_дочерний_2", "Рутовый объект 2_дочерний_2_дочерний_2");
    var node = await tree.GetNodeByPath("Рутовый объект 2_дочерний_2_дочерний_2");
    Assert.IsNotNull(node);
  }

  [Test(Description = "Проверка на разворачивание узла")]
  public override async Task ExpandNode()
  {
    var tree = Locator.FindMirComponent(MirSelector.LegacyTree).AsLegacyTree();
    await tree.WaitForReady();
    await tree.ExpandNode("Рутовый объект 2");
    var model = await tree.GetNodeByPath("Рутовый объект 2");
    Assert.IsNotNull(model);
    Assert.That(model.IsExpanded, Is.True);
  }

  [Test(Description = "Проверка на получение моделт узла по пути")]
  public override async Task GetNodeModelByPath()
  {
    var tree = Locator.FindMirComponent(MirSelector.LegacyTree).AsLegacyTree();
    await tree.WaitForReady();
    var node = await tree.GetNodeByPath("Рутовый объект 2");
    Assert.IsNotNull(node);
    Assert.That(node.Label, Is.EqualTo("Рутовый объект 2"));
    Assert.That(node.IsExpanded, Is.False);
    Assert.That(node.IsSelected, Is.False);
    Assert.That(node.IsExpandable, Is.True);
    Assert.That(node.Level, Is.EqualTo(0));
  }

  [Test(Description = "Проверка на открытие контекстного меню")]
  public override async Task OpenContextMenu()
  {
    var tree = Locator.FindMirComponent(MirSelector.LegacyTree).AsLegacyTree();
    await tree.WaitForReady();
    var menu = await tree.OpenContextMenuAsync("Рутовый объект 2");
    Assert.That(await menu.IsVisibleAnyAsync(), Is.True);
    var items = await menu.GetItemsAsync();
    Assert.That(items.Count, Is.EqualTo(3));
    await menu.ClickItemAsync("Удалить");
    Assert.That(await menu.IsVisibleAnyAsync(), Is.False);
  }

  [Test(Description = "Проверка на прокрутку до узла")]
  public override async Task ScrollToNode()
  {
    var tree = Locator.FindMirComponent(MirSelector.LegacyTree).AsLegacyTree();
    await tree.WaitForReady();
  }

  [Test(Description = "Проверка на выбор узла")]
  public override async Task SelectNode()
  {
    var tree = Locator.FindMirComponent(MirSelector.LegacyTree).AsLegacyTree();
    await tree.WaitForReady();
    await tree.SelectNode("Рутовый объект 2");
  }

  [Test(Description = "Проверка на готовность дерева")]
  public override async Task WaitForReady()
  {
    await Locator.FindMirComponent(MirSelector.LegacyTree).AsLegacyTree().WaitForReady();
    await ToHaveCountNodesAsync(9);
  }
}

[TestFixture(Description = "Тесты для нового дерева")]
[Parallelizable]
public class NewTree : BaseTreeTest
{
  public NewTree() : base("tree-basic--basic")
  {
  }

  [Test(Description = "Проверка на сворачивание узла")]
  public override async Task CollapseNode()
  {
    var tree = Locator.FindMirComponent(MirSelector.Tree).AsTree();
    await tree.WaitForReady();
    var label = "Рутовый объект 2";
    await tree.ExpandByPath(label);
    var model = await tree.GetNodeByPath(label);
    Assert.That(model.IsExpanded, Is.True);
    await tree.CollapseNode(label);
    model = await tree.GetNodeByPath(label);
    Assert.That(model.IsExpanded, Is.False);
  }

  [Test(Description = "Проверка на разворачивание узла по пути")]
  public override async Task ExpandByPath()
  {
    var tree = Locator.FindMirComponent(MirSelector.Tree).AsTree();
    await tree.WaitForReady();
    await tree.ExpandByPath("Рутовый объект 2", "Рутовый объект 2_дочерний_2", "Рутовый объект 2_дочерний_2_дочерний_2");
    var node = await tree.GetNodeByPath("Рутовый объект 2_дочерний_2_дочерний_2");
    Assert.IsNotNull(node);
  }

  [Test(Description = "Проверка на разворачивание узла")]
  public override async Task ExpandNode()
  {
    var tree = Locator.FindMirComponent(MirSelector.Tree).AsTree();
    await tree.WaitForReady();
    await tree.ExpandNode("Рутовый объект 2");
    var model = await tree.GetNodeByPath("Рутовый объект 2");
    Assert.IsNotNull(model);
    Assert.That(model.IsExpanded, Is.True);
  }

  [Test(Description = "Проверка на получение моделт узла по пути")]
  public override async Task GetNodeModelByPath()
  {
    var tree = Locator.FindMirComponent(MirSelector.Tree).AsTree();
    await tree.WaitForReady();
    var node = await tree.GetNodeByPath("Рутовый объект 2");
    Assert.IsNotNull(node);
    Assert.That(node.Label, Is.EqualTo("Рутовый объект 2"));
    Assert.That(node.IsExpanded, Is.False);
    Assert.That(node.IsSelected, Is.False);
    Assert.That(node.IsExpandable, Is.True);
    Assert.That(node.Level, Is.EqualTo(0));
  }

  [Test(Description = "Проверка на открытие контекстного меню")]
  public override async Task OpenContextMenu()
  {
    var tree = Locator.FindMirComponent(MirSelector.Tree).AsTree();
    await tree.WaitForReady();
    var menu = await tree.OpenContextMenuAsync("Рутовый объект 2");
    Assert.That(await menu.IsVisibleAnyAsync(), Is.True);
    var items = await menu.GetItemsAsync();
    Assert.That(items.Count, Is.EqualTo(3));
    await menu.ClickItemAsync("Удалить");
    Assert.That(await menu.IsVisibleAnyAsync(), Is.False);
  }

  [Test(Description = "Проверка на прокрутку до узла")]
  public override async Task ScrollToNode()
  {
    var tree = Locator.FindMirComponent(MirSelector.Tree).AsTree();
    await tree.WaitForReady();
  }

  [Test(Description = "Проверка на выбор узла")]
  public override async Task SelectNode()
  {
    var tree = Locator.FindMirComponent(MirSelector.Tree).AsTree();
    await tree.WaitForReady();
    await tree.SelectNode("Рутовый объект 2");
  }

  [Test(Description = "Проверка на готовность дерева")]
  public override async Task WaitForReady()
  {
    await Locator.FindMirComponent(MirSelector.Tree).AsTree().WaitForReady();
    await ToHaveCountNodesAsync(9);
  }
}
using System;
using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Mir.Playwright.Extensions.Core.Components.Overlay;
using Mir.Playwright.Extensions.Core.Extensions;

namespace Mir.Playwright.Extensions.Core.Components.Tree;

/// <summary>
/// Старое дерево, которое используется почти везде
/// </summary>
/// <param name="locator"></param>
public class LegacyTreeComponent(ILocator locator) : BaseTree(locator)
{
  /// <inheritdoc/>
  public override async Task CollapseNode(string label)
  {
    var nodeLocator = Locator.Locator($"{_nodeSelector}", GetMatchTextLocatorOptions(label));
    var button = nodeLocator.Locator(".mir-tree-node-expand-button");
    await button.ClickAsync();
    await WaitAsync(100);
  }

  /// <inheritdoc/>
  public override async Task ExpandByPath(params string[] labelsPath)
  {
    for (var i = 0; i < labelsPath.Length; i++)
    {
      var label = labelsPath[i];
      await ExpandNode(label);
    }
  }

  /// <inheritdoc/>
  public override async Task ExpandNode(string label)
  {
    var options = GetMatchTextLocatorOptions(label);
    var nodeLocator = Locator.Locator($"{_nodeSelector}", options);
    var expandLocator = nodeLocator.Locator(".mir-tree-node-expand-button");
    await expandLocator.ClickAsync();

    await Locator.Locator($"{_nodeSelector}.mir-tree-node-expanded", options)
      .WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
    await WaitAsync(500);
  }

  /// <inheritdoc/>
  public override async Task<MirTreeNode> GetNodeByPath(params string[] labelsPath)
  {
    var nodeLocator = await GetNodeLocatorAsync(labelsPath);
    var id = await nodeLocator.GetAttributeAsync("id") ?? "";
    var icon = await nodeLocator.Locator(".mir-tree-node-content>span").TryGetAttributeAsync("class") ?? "";
    var classes = (await nodeLocator.GetAttributeAsync("class") ?? "").Split(' ');
    var isDisabled = (await nodeLocator.GetAttributeAsync("aria-disabled")) == "true";
    var parentId = await nodeLocator.GetAttributeAsync("parent-id") ?? "";
    var level = await nodeLocator.GetAttributeAsync("aria-level") ?? "";
    return new MirTreeNode(
      id,
      labelsPath.Last(),
      icon,
      classes.Contains("mir-tree-node-expanded"),
      classes.Contains("mir-tree-node-expandable"),
      classes.Contains("'mir-tree-node-selected"),
      isDisabled,
      parentId,
      int.TryParse(level, out var result) ? result : -1
    );
  }

  /// <inheritdoc/>
  public override async Task SelectNode(params string[] labelsPath)
  {
    var node = await GetNodeByPath(labelsPath);
    if (node == null || node.IsSelected)
    {
      return;
    }
    var l = FindLocatorNodeByModel(node);
    await l.ClickAsync();
  }

  /// <inheritdoc/>
  public override async Task WaitForReady()
  {
    await Locator.Locator(".spinner").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Detached });
  }
}
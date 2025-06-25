using System;
using Microsoft.Playwright;

namespace Mir.Playwright.Extensions.Core.Components.Tree;

/// <summary>
/// Новое дерево, которое используется почти нигде
/// </summary>
/// <param name="locator"></param>
public class TreeComponent(ILocator locator) : BaseTree(locator)
{
    private const string NodeSelector = ".cdk-tree-node";
    private const string ToggleButtonSelector = ".mir-tree-node-toggle-button";
    private const string LoadingSpinnerSelector = ".mir-tree-node-toggle-spinner-button";

    /// <inheritdoc/>
    public override Task CollapseNode(string label)
    {
        var nodeLocator = await GetNodeLocatorByLabel(label);
        var toggleButton = nodeLocator.Locator($"{ToggleButtonSelector} mir-icon-button");
        
        var isExpanded = await IsNodeExpanded(nodeLocator); // почему добавили проверку, хотя раньше её не было
        if (!isExpanded)
        {
            return;
        }

        await toggleButton.ClickAsync();
        await WaitAsync(100);
    }

    /// <inheritdoc/>
    public override Task ExpandByPath(params string[] labelsPath)
    {
        for (var i = 0; i < labelsPath.Length; i++)
        {
            var label = labelsPath[i];
            await ExpandNode(label);
        }
    }
    /// <inheritdoc/>
    public override Task ExpandNode(string label)
    {
        var nodeLocator = await GetNodeLocatorByLabel(label); // в чём конкретно разница между новым и старым кодом и чем это обусловлено
        var toggleButton = nodeLocator.Locator($"{ToggleButtonSelector} mir-icon-button");
        
        var hasToggleButton = await toggleButton.CountAsync() > 0;
        if (!hasToggleButton)
        {
            return;
        }

        var isExpanded = await IsNodeExpanded(nodeLocator);
        if (isExpanded)
        {
            return;
        }

        await toggleButton.ClickAsync();

        await WaitForNodeExpanded(nodeLocator);
        await WaitAsync(500);
    }
    /// <inheritdoc/>
    public override Task<MirTreeNode> GetNodeByPath(params string[] labelsPath)
    {
        var nodeLocator = await GetNodeLocatorAsync(labelsPath); // почему были добавлены новые переменные и что значит в новом дереве может потребоваться дополнительная логика? Какая? Как можно реализовать?
        var id = await nodeLocator.GetAttributeAsync("id") ?? "";
        var classes = (await nodeLocator.GetAttributeAsync("class") ?? "").Split(' ');
        var isDisabled = classes.Contains("mir-tree-node-disabled");
        var isSelected = classes.Contains("mir-tree-node-selected");
        
        var iconElement = nodeLocator.Locator(".mir-tree-node-icon");
        var icon = await iconElement.GetAttributeAsync("class") ?? "";
        
        var level = await GetNodeLevel(nodeLocator);
        
        var hasToggleButton = await nodeLocator.Locator($"{ToggleButtonSelector} mir-icon-button").CountAsync() > 0;
        var isExpandable = hasToggleButton;
        
        var isExpanded = isExpandable && await IsNodeExpanded(nodeLocator);
        
        return new MirTreeNode(
            id,
            labelsPath.Last(),
            icon,
            isExpanded,
            isExpandable,
            isSelected,
            isDisabled,
            "", // parentId - в новом дереве может потребоваться дополнительная логика
            level
        );
    }
    /// <inheritdoc/>
    public override Task SelectNode(params string[] labelsPath)
    {
        var nodeLocator = await GetNodeLocatorAsync(labelsPath); // в чём разница между старым и новым
        
        var classes = (await nodeLocator.GetAttributeAsync("class") ?? "").Split(' ');
        var isSelected = classes.Contains("mir-tree-node-selected");
        
        if (isSelected) // почему теперь есть проверка
        {
            return;
        }

        await nodeLocator.ClickAsync();
    }
    /// <inheritdoc/>
    public override Task WaitForReady()
    {
        await Locator.Locator("mir-tree-loading").WaitForAsync(new LocatorWaitForOptions // почему добавлен timeout?
        { 
            State = WaitForSelectorState.Detached,
            Timeout = 10000
        });
    }
}
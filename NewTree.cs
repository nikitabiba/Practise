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
    public override async Task CollapseNode(string label)
    {
        var options = GetMatchTextLocatorOptions(label);
        var nodeLocator = Locator.Locator($"{NodeSelector}", options);

        var toggleButton = nodeLocator.Locator($"{ToggleButtonSelector} mir-icon-button");

        var isExpanded = await IsNodeExpanded(nodeLocator);
        if (!isExpanded)
        {
            return;
        }

        await toggleButton.ClickAsync();
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
        var nodeLocator = Locator.Locator($"{NodeSelector}", options);

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
    public override async Task<MirTreeNode> GetNodeByPath(params string[] labelsPath)
    {
        var lastLabel = labelsPath.Last();
        var options = GetMatchTextLocatorOptions(lastLabel);
        var nodeLocator = Locator.Locator($"{NodeSelector}", options);
        var id = await nodeLocator.GetAttributeAsync("id") ?? "";
        var classes = (await nodeLocator.GetAttributeAsync("class") ?? "").Split(' ');
        var isDisabled = classes.Contains("mir-tree-node-disabled");
        var isSelected = classes.Contains("mir-tree-node-selected");

        var iconElement = nodeLocator.Locator(".mir-tree-node-icon");
        var icon = await iconElement.GetAttributeAsync("class") ?? "";

        var parentId = await GetParentId(nodeLocator);

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
            parentId,
            level
        );
    }
    /// <inheritdoc/>
    public override async Task SelectNode(params string[] labelsPath)
    {
        var lastLabel = labelsPath.Last();
        var options = GetMatchTextLocatorOptions(lastLabel);
        var nodeLocator = Locator.Locator($"{NodeSelector}", options);

        var classes = (await nodeLocator.GetAttributeAsync("class") ?? "").Split(' ');
        var isSelected = classes.Contains("mir-tree-node-selected");

        if (isSelected)
        {
            return;
        }

        await nodeLocator.ClickAsync();
    }
    /// <inheritdoc/>
    public override async Task WaitForReady()
    {
        await Locator.Locator("mir-tree-loading").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Detached });
    }

    /// <summary>
    /// Проверяет, развернут ли узел
    /// </summary>
    private async Task<bool> IsNodeExpanded(ILocator nodeLocator)
    {
        var toggleIcon = nodeLocator.Locator($"{ToggleButtonSelector} mir-icon-button");
        var iconClasses = await toggleIcon.GetAttributeAsync("class") ?? "";

        var hasChevronDown = iconClasses.Contains("chevronDown") || iconClasses.Contains("fa-chevron-down");

        return hasChevronDown;
    }

    /// <summary>
    /// Ждет, пока узел развернется
    /// </summary>
    private async Task WaitForNodeExpanded(ILocator nodeLocator)
    {
        var toggleIcon = nodeLocator.Locator($"{ToggleButtonSelector} mir-icon-button");

        await Locator.Page.WaitForFunctionAsync(@"
            (toggleButton) => {
                const classes = toggleButton.getAttribute('class') || '';
                return classes.includes('chevronDown') || classes.includes('fa-chevron-down');
            }
        ", await toggleIcon.ElementHandleAsync());
    }

    /// <summary>
    /// Получает уровень вложенности узла
    /// </summary>
    private async Task<int> GetNodeLevel(ILocator nodeLocator)
    {
        var style = await nodeLocator.GetAttributeAsync("style") ?? "";
        var paddingMatch = RegexHelper.PaddingLeftPattern().Match(style);
        if (paddingMatch.Success && int.TryParse(paddingMatch.Groups[1].Value, out var padding))
        {
            var indentValue = 24;
            return padding / indentValue;
        }

        return 0;
    }

    /// <summary>
    /// Получает id родительского узла
    /// </summary>
    private async Task<string> GetParentId(ILocator nodeLocator)
    {
        var parentNode = nodeLocator.Locator("xpath=ancestor::cdk-tree-node[1]");
        return await parentNode.GetAttributeAsync("id") ?? "";
    }
}

internal static partial class RegexHelper
{
    [System.Text.RegularExpressions.GeneratedRegex(@"padding-left:\s*(\d+)px")]
    public static partial System.Text.RegularExpressions.Regex PaddingLeftPattern();
}

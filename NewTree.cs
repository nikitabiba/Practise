using System;
using Microsoft.Playwright;

namespace Mir.Playwright.Extensions.Core.Components.Tree;

/// <summary>
/// Новое дерево, которое используется почти нигде
/// </summary>
/// <param name="locator"></param>
public class TreeComponent(ILocator locator) : BaseTree(locator)
{
    /// <inheritdoc/>
    public override Task CollapseNode(string label)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Task ExpandByPath(params string[] labelsPath)
    {
        throw new NotImplementedException();
    }
    /// <inheritdoc/>
    public override Task ExpandNode(string label)
    {
        throw new NotImplementedException();
    }
    /// <inheritdoc/>
    public override Task<MirTreeNode> GetNodeByPath(params string[] labelsPath)
    {
        throw new NotImplementedException();
    }
    /// <inheritdoc/>
    public override Task SelectNode(params string[] labelsPath)
    {
        throw new NotImplementedException();
    }
    /// <inheritdoc/>
    public override Task WaitForReady()
    {
        throw new NotImplementedException();
    }
}
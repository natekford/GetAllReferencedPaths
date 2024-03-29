using Avalonia.Controls;
using Avalonia.Controls.Templates;

using GetAllReferencedPaths.UI.ViewModels;

using System;

namespace GetAllReferencedPaths.UI;

public sealed class ViewLocator : IDataTemplate
{
	public IControl Build(object? data)
	{
		var name = data!.GetType().FullName!.Replace("ViewModel", "View");
		var type = Type.GetType(name);

		if (type != null)
		{
			return (Control)Activator.CreateInstance(type)!;
		}
		else
		{
			return new TextBlock { Text = "Not Found: " + name };
		}
	}

	public bool Match(object? data)
		=> data is ViewModelBase;
}
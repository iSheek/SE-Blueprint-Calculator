using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SECalc;

public partial class MainWindow : Window
{
    private string? _selectedFolderPath;
    private string? _selectedBlueprintPath;

    public MainWindow()
    {
        InitializeComponent();
    }

    public async void OnSelectFolderClicked(object sender, RoutedEventArgs e)
    {
        var storage = GetTopLevel(this)?.StorageProvider;
        if (storage == null) return;

        var folders = await storage.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select CubeBlocks folder",
            AllowMultiple = false
        });

        if (folders.Any())
        {
            _selectedFolderPath = folders[0].Path.LocalPath;
            FolderPathText.Text = _selectedFolderPath;
        }
    }

    public async void OnSelectBlueprintClicked(object sender, RoutedEventArgs e)
    {
        var storage = GetTopLevel(this)?.StorageProvider;
        if (storage == null) return;

        var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Blueprint file (.sbc)",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("Space Engineers SBC") { Patterns = new[] { "*.sbc" } } }
        });

        if (files.Any())
        {
            _selectedBlueprintPath = files[0].Path.LocalPath;
            BlueprintPathText.Text = _selectedBlueprintPath;
        }
    }

    // Główna logika obliczeń
    public void OnCalculateClicked(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_selectedFolderPath) || string.IsNullOrEmpty(_selectedBlueprintPath))
        {
            ResultTextBlock.Text = "ERROR: YOU HAVE TO SELECT BOTH THINGS!";
            return;
        }

        try
        {
            var sourceLoader = new SourceBlocksLoader();
            sourceLoader.Load(_selectedFolderPath);

            var bpLoader = new BlueprintLoader();
            bpLoader.Load(_selectedBlueprintPath);

            var calculator = new Calculator();
            var results = calculator.Calculate(bpLoader.gridBlocks, sourceLoader.blocks);

            if (results.Count == 0)
            {
                ResultTextBlock.Text = "No components found! Check if both blueprint files and CubeBlocks folder are correct";
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("Total sum of components:");
            sb.AppendLine("---------------------------");

            foreach (var item in results.OrderByDescending(x => x.Value))
            {
                sb.AppendLine($"{item.Key}: {item.Value:N0}");
            }

            ResultTextBlock.Text = sb.ToString();
        }
        catch (Exception ex)
        {
            ResultTextBlock.Text = $"ERROR:\n{ex.Message}";
        }
    }
}
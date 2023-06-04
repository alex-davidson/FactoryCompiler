using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FactoryCompiler.Model.Diagnostics;
using Microsoft.Msagl.Drawing;

namespace FactoryCompiler.Jobs.Visualise;

public class VisualiseFactoryModel : INotifyPropertyChanged, INotifyPropertyChanging
{
    public VisualiseFactoryModel(VisualiseFactoryModelInputs inputs)
    {
        RefreshCommand = new RefreshVisualiseFactoryModelCommand(inputs, this);
    }

    public RefreshVisualiseFactoryModelCommand RefreshCommand { get; }

    private SourceDataModel? sourceData;
    /// <summary>
    /// Set this when game data and all factory parts have been loaded.
    /// </summary>
    public SourceDataModel? SourceData
    {
        get => sourceData;
        set
        {
            if (sourceData == value) return;
            OnPropertyChanging();
            sourceData = value;
            SelectedObject = SelectedObjectModel.None;
            UpdateIssues();
            OnPropertyChanged();
        }
    }

    private FactoryModel? factory;
    /// <summary>
    /// Set this when the factory state has been computed and summarised.
    /// </summary>
    public FactoryModel? Factory
    {
        get => factory;
        set
        {
            if (factory == value) return;
            OnPropertyChanging();
            factory = value;
            SelectedObject = SelectedObjectModel.None;
            UpdateIssues();
            OnPropertyChanged();
        }
    }

    private Graph? graph;
    /// <summary>
    /// Set this when the factory graph has been generated.
    /// </summary>
    public Graph? Graph
    {
        get => graph;
        set
        {
            if (graph == value) return;
            OnPropertyChanging();
            graph = value;
            SelectedObject = SelectedObjectModel.None;
            OnPropertyChanged();
        }
    }

    public void SetSelectedObject(DrawingObject? obj)
    {
        SelectedObject = new SelectedObjectModelFactory(Factory, Graph).Create(obj);
    }

    private SelectedObjectModel selectedObject = SelectedObjectModel.None;
    /// <summary>
    /// Currently-selected graph Node.
    /// </summary>
    public SelectedObjectModel SelectedObject
    {
        get => selectedObject;
        private set
        {
            if (selectedObject == value) return;
            OnPropertyChanging();
            selectedObject = value;
            OnPropertyChanged();
        }
    }

    private void UpdateIssues()
    {
        Issues = new IssuesModel(SourceData.GetOrderedDiagnostics()
            .Concat(Factory?.State.Diagnostics ?? Enumerable.Empty<Diagnostic>())
            .ToArray());
    }

    private IssuesModel issues = new IssuesModel(Array.Empty<Diagnostic>());

    /// <summary>
    /// Diagnostic messages.
    /// </summary>
    public IssuesModel Issues
    {
        get => issues;
        private set
        {
            if (issues == value) return;
            OnPropertyChanging();
            issues = value;
            OnPropertyChanged();
        }
    }

    private bool isRefreshing;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set
        {
            if (isRefreshing == value) return;
            OnPropertyChanging();
            isRefreshing = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangingEventHandler? PropertyChanging;
    private void OnPropertyChanging([CallerMemberName] string memberName = null!)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(memberName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string memberName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
    }
}

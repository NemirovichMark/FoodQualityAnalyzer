using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using Model.Core;
using Model.Data;
using Convert = Model.Data.Convert;

namespace FoodQualityAnalyzer;

public partial class ChartWindow : Window
{
    private readonly MainWindow _mainWindow;
    private FoodProduct[] _products;

    public ChartWindow(FoodProduct[] products, MainWindow mainWindow)
    {
        InitializeComponent();
        _products = products;
        _mainWindow = mainWindow;

        InitializeChart(products);
        SubscribeToCheckboxes();

        ReportFormatComboBox.ItemsSource = new List<string>
        {
            "JSON",
            "XML"
        };
        ReportFormatComboBox.SelectedIndex = 0;

        DataContext = this;
        Closing += Window_Closing;
        AddProductWindow.Update(this);
    }

    public SeriesCollection ColumnSeries { get; set; }
    public List<string> Labels { get; set; }
    public Func<double, string> Formatter { get; set; } //встроенный делегат преобразует число в строку

    private void InitializeChart(FoodProduct[] products)
    {
        Labels = products.Select(p => p.Name).ToList();
        var values = products.Select(p => p.GetQuality()).ToList();
        //диаграмма
        var colors = new[]
        {
            Brushes.AntiqueWhite,
            Brushes.Aquamarine,
            Brushes.CornflowerBlue,
            Brushes.LightPink,
            Brushes.Lavender,
            Brushes.PaleVioletRed,
            Brushes.LightSkyBlue,
            Brushes.LemonChiffon,
            Brushes.PeachPuff
        };

        ColumnSeries = new SeriesCollection();
        for (var i = 0; i < values.Count; i++)
            ColumnSeries.Add(new ColumnSeries
            {
                Title = Labels[i],
                Values = new ChartValues<double> { values[i] },
                Fill = colors[i % colors.Length], // Циклическое использование цветов
                DataLabels = false
            });

        // Форматтер для значений
        Formatter = value => value.ToString("N0") + "%"; // лямбда выр
    }

    private void ShowColumnChart(object sender, RoutedEventArgs e)
    {
        ColumnChart.Visibility = Visibility.Visible;
        Title = "Столбчатая диаграмма качества";
    }

    public void SubscribeToCheckboxes()
    {
        if (_mainWindow != null)
            foreach (var product in Model.Core.FoodQualityAnalyzer.Products)
            {
                var checkBox = _mainWindow.FindCheckBox(product.Name);
                if (checkBox != null)
                {
                    checkBox.Checked += (s, e) => UpdateChart();
                    checkBox.Unchecked += (s, e) => UpdateChart();
                }
            }
    }

    private void UpdateChart()
    {
        var selectedProducts = new List<FoodProduct>();

        if (_mainWindow != null)
            foreach (var product in Model.Core.FoodQualityAnalyzer.Products)
            {
                var checkBox = _mainWindow.FindCheckBox(product.Name);
                if (checkBox != null && checkBox.IsChecked == true) selectedProducts.Add(product);
            }

        var values = selectedProducts.Select(p => p.GetQuality()).ToList();
        Labels = selectedProducts.Select(p => p.Name).ToList();
        _products = selectedProducts.ToArray();

        var colors = new[]
        {
            Brushes.AntiqueWhite,
            Brushes.Aquamarine,
            Brushes.CornflowerBlue,
            Brushes.LightPink,
            Brushes.Lavender,
            Brushes.PaleVioletRed,
            Brushes.LightSkyBlue,
            Brushes.LemonChiffon,
            Brushes.PeachPuff
        };

        ColumnSeries.Clear();

        // Добавляем обновлённые данные
        for (var i = 0; i < values.Count; i++)
            ColumnSeries.Add(new ColumnSeries
            {
                Title = Labels[i],
                Values = new ChartValues<double> { values[i] },
                Fill = colors[i % colors.Length], // Циклическое использование цветов
                DataLabels = false
            });

        // Явно обновляем оси
        ColumnChart.AxisY[0].MinValue = 0;
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        var result = MessageBox.Show("Сохранить изменения перед выходом?",
            "Подтверждение",
            MessageBoxButton.YesNoCancel);

        if (result == MessageBoxResult.Yes)
        {
            if (_products.Length != 0)
            {
                switch (ReportFormatComboBox.SelectedItem)
                {
                    case "JSON":
                        Convert.ConvertFromXMLToJson(
                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReportsXML"));
                        var SerJ = new SerializeJson();
                        SerJ.GenerateReport(_products);
                        break;

                    case "XML":
                        Convert.ConvertFromJsonToXML(
                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReportsJson"));
                        var SerX = new SerializeXML();
                        SerX.GenerateReport(_products);
                        break;
                }
            }
            else
            {
                var res = MessageBox.Show("Выберите хотя бы один продукт",
                    "Ошибка",
                    MessageBoxButton.OK);
                e.Cancel = true; // Отменяем закрытие
            }

            AddProductWindow.Update();
        }
        else if (result == MessageBoxResult.Cancel)
        {
            e.Cancel = true; // Отменяем закрытие
        }
    }
}
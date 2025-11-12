using System.Windows;
using System.Windows.Controls;
using Model.Core;
using Model.Data;

namespace FoodQualityAnalyzer;

public partial class AddProductWindow : Window
{
    private static ChartWindow _window;

    public AddProductWindow()
    {
        InitializeComponent();

        // Заполняем ComboBox типами продуктов
        TypeComboBox.ItemsSource = new List<string>
        {
            "Овощ",
            "Фрукт",
            "Мясо",
            "Выпечка"
        };
        TypeComboBox.SelectedIndex = 0;
    }

    public FoodProduct NewProduct { get; private set; }


    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            MessageBox.Show("Введите название продукта", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(MaxTextBox.Text, out var maxExpiryDays) || maxExpiryDays <= 0)
        {
            MessageBox.Show("Введите корректный Максимальный срок годности (дней)", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(OstTextBox.Text, out var ostExpiryDays) || ostExpiryDays <= 0)
        {
            MessageBox.Show("Введите корректный Оставшийся срок годности (дней)", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (int.Parse(OstTextBox.Text) > int.Parse(MaxTextBox.Text))
        {
            MessageBox.Show("Оставшийся срок годности не может превышать Максимальный срок годности", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!Case1.IsChecked.Value && !Case2.IsChecked.Value && (bool)!Case3.IsChecked && (bool)!Case4.IsChecked &&
            (bool)!Case5.IsChecked && (bool)!Case6.IsChecked && (bool)!Case7.IsChecked && (bool)!Case8.IsChecked)
        {
            var result = MessageBox.Show(
                "Проверьте корректность выбранных характеристик продукта. Хотите продолжить без выбора категории?",
                "Предупреждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No) return; // Прерываем сохранение, если пользователь выбрал "Нет"
        }

        switch (TypeComboBox.SelectedItem)
        {
            case "Овощ":
                NewProduct = new Vegetable(NameTextBox.Text, int.Parse(OstTextBox.Text), int.Parse(MaxTextBox.Text),
                    Case1.IsChecked == true, Case2.IsChecked == true);
                break;
            case "Фрукт":
                NewProduct = new Fruit(NameTextBox.Text, int.Parse(OstTextBox.Text), int.Parse(MaxTextBox.Text),
                    Case3.IsChecked == true, Case4.IsChecked == true);
                break;
            case "Мясо":
                NewProduct = new Meat(NameTextBox.Text, int.Parse(OstTextBox.Text), int.Parse(MaxTextBox.Text),
                    Case5.IsChecked == true, Case6.IsChecked == true);
                break;
            case "Выпечка":
                NewProduct = new Backery(NameTextBox.Text, int.Parse(OstTextBox.Text), int.Parse(MaxTextBox.Text),
                    Case7.IsChecked == true, Case8.IsChecked == true);
                break;
        }

        var mainWindow = (MainWindow)Application.Current.MainWindow;
        mainWindow.AddProduct(NewProduct); //добавляем чек бокс

        var analyzer = new Model.Core.FoodQualityAnalyzer();
        analyzer.Add(NewProduct);
        ProductAdd_Json.SaveProduct(NewProduct);
        if (_window != null) _window.SubscribeToCheckboxes(); //если открыта диаграмма обновляем подписку на чекбокс
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TypeComboBox.SelectedItem == "Овощ")
        {
            Case1.Visibility = Visibility.Visible;
            Case2.Visibility = Visibility.Visible;
            Case3.Visibility = Visibility.Collapsed;
            Case4.Visibility = Visibility.Collapsed;
            Case5.Visibility = Visibility.Collapsed;
            Case6.Visibility = Visibility.Collapsed;
            Case7.Visibility = Visibility.Collapsed;
            Case8.Visibility = Visibility.Collapsed;
        }

        if (TypeComboBox.SelectedItem == "Фрукт")
        {
            Case1.Visibility = Visibility.Collapsed;
            Case2.Visibility = Visibility.Collapsed;
            Case3.Visibility = Visibility.Visible;
            Case4.Visibility = Visibility.Visible;
            Case5.Visibility = Visibility.Collapsed;
            Case6.Visibility = Visibility.Collapsed;
            Case7.Visibility = Visibility.Collapsed;
            Case8.Visibility = Visibility.Collapsed;
        }

        if (TypeComboBox.SelectedItem == "Мясо")
        {
            Case1.Visibility = Visibility.Collapsed;
            Case2.Visibility = Visibility.Collapsed;
            Case3.Visibility = Visibility.Collapsed;
            Case4.Visibility = Visibility.Collapsed;
            Case5.Visibility = Visibility.Visible;
            Case6.Visibility = Visibility.Visible;
            Case7.Visibility = Visibility.Collapsed;
            Case8.Visibility = Visibility.Collapsed;
        }

        if (TypeComboBox.SelectedItem == "Выпечка")
        {
            Case1.Visibility = Visibility.Collapsed;
            Case2.Visibility = Visibility.Collapsed;
            Case3.Visibility = Visibility.Collapsed;
            Case4.Visibility = Visibility.Collapsed;
            Case5.Visibility = Visibility.Collapsed;
            Case6.Visibility = Visibility.Collapsed;
            Case7.Visibility = Visibility.Visible;
            Case8.Visibility = Visibility.Visible;
        }
    }

    public static void Update(ChartWindow w)
    {
        _window = w;
    }

    public static void Update()
    {
        _window = null;
    }
}
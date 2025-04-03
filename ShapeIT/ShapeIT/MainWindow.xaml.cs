using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Haley.WPF.Controls;


namespace ShapeIT
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        MenuItemModel menuItemModel;
        Figures figures;
        FiguresCache figuresCache;
        MyCanvas canvas;
        public MainWindow()
        {
            InitializeComponent();
            figuresCache = new FiguresCache();
            canvas = new MyCanvas();
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseRightButtonDown += Canvas_MouseRightButtonDown;
            canvas.Background = new SolidColorBrush(Colors.Transparent);
            Grid.SetRow(canvas, 1);


            //canvas.Background = new SolidColorBrush();
            MainGrid.Children.Add(canvas);
            figures = new Figures();
            canvas.figures = figures;
            Assembly asm = Assembly.GetEntryAssembly();
            AssemblyName asmName = asm.GetName();
            string simpleAsmName = asmName.Name;
            Type myType = Type.GetType($"{simpleAsmName}.Figure", false, false);
            figures.FigureTypes = asm.GetTypes().Where(t => t.IsSubclassOf(myType)).ToArray();
            menuItemModel = new MenuItemModel();
            for (int i = 0; i < figures.FigureTypes.Length; i++)
            {
                System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem();
                string figureTypeName = figures.FigureTypes[i].Name;
                ConstructorInfo constructorInfo = figures.FigureTypes[i].GetConstructor(Type.EmptyTypes);
                Figure newObj = (Figure)constructorInfo.Invoke(null);
                menuItem.Header = newObj.GetName();
                Image icon = new Image();

                icon.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Figures/{menuItem.Header}.png"));
                menuItem.Icon = icon;
                miFigures.Items.Add(menuItem);

            }
            miFigures.Click += Figures_Click;
            Undo.Click += Undo_Click;
            Redo.Click += Redo_Click;
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            
            if (figuresCache.Backward != 0)
            {
                Undo.IsEnabled = true;
                figuresCache.Undo();
                canvas.figures.Linker = figuresCache.GetFinalList();
                canvas.InvalidateVisual();
            }
            else
                Undo.IsEnabled = false;
            Redo.IsEnabled = true;
            
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            
            if (figuresCache.Forward != 0)
            {
                Redo.IsEnabled = true;
                figuresCache.Redo();
                canvas.figures.Linker = figuresCache.GetFinalList();
                canvas.InvalidateVisual();
            }
            else
                Redo.IsEnabled = false;
            Undo.IsEnabled = true;
        }

        private void Figures_Click(object sender, RoutedEventArgs e)
        {
            menuItemModel.SelectedItemInd = ((System.Windows.Controls.MenuItem)sender).Items.IndexOf(e.OriginalSource);
        }



        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            figures.IndPotentialFigure = menuItemModel.SelectedItemInd;
            if (figures.IndPotentialFigure != -1)
            {
                ConstructorInfo figureConstructorInfo = figures.FigureTypes[figures.IndPotentialFigure].GetConstructor(Type.EmptyTypes);
                figures.PotentialFigure = (Figure)figureConstructorInfo.Invoke(null);
                figures.PotentialFigure.Fill = (Color)btColorFill.SelectedColor;
                figures.PotentialFigure.Stroke = (Color)btColorStroke.SelectedColor;
                figures.PotentialFigure.StrokeThikness = (int)Thickness.Value;
                figures.PotentialFigure.AddPoint(e.GetPosition((MyCanvas)sender));
            }
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            int possibleInd = menuItemModel.SelectedItemInd;

            if (figures.IndPotentialFigure != -1) {

                if (!figures.Linker.Contains(figures.PotentialFigure))
                {


                    if (figures.IndPotentialFigure == possibleInd)
                    {

                        figures.PotentialFigure.AddPoint(e.GetPosition((MyCanvas)sender));
                        figures.Linker.Add(figures.PotentialFigure);
                        figuresCache.FigCacheUpdate(figures.Linker);
                        Undo.IsEnabled = true;
                    }
                }

                else
                {
                    figures.PotentialFigure.AddPoint(e.GetPosition((MyCanvas)sender));
                }
            }
            ((MyCanvas)sender).InvalidateVisual();
        }
    }

    public class MenuItemModel
    {
        private int selectedItemInd = -1;
        public int SelectedItemInd { get { return selectedItemInd; } set { if (selectedItemInd != value) { selectedItemInd = value; } } }
    }
      
}

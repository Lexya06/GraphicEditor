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
using Xceed.Wpf.Toolkit;


namespace ShapeIT
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        bool canMove = false;
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
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            
            
            canvas.Background = new SolidColorBrush(Colors.Transparent);
            Grid.SetRow(canvas, 1);

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

            figures.PotentialFigure = null;
            Undo.IsEnabled = true;
            figuresCache.Undo();
            
            canvas.figures.Linker = figuresCache.GetFinalList();
            canvas.InvalidateVisual();  
            
            if (figuresCache.Backward == 0)
                Undo.IsEnabled = false;

            Redo.IsEnabled = true;
            
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            
            Redo.IsEnabled = true;
            figuresCache.Redo();

            canvas.figures.Linker = figuresCache.GetFinalList();
            canvas.InvalidateVisual();
            
            if (figuresCache.Forward == 0)
                Redo.IsEnabled = false;
           
            Undo.IsEnabled = true;
        }

        private void Figures_Click(object sender, RoutedEventArgs e)
        {
            menuItemModel.SelectedItemInd = ((System.Windows.Controls.MenuItem)sender).Items.IndexOf(e.OriginalSource);
            figures.IndPotentialFigure = menuItemModel.SelectedItemInd;
            figures.PotentialFigure = null;
            
        }

        private Image CreateLineImage(int height,int width,int strokeThickness,Color colorBrush)
        {
            SolidColorBrush brush = new SolidColorBrush(colorBrush);
            Pen pen = new Pen(brush, strokeThickness);
            LineGeometry lineGeometry = new LineGeometry(new Point(0,height/2),new Point(width,height/2));
            DrawingImage lineImage = new DrawingImage(new GeometryDrawing(brush,pen,lineGeometry));
            Image image = new Image();
            image.Source = lineImage;
            return image;
        }

        

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (figures.IndPotentialFigure != -1)
            {
                if (figures.PotentialFigure == null)
                {
                    ConstructorInfo figureConstructorInfo = figures.FigureTypes[figures.IndPotentialFigure].GetConstructor(Type.EmptyTypes);
                    figures.PotentialFigure = (Figure)figureConstructorInfo.Invoke(null);
                    figures.PotentialFigure.AddPoint(e.GetPosition((MyCanvas)sender));
                   
                    return;
                }
                else
                {
                    figures.PotentialFigure.AddPoint(e.GetPosition((MyCanvas)sender));
                    canMove = true;


                    if (!figures.Linker.Contains(figures.PotentialFigure))
                    {
                        figures.PotentialFigure.Fill = (Color)btColorFill.SelectedColor;
                        figures.PotentialFigure.Stroke = (Color)btColorStroke.SelectedColor;
                        figures.PotentialFigure.StrokeThikness = (int)Thickness.Value;
                        figures.Linker.Add(figures.PotentialFigure);
                        figuresCache.FigCacheUpdate(figures.Linker);
                        Undo.IsEnabled = true;
                        Redo.IsEnabled = false;
                    }
                    
                    ((MyCanvas)sender).InvalidateVisual();
                }
                
            }


        }

        private void Canvas_MouseMove(object sender,MouseEventArgs e)
        {
            if (figures.PotentialFigure != null)
            {
                if (figures.IndPotentialFigure != -1 && canMove)
                {
                    figures.PotentialFigure.Points[figures.PotentialFigure.Points.Length - 1] = e.GetPosition((MyCanvas)sender);
                    ((MyCanvas)sender).InvalidateVisual();
                }
            
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canMove = false;
            if (figures.PotentialFigure != null)
            {
                if (figures.PotentialFigure.MaxPoints() == 2 && figures.Linker.Contains(figures.PotentialFigure))
                {
                    figures.PotentialFigure = null;
                }
            }
            
        }

        

        private void Pen_ValueChanged(object sender, EventArgs e)
        {

            Image newimage = CreateLineImage(20, 20, (int)(Thickness.Value), (Color)(btColorStroke.SelectedColor));
            miThickness.Icon = newimage;
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Thickness.ValueChanged += Pen_ValueChanged;
            btColorStroke.SelectedColorChanged += Pen_ValueChanged;
            Thickness.Value = 5;
            btColorStroke.SelectedColor = Colors.Black;

        }
    }

    public class MenuItemModel
    {
        private int selectedItemInd = -1;
        public int SelectedItemInd { get { return selectedItemInd; } set { if (selectedItemInd != value) { selectedItemInd = value; } } }
    }
      
}

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
        FiguresDesigner figuresDesigner;
        public MainWindow()
        {
            InitializeComponent();
            MyCanvas canvas = new MyCanvas();
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseRightButtonDown += Canvas_MouseRightButtonDown;
            canvas.Background = new SolidColorBrush(Colors.Transparent);
            Grid.SetRow(canvas, 1);


            //canvas.Background = new SolidColorBrush();
            MainGrid.Children.Add(canvas);
            
            figuresDesigner = new FiguresDesigner(new SolidColorBrush(), new SolidColorBrush());
            figures = new Figures();
            Assembly asm = Assembly.Load("ShapeIT");
            AssemblyName asmName = asm.GetName();
            string simpleAsmName = asmName.Name;
            Type myType = Type.GetType($"{simpleAsmName}.Figure", false, false);
            figures.FigureTypes = asm.GetTypes().Where(t => t.IsSubclassOf(myType)).ToArray();
            canvas.figures = figures;
            canvas.figuresDesigner = figuresDesigner;
            menuItemModel = new MenuItemModel();
            for (int i = 0; i < figures.FigureTypes.Length; i++)
            {
                MenuItem menuItem = new MenuItem();
                string figureTypeName = figures.FigureTypes[i].Name;
                ConstructorInfo constructorInfo = figures.FigureTypes[i].GetConstructor(Type.EmptyTypes);
                Figure newObj = (Figure)constructorInfo.Invoke(null);
                menuItem.Header = newObj.GetName();
                Image icon = new Image();

                icon.Source = new BitmapImage(new Uri($"pack://application:,,,/Figures/{menuItem.Header}.png"));
                menuItem.Icon = icon;
                miFigures.Items.Add(menuItem);

            }
            miFigures.Click += MenuItem_Click;
            
           
        }
        

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            menuItemModel.SelectedItemInd = ((MenuItem)sender).Items.IndexOf(e.OriginalSource);
        }
        
        

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            figures.IndPotentialFigure = menuItemModel.SelectedItemInd;
            if (figures.IndPotentialFigure != -1)
            {
                ConstructorInfo figureConstructorInfo = figures.FigureTypes[figures.IndPotentialFigure].GetConstructor(Type.EmptyTypes);
                figures.PotentialFigure = (Figure)figureConstructorInfo.Invoke(null);
                figures.PotentialFigure.AddPoint(0, e.GetPosition((MyCanvas)sender));
            }
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            int possibleInd = menuItemModel.SelectedItemInd;
            if (figures.IndPotentialFigure != -1)
            {
                if (figures.IndPotentialFigure == possibleInd)
                {
                    int ind = figures.PotentialFigure.Points.Length - 1;
                    figures.PotentialFigure.AddPoint(ind, e.GetPosition((MyCanvas)sender));
                    figures.Linker.Add(figures.PotentialFigure);
                    ((MyCanvas)sender).InvalidateVisual();
                }
            }
        }

        
    }

    public class MyCanvas : Canvas
    {
        public Figures figures { get; set; }
        public FiguresDesigner figuresDesigner { get; set; }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            this.figures.Draw(drawingContext, figuresDesigner.ColorBrushFill, figuresDesigner.ColorBrushStroke);
        }
    }


    public class MenuItemModel
    {


        private int selectedItemInd = -1;
        public int SelectedItemInd { get { return selectedItemInd; } set { if (selectedItemInd != value) { selectedItemInd = value;  } } }
        
        
        
    }
   
  
    public class Figures
    {
        public Figures()
        {
            Linker = new List<Figure>();
        }
        public Type[] FigureTypes { get; set; }
        public int IndPotentialFigure {  get; set; } = -1;
        public Figure PotentialFigure { get; set; }
        public List<Figure> Linker { get; set; }
       
        public void Draw(DrawingContext drawingContext,SolidColorBrush brushFill,SolidColorBrush brushStroke)
        {
            for (int i = 0; i < Linker.Count; i++)
            {
                Linker[i].DrawShape(drawingContext, brushFill,brushStroke);
            }
        }
    }
        
   
   public class FiguresDesigner
   {

        public static int confirmDist = 3;
        public SolidColorBrush ColorBrushFill { get; set; }
        public SolidColorBrush ColorBrushStroke { get; set; }
        public FiguresDesigner()
        {

        }
        
        public FiguresDesigner(SolidColorBrush brushFill,SolidColorBrush brushStroke)
        {
            ColorBrushFill = brushFill;
            ColorBrushStroke = brushStroke; 
        }
        


    }
  
    public abstract class Figure
    {

        public virtual string GetName() 
        {
            return "Figure";
        }
        public Point[] Points { get; set; }
        public virtual void AddPoint(int numArrPoint, Point cord)
        {
            Points[numArrPoint] = cord;
        }
       
       
        public abstract void DrawShape(DrawingContext drawingContext,SolidColorBrush brushFill,SolidColorBrush brushStroke);
        public Color Fill { get; set; }
        public int StrokeThikness { get; set; }
        public Color Stroke { get; set; }
    }
    
    
}

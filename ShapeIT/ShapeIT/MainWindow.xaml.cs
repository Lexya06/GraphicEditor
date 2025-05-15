using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using FigureAbstract;
using System.Windows.Shell;


namespace ShapeIT
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        OpenFileDialog SerializeOpenFileDialog;
        OpenFileDialog PluginOpenFileDialog;
        SaveFileDialog SerializeSaveFileDialog;
        bool canMove = false;
        MenuItemModel menuItemModel;
        Figures figures;
        FiguresCache figuresCache;
        MyCanvas canvas;
        string currProjectName;
        bool isSaved;
        List<Figure> tempList;
        public static string WorkDir { get; set; }

        private void SerializeDialogInit(FileDialog dialog)
        {
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON файлы (*.json)|*json";
            dialog.AddExtension = true;
            dialog.InitialDirectory = WorkDir + "\\Drawings";

        }

        private void PluginDialogInit(FileDialog dialog)
        {
            dialog.DefaultExt = ".dll";
            dialog.Filter = "DLL файлы (*.dll)|*.dll";
            dialog.AddExtension = true;
        }

        private bool DialogOpen(FileDialog dialog,ref string fileName)
        {
            bool value = false;
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                fileName = dialog.FileName;
                value = true;
            }
          
            return value;
        }

        private bool SaveFileOperation(ref string fileName)
        {
            bool result = true;
            if (!isSaved)
            {
                result = DialogOpen(SerializeSaveFileDialog, ref fileName);
                if (result == true)
                {
                    isSaved = true;
                    
                }
                else
                    return result;
            }
            FiguresSerialize.FileSerialize(currProjectName,figures.Linker);
            tempList.Clear();
            tempList.AddRange(figures.Linker);
            return result;
        }
        private bool ConfirmFileOperation(ref string fileName)
        {
            bool mustBeSave = false;
            bool answer = true;
            MessageBoxResult result;
            if (!tempList.SequenceEqual<Figure>(figures.Linker))
            {

                result = System.Windows.MessageBox.Show("You have unsaved changes. Do you want to save current work?", "Info",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    mustBeSave = SaveFileOperation(ref fileName);
                }
                answer = ((result == MessageBoxResult.Yes && mustBeSave) || (result == MessageBoxResult.No && !mustBeSave));
              
            }
            return answer;
            
        }
        public MainWindow()
        {
            
            InitializeComponent();
            WorkDir = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName;

            PluginOpenFileDialog = new OpenFileDialog();
            PluginDialogInit(PluginOpenFileDialog);

            SerializeOpenFileDialog = new OpenFileDialog();
            SerializeDialogInit(SerializeOpenFileDialog);

            SerializeSaveFileDialog = new SaveFileDialog();
            SerializeDialogInit(SerializeSaveFileDialog);
            SerializeSaveFileDialog.FileName = "No name.json";

            tempList = new List<Figure>();
            

            currProjectName = "No name.json";
            this.Title = $"{currProjectName} - ShapeIT";
            isSaved = false;

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
            figures.FigureTypes = new List<Type>();
            menuItemModel = new MenuItemModel();

            Assembly asm = Assembly.GetExecutingAssembly();
            Type myType = Type.GetType($"FigureAbstract.Figure,FigureAbstract", false, false);
            LoadTypesFromAssembly(asm, myType);
            LoadPlugins(myType);

            Image icon;
            System.Windows.Controls.MenuItem pluginItem = new System.Windows.Controls.MenuItem();
            pluginItem.Header = "Add plugin";
            icon = new Image();
            icon.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Figures/Plugin.png"));
            pluginItem.Icon = icon;
            miFigures.Items.Add(pluginItem);

            miFigures.Click += Figures_Click;
            miFile.Click += File_Click;
            Undo.Click += Undo_Click;
            Redo.Click += Redo_Click;
        }

        private void LoadTypesFromAssembly(Assembly assembly,Type myType)
        {
            List<Type> addedTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(myType)).ToList();
            figures.FigureTypes.AddRange(addedTypes);
            Image icon;
            for (int i = 0; i < addedTypes.Count; i++)
            {
                System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem();
                string figureTypeName = addedTypes[i].Name;
                ConstructorInfo constructorInfo = addedTypes[i].GetConstructor(Type.EmptyTypes);
                Figure newObj = (Figure)constructorInfo.Invoke(null);
                menuItem.Header = newObj.GetName();
                icon = new Image();

                icon.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Figures/{menuItem.Header}.png"));
                menuItem.Icon = icon;
                miFigures.Items.Add(menuItem);

            }
        }

        private void LoadPlugins(Type myType)
        {
            string[] pluginNames = Directory.GetFiles(WorkDir + "\\Plugins","*.dll");
            foreach (string pluginName in pluginNames)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(pluginName);
                    LoadTypesFromAssembly(assembly, myType);
                }
                catch (Exception)
                {
                    if (File.Exists(pluginName))
                    {
                        File.Delete(pluginName);
                        MessageBox.Show("Plugin file damaged", "Alarm", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    
                }
            }
        }


        private void Undo_Click(object sender, EventArgs e)
        {

            //figures.PotentialFigure = null;
            Undo.IsEnabled = true;
            figuresCache.Undo();
            
            canvas.figures.Linker = figuresCache.GetFinalList();
            canvas.InvalidateVisual();  
            
            if (figuresCache.Cache.Count == 0)
                Undo.IsEnabled = false;

            Redo.IsEnabled = true;
            
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            
            Redo.IsEnabled = true;
            figuresCache.Redo();

            canvas.figures.Linker = figuresCache.GetFinalList();
            canvas.InvalidateVisual();
            
            if (figuresCache.PopFigure.Count == 0)
                Redo.IsEnabled = false;
           
            Undo.IsEnabled = true;
        }

        private async void Figures_Click(object sender, RoutedEventArgs e)
        {
            menuItemModel.SelectedItemInd = ((System.Windows.Controls.MenuItem)sender).Items.IndexOf(e.OriginalSource);
            if (menuItemModel.SelectedItemInd == figures.FigureTypes.Count)
            {
                string loadedFileName = "";
                if (DialogOpen(PluginOpenFileDialog, ref loadedFileName))
                {
                    if (await PluginsInteraction.AddPluginAsync(loadedFileName))
                    {
                        MessageBox.Show("The plugin will be added the next time you log into the application", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("This plugin already in use,rename you plugin or delete existing", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                figures.IndPotentialFigure = menuItemModel.SelectedItemInd;
                figures.PotentialFigure = null;
            }
            
        }

        private void File_Click(object sender, RoutedEventArgs e)
        {
            
            menuItemModel.SelectedItemInd = ((System.Windows.Controls.MenuItem)sender).Items.IndexOf(e.OriginalSource);
            if (menuItemModel.SelectedItemInd < 0)
                return;

       
            switch (menuItemModel.SelectedItemInd)
            {
               
                case FiguresSerialize.fCreate:
                    if (ConfirmFileOperation(ref currProjectName))
                    {
                        isSaved = false;
                        figures.Linker.Clear();
                        figuresCache.FigCacheUpdate(figures.Linker);
                        currProjectName = "No name.json";
                        tempList.Clear();
                        this.Title = $"{currProjectName} - ShapeIT";
                    }
                    
                break;

                case FiguresSerialize.fOpen:
                    if (ConfirmFileOperation(ref currProjectName))
                    {

                        if (DialogOpen(SerializeOpenFileDialog, ref currProjectName))
                        {
                            List<Figure> newFigures = new List<Figure>();
                            FiguresSerialize.FileDeserialize(currProjectName, ref newFigures);
                            figuresCache.FigCacheUpdate(newFigures);
                            figures.Linker = newFigures;
                            tempList.Clear();
                            tempList.AddRange(figures.Linker);
                            this.Title = $"{currProjectName} - ShapeIT";
                        }
                    }
                    
                break;

                case FiguresSerialize.fSave:
                    SaveFileOperation(ref currProjectName);
                    this.Title = $"{currProjectName} - ShapeIT";
                break;


            }
            canvas.InvalidateVisual();
            
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
                    if (figures.PotentialFigure.DotsFilled >= figures.PotentialFigure.MinPoints())
                    {
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
                    figures.PotentialFigure.ReplacePoint(figures.PotentialFigure.Points.Length - 1,e.GetPosition((MyCanvas)sender));
                    ((MyCanvas)sender).InvalidateVisual();
                }
            
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canMove = false;
            if (figures.PotentialFigure != null)
            {
                if ((figures.PotentialFigure.DotsFilled == figures.PotentialFigure.MaxPoints()) && figures.Linker.Contains(figures.PotentialFigure))
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!tempList.SequenceEqual(figures.Linker))
            {
                if (!ConfirmFileOperation(ref currProjectName))
                {
                    e.Cancel = true;
                }
               
                  
            }
        }
    }

   
      
}

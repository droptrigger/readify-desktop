using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Readify.Controls
{
    /// <summary>
    /// Логика взаимодействия для LoadingSpinnerControl.xaml
    /// </summary>
    public partial class LoadingSpinnerControl : UserControl
    {
        /// <summary>
        /// Свойство <see cref="LoadingSpinnerControl"/> элемента, показывающее грузит ли оно или нет
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadingSpinnerControl), new PropertyMetadata(false));

        /// <summary>
        /// Свойство <see cref="LoadingSpinnerControl"/> элемента, устанавливающее диаметр для спиннера
        /// </summary>
        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register("Diameter", typeof(double), typeof(LoadingSpinnerControl), new PropertyMetadata(100.0));

        /// <summary>
        /// Свойство <see cref="LoadingSpinnerControl"/> элемента, устанавливающее диаметр для спиннера
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(LoadingSpinnerControl), new PropertyMetadata(1.0));

        /// <summary>
        /// Свойство <see cref="LoadingSpinnerControl"/> элемента, задающее для него Cap
        /// </summary>
        public static readonly DependencyProperty CapProperty =
            DependencyProperty.Register("Cap", typeof(PenLineCap), typeof(LoadingSpinnerControl), new PropertyMetadata(PenLineCap.Flat));

        /// <summary>
        /// Свойство <see cref="LoadingSpinnerControl"/> элемента, задающее для него цвет
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(LoadingSpinnerControl), new PropertyMetadata(Brushes.White));

        /// <summary>
        /// Устанавливает и отдает значение свойства <see cref="IsLoadingProperty"/>
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return (bool)GetValue(IsLoadingProperty);
            }
            set
            {
                SetValue(IsLoadingProperty, value);
            }
        }

        /// <summary>
        /// Устанавливает и отдает значение свойства <see cref="DiameterProperty"/>
        /// </summary>
        public double Diameter
        {
            get
            {
                return (double)GetValue(DiameterProperty);
            }
            set
            {
                SetValue(DiameterProperty, value);
            }
        }

        /// <summary>
        /// Устанавливает и отдает значение свойства <see cref="ThicknessProperty"/>
        /// </summary>
        public double Thickness
        {
            get
            {
                return (double)GetValue(ThicknessProperty);
            }
            set
            {
                SetValue(ThicknessProperty, value);
            }
        }

        /// <summary>
        /// Устанавливает и отдает значение свойства <see cref="CapProperty"/>
        /// </summary>
        public PenLineCap Cap
        {
            get
            {
                return (PenLineCap)GetValue(CapProperty);
            }
            set
            {
                SetValue(CapProperty, value);
            }
        }

        /// <summary>
        /// Устанавливает и отдает значение свойства <see cref="ColorProperty"/>
        /// </summary>
        public Brush Color
        {
            get
            {
                return (Brush)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public LoadingSpinnerControl()
        {
            InitializeComponent();
        }
    }
}

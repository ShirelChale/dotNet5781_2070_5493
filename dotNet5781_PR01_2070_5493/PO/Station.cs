using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace dotNet5781_PR01_2070_5493.PO
{
    public class Station : DependencyObject
    {

        public int Code
        {
            get { return (int)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Code.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register("Code", typeof(int), typeof(Station), new PropertyMetadata(default(int)));



        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Station), new PropertyMetadata(default(string)));



        public int Longitude
        {
            get { return (int)GetValue(LongitudeProperty); }
            set { SetValue(LongitudeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Longitude.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LongitudeProperty =
            DependencyProperty.Register("Longitude", typeof(int), typeof(Station), new PropertyMetadata(default(int)));



        public int Lattitude
        {
            get { return (int)GetValue(LattitudeProperty); }
            set { SetValue(LattitudeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Lattitude.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LattitudeProperty =
            DependencyProperty.Register("Lattitude", typeof(int), typeof(Station), new PropertyMetadata(default(int)));



        public ObservableCollection<BL.BO.Line> Lines
        {
            get { return (ObservableCollection<BL.BO.Line>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Lines.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinesProperty =
            DependencyProperty.Register("Lines", typeof(ObservableCollection<BL.BO.Line>), typeof(Station), new PropertyMetadata(default(ObservableCollection<BL.BO.Line>)));

    }
}

﻿using DataPresenter.DataSources.OData;
using Infragistics.Windows.Controls;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Themes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

namespace DataPresenter.DataSources.OData.SampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		#region Member Variables
		private Infragistics.Windows.DataPresenter.GridView			_gridView;
		private Infragistics.Windows.DataPresenter.CardView			_cardView;
		private Infragistics.Windows.DataPresenter.CarouselView		_carouselView;
		private Infragistics.Windows.DataPresenter.TreeView			_treeView;
		#endregion //Member Variables

		#region Constructor
		public MainWindow()
        {
            InitializeComponent();

			this.numDesiredPageSize.Value						= 200;
			this.numMaximumCachedPages.Value					= 200;
			this.cboOdataSources.SelectedIndex					= 0;
			this.cboDataPresenterView.SelectedIndex				= 0;
			this.cboRecordFilterLogicalOperator.SelectedIndex	= 0;
		}
		#endregion //Constructor

		#region Private Properties

		#region CardView
		private Infragistics.Windows.DataPresenter.CardView CardView
		{
			get
			{
				if (null == this._cardView)
					this._cardView = new Infragistics.Windows.DataPresenter.CardView();

				return this._cardView;
			}
		}
		#endregion //CardView

		#region CarouselView
		private Infragistics.Windows.DataPresenter.CarouselView CarouselView
		{
			get
			{
				if (null == this._carouselView)
					this._carouselView = new Infragistics.Windows.DataPresenter.CarouselView();

				return this._carouselView;
			}
		}
		#endregion //CarouselView

		#region CurrentDataSource
		private ODataDataSource CurrentDataSource
		{
			get
			{
				if (null != this.dataPresenter1)
					return this.dataPresenter1.DataSource as ODataDataSource;

				return null;
			}
		}
		#endregion //CurrentDataSource

		#region DesiredPageSize
		private int DesiredPageSize
		{
			get { return Convert.ToInt32((double)this.numDesiredPageSize.Value); }
		}
		#endregion //DesiredPageSize

		#region GridView
		private Infragistics.Windows.DataPresenter.GridView GridView
		{
			get
			{
				if (null == this._gridView)
					this._gridView = new Infragistics.Windows.DataPresenter.GridView();

				return this._gridView;
			}
		}
		#endregion //GridView

		#region MaximumCachedPages
		private int MaximumCachedPages
		{
			get { return Convert.ToInt32((double)this.numMaximumCachedPages.Value); }
		}
		#endregion //MaximumCachedPages

		#region TreeView
		private Infragistics.Windows.DataPresenter.TreeView TreeView
		{
			get
			{
				if (null == this._treeView)
					this._treeView = new Infragistics.Windows.DataPresenter.TreeView();

				return this._treeView;
			}
		}
		#endregion //TreeView

		#endregion //Private Properties

		#region Event Handlers

		#region cboDataPresenterView_SelectionChanged
		private void cboDataPresenterView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (null == this.dataPresenter1)
				return;

			string selectedView = (string)this.cboDataPresenterView.SelectedValue;
			switch (selectedView)
			{
				case "CardView":
					this.dataPresenter1.View = this.CardView;
					break;
				case "CarouselView":
					this.dataPresenter1.View = this.CarouselView;
					break;
				case "GridView":
					this.dataPresenter1.View = this.GridView;
					break;
				case "TreeView":
					this.dataPresenter1.View = this.TreeView;
					break;
			}
		}
		#endregion //cboDataPresenterView_SelectionChanged

		#region cboOdataSources_SelectionChanged
		private void cboOdataSources_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ODataSourceListItem ods = this.cboOdataSources.SelectedItem as ODataSourceListItem;
			if (null != ods)
			{
				// If requested, null out the grid's DataSource before setting the new one.
				if (this.chkNullOutDatasource.IsChecked.HasValue && this.chkNullOutDatasource.IsChecked.Value == true)
					this.dataPresenter1.DataSource = null;

				// Set the grid's DataSource to an instance of an ODataDataSource for the selected Uri and EntitySet..
				this.dataPresenter1.DataSource = new ODataDataSource { BaseUri = ods.BaseUri, EntitySet = ods.EntitySet, DesiredPageSize = this.DesiredPageSize, MaximumCachedPages = this.MaximumCachedPages };
			}
		}
		#endregion //cboOdataSources_SelectionChanged

		private void cboRecordFilterLogicalOperator_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (null == this.dataPresenter1)
				return;

			string logicalOperator = (string)this.cboRecordFilterLogicalOperator.SelectedValue;
			switch (logicalOperator)
			{
				case "And":
					this.dataPresenter1.FieldLayoutSettings.RecordFiltersLogicalOperator = LogicalOperator.And;
					break;
				case "Or":
					this.dataPresenter1.FieldLayoutSettings.RecordFiltersLogicalOperator = LogicalOperator.Or;
					break;
			}
		}

		#region chkShowProgressBar_Checked
		private void chkShowProgressBar_Checked(object sender, RoutedEventArgs e)
		{
			if (this.chkShowProgressBar.IsChecked.Value)
			{
				FrameworkElementFactory fef = new FrameworkElementFactory(typeof(ProgressBar));
				fef.SetValue(ProgressBar.IsIndeterminateProperty, true);
				fef.SetValue(ProgressBar.WidthProperty, 100d);
				fef.SetValue(ProgressBar.HeightProperty, 5d);

				DataTemplate customTemplate = new DataTemplate { VisualTree = fef };
				this.dataPresenter1.FieldLayoutSettings.DynamicDataPendingContentTemplate = customTemplate;
			}
			else
				this.dataPresenter1.FieldLayoutSettings.DynamicDataPendingContentTemplate = null;
		}
		#endregion //chkShowProgressBar_Checked

		#region numDesiredPageSize_EditModeEnded
		private void numDesiredPageSize_EditModeEnded(object sender, Infragistics.Windows.Editors.Events.EditModeEndedEventArgs e)
		{
			if (null != this.CurrentDataSource)
				this.CurrentDataSource.DesiredPageSize = this.DesiredPageSize;
		}
		#endregion //numDesiredPageSize_EditModeEnded

		#region numMaximumCachedPages_EditModeEnded
		private void numMaximumCachedPages_EditModeEnded(object sender, Infragistics.Windows.Editors.Events.EditModeEndedEventArgs e)
		{
			if (null != this.CurrentDataSource)
				this.CurrentDataSource.MaximumCachedPages = this.MaximumCachedPages;
		}
		#endregion //numMaximumCachedPages_EditModeEnded

		#region txtPendingMessage_TextChanged
		private void txtPendingMessage_TextChanged(object sender, TextChangedEventArgs e)
		{
			// Update the dynamic resource string for the 'dynamic data pending' message.
			Infragistics.Windows.DataPresenter.Resources.Customizer.SetCustomizedString("DynamicDataPendingMessage", ((TextBox)sender).Text);
		}
		#endregion //txtPendingMessage_TextChanged

		#endregion //Event Handlers
	}

	#region ODataSourceListItem Class
	public class ODataSourceListItem
	{
		public override string ToString() { return Description; }

		public string BaseUri { get; set; }
		public string Description { get; set; }
		public string EntitySet{ get; set; }
	}
	#endregion //ODataSourceListItem Class

	#region ColorToBrushConverter Class
	public class ColorToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Process(value, targetType, parameter, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Process(value, targetType, parameter, culture);
		}

		private static object Process(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string)
			{
				Color color = Parse((string)value);

				if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
					return new SolidColorBrush(color);
				else
				if (targetType == typeof(Color) || targetType == typeof(Nullable<Color>))
					return color;
			}

			if (value is SolidColorBrush)
			{
				if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
					return value;
				else
				if (targetType == typeof(Color) || targetType == typeof(Nullable<Color>))
					return ((SolidColorBrush)value).Color;
			}

			if (value == null)
			{
				if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
					return new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
				else
				if (targetType == typeof(Color) || targetType == typeof(Nullable<Color>))
					return Color.FromArgb(0, 0, 0, 0);
			}

			if (value is Color)
			{
				if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
					return new SolidColorBrush((Color)value);
				else
				if (targetType == typeof(Color) || targetType == typeof(Nullable<Color>))
					return value;
			}

			throw new NotSupportedException("ColorToBrushConverter cannot convert value!");
		}

		private static Color Parse(string color)
		{
			var offset = color.StartsWith("#") ? 1 : 0;

			var a = Byte.Parse(color.Substring(0 + offset, 2), NumberStyles.HexNumber);
			var r = Byte.Parse(color.Substring(2 + offset, 2), NumberStyles.HexNumber);
			var g = Byte.Parse(color.Substring(4 + offset, 2), NumberStyles.HexNumber);
			var b = Byte.Parse(color.Substring(6 + offset, 2), NumberStyles.HexNumber);

			return Color.FromArgb(a, r, g, b);
		}
	}
	#endregion //ColorToBrushConverter Class
}

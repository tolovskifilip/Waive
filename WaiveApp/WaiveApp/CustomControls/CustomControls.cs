using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WaiveApp.CustomControls
{
    public class CustomCell : ViewCell
    {
        Label dateTime_of_PredictionLabel, renewableShareLabel;

        public static readonly BindableProperty dateTime_of_PredictionProperty = BindableProperty.Create("dateTime_of_Prediction", typeof(DateTime), typeof(CustomCell), new DateTime(2021, 1, 1, 0, 0, 0));
        public static readonly BindableProperty renewableShareProperty = BindableProperty.Create("renewableShare", typeof(double), typeof(CustomCell), 0.0);

        public DateTime dateTime_of_Prediction
        {
            get { return (DateTime)GetValue(dateTime_of_PredictionProperty); }
            set { SetValue(dateTime_of_PredictionProperty, value); }
        }

        public double renewableShare
        {
            get { return (double)GetValue(renewableShareProperty); }
            set { SetValue(renewableShareProperty, value); }
        }



        public CustomCell()
        {
            var grid = new Grid { Padding = new Thickness(10) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });

            dateTime_of_PredictionLabel = new Label { FontAttributes = FontAttributes.Bold };  
            renewableShareLabel = new Label();

            grid.Children.Add(dateTime_of_PredictionLabel);
            grid.Children.Add(renewableShareLabel, 1, 0);


            View = grid;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                dateTime_of_PredictionLabel.Text = dateTime_of_Prediction.ToString();
                if (renewableShare>=Constants.renewability_border_global)
                {
                    renewableShareLabel.TextColor = Color.FromRgb(24, 91, 59); ;
                    renewableShareLabel.Text = renewableShare.ToString(); 
                }
                else
                {
                    renewableShareLabel.TextColor = Color.FromRgb(149, 35, 35);
                    renewableShareLabel.Text = renewableShare.ToString();
                }
            }
        }
    }
}

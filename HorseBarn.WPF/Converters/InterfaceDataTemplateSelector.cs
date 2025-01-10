﻿using HorseBarn.lib.Horse;
using HorseBarn.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace HorseBarn.WPF.Converters
{
    public class InterfaceDataTemplateSelector : DataTemplateSelector
    {

        private DataTemplate[] _dataTemplates;

        public DataTemplate[] DataTemplates
        {
            get { return _dataTemplates; }
            set { _dataTemplates = value; }
        }


        public DataTemplate NullTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return base.SelectTemplate(item,container);

            Type? t = item?.GetType();

            if(item is HorseViewModel cvm)
            {
                t = cvm.Horse?.GetType();
            }

            if (t == null) return NullTemplate;

            foreach (var dt in DataTemplates)
                {
                    if (((Type)dt.DataType).IsAssignableFrom(t))
                    {
                        return dt;
                    }
                }
            

            return base.SelectTemplate(item, container);

        }

    }
}
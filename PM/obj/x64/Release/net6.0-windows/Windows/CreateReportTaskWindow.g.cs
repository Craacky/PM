﻿#pragma checksum "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "025B6149CFB5CCD6FF2458063C188CD40BCAC7C6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PM.Controls;
using PM.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace PM.Windows {
    
    
    /// <summary>
    /// CreateReportTaskWindow
    /// </summary>
    public partial class CreateReportTaskWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 68 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox gtinComboBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PM;component/windows/createreporttaskwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 43 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.gtinComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 72 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            this.gtinComboBox.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent, new System.Windows.Controls.TextChangedEventHandler(this.gtinComboBox_TextChanged));
            
            #line default
            #line hidden
            
            #line 74 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            this.gtinComboBox.DropDownClosed += new System.EventHandler(this.gtinComboBox_DropDownClosed);
            
            #line default
            #line hidden
            
            #line 75 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            this.gtinComboBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.gtinComboBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 81 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.TextBox_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 82 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.TextBox_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 88 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            ((System.Windows.Controls.DatePicker)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.DatePicker_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 100 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.TextBox_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 101 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.TextBox_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 102 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewKeyUp += new System.Windows.Input.KeyEventHandler(this.TextBox_PreviewKeyUp);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 109 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.TextBox_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 110 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.TextBox_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 111 "..\..\..\..\..\Windows\CreateReportTaskWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewKeyUp += new System.Windows.Input.KeyEventHandler(this.TextBox_PreviewKeyUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


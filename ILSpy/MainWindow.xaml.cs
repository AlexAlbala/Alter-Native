﻿// Copyright (c) 2011 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ICSharpCode.Decompiler;
using ICSharpCode.ILSpy.Debugger;
using ICSharpCode.ILSpy.TextView;
using ICSharpCode.ILSpy.TreeNodes;
using ICSharpCode.ILSpy.XmlDoc;
using ICSharpCode.TreeView;
using Microsoft.Win32;
using Mono.Cecil;

namespace ICSharpCode.ILSpy
{
	/// <summary>
	/// The main window of the application.
	/// </summary>
	partial class MainWindow : Window
	{
		readonly NavigationHistory<NavigationState> history = new NavigationHistory<NavigationState>();
		ILSpySettings spySettings;
		internal SessionSettings sessionSettings;

		internal AssemblyListManager assemblyListManager;
		AssemblyList assemblyList;
		AssemblyListTreeNode assemblyListTreeNode;
		
		[Import]
		DecompilerTextView decompilerTextView = null;
		
		static MainWindow instance;
		
		public static MainWindow Instance {
			get { return instance; }
		}
		
		public SessionSettings SessionSettings {
			get { return sessionSettings; }
		}
		
		public MainWindow()
		{
			instance = this;
			spySettings = ILSpySettings.Load();
			this.sessionSettings = new SessionSettings(spySettings);
			this.assemblyListManager = new AssemblyListManager(spySettings);
			
			this.Icon = new BitmapImage(new Uri("pack://application:,,,/ILSpy;component/images/ILSpy.ico"));
			
			this.DataContext = sessionSettings;
			
			InitializeComponent();
			App.CompositionContainer.ComposeParts(this);
			mainPane.Content = decompilerTextView;
			
			if (sessionSettings.SplitterPosition > 0 && sessionSettings.SplitterPosition < 1) {
				leftColumn.Width = new GridLength(sessionSettings.SplitterPosition, GridUnitType.Star);
				rightColumn.Width = new GridLength(1 - sessionSettings.SplitterPosition, GridUnitType.Star);
			}
			sessionSettings.FilterSettings.PropertyChanged += filterSettings_PropertyChanged;
			
			InitMainMenu();
			InitToolbar();
			ContextMenuProvider.Add(treeView, decompilerTextView);
			
			this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
		}
		
		void SetWindowBounds(Rect bounds)
		{
			this.Left = bounds.Left;
			this.Top = bounds.Top;
			this.Width = bounds.Width;
			this.Height = bounds.Height;
		}
		
		#region Toolbar extensibility
		[ImportMany("ToolbarCommand", typeof(ICommand))]
		Lazy<ICommand, IToolbarCommandMetadata>[] toolbarCommands = null;
		
		void InitToolbar()
		{
			int navigationPos = 0;
			int openPos = 1;
			foreach (var commandGroup in toolbarCommands.OrderBy(c => c.Metadata.ToolbarOrder).GroupBy(c => c.Metadata.ToolbarCategory)) {
				if (commandGroup.Key == "Navigation") {
					foreach (var command in commandGroup) {
						toolBar.Items.Insert(navigationPos++, MakeToolbarItem(command));
						openPos++;
					}
				} else if (commandGroup.Key == "Open") {
					foreach (var command in commandGroup) {
						toolBar.Items.Insert(openPos++, MakeToolbarItem(command));
					}
				} else {
					toolBar.Items.Add(new Separator());
					foreach (var command in commandGroup) {
						toolBar.Items.Add(MakeToolbarItem(command));
					}
				}
			}
			
		}
		
		Button MakeToolbarItem(Lazy<ICommand, IToolbarCommandMetadata> command)
		{
			return new Button {
				Command = CommandWrapper.Unwrap(command.Value),
				ToolTip = command.Metadata.ToolTip,
				Tag = command.Metadata.Tag,
				Content = new Image {
					Width = 16,
					Height = 16,
					Source = Images.LoadImage(command.Value, command.Metadata.ToolbarIcon)
				}
			};
		}
		#endregion
		
		#region Main Menu extensibility
		[ImportMany("MainMenuCommand", typeof(ICommand))]
		Lazy<ICommand, IMainMenuCommandMetadata>[] mainMenuCommands = null;
		
		void InitMainMenu()
		{
			foreach (var topLevelMenu in mainMenuCommands.OrderBy(c => c.Metadata.MenuOrder).GroupBy(c => c.Metadata.Menu)) {
				var topLevelMenuItem = mainMenu.Items.OfType<MenuItem>().FirstOrDefault(m => (m.Header as string) == topLevelMenu.Key);
				foreach (var category in topLevelMenu.GroupBy(c => c.Metadata.MenuCategory)) {
					if (topLevelMenuItem == null) {
						topLevelMenuItem = new MenuItem();
						topLevelMenuItem.Header = topLevelMenu.Key;
						mainMenu.Items.Add(topLevelMenuItem);
					} else if (topLevelMenuItem.Items.Count > 0) {
						topLevelMenuItem.Items.Add(new Separator());
					}
					foreach (var entry in category) {
						MenuItem menuItem = new MenuItem();
						menuItem.Command = CommandWrapper.Unwrap(entry.Value);
						if (!string.IsNullOrEmpty(entry.Metadata.Header))
							menuItem.Header = entry.Metadata.Header;
						if (!string.IsNullOrEmpty(entry.Metadata.MenuIcon)) {
							menuItem.Icon = new Image {
								Width = 16,
								Height = 16,
								Source = Images.LoadImage(entry.Value, entry.Metadata.MenuIcon)
							};
						}
						
						menuItem.IsEnabled = entry.Metadata.IsEnabled;
						menuItem.InputGestureText = entry.Metadata.InputGestureText;
						topLevelMenuItem.Items.Add(menuItem);
					}
				}
			}
		}
		#endregion
		
		#region Message Hook
		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			PresentationSource source = PresentationSource.FromVisual(this);
			HwndSource hwndSource = source as HwndSource;
			if (hwndSource != null) {
				hwndSource.AddHook(WndProc);
			}
			// Validate and Set Window Bounds
			Rect bounds = Rect.Transform(sessionSettings.WindowBounds, source.CompositionTarget.TransformToDevice);
			var boundsRect = new System.Drawing.Rectangle((int)bounds.Left, (int)bounds.Top, (int)bounds.Width, (int)bounds.Height);
			bool boundsOK = false;
			foreach (var screen in System.Windows.Forms.Screen.AllScreens) {
				var intersection = System.Drawing.Rectangle.Intersect(boundsRect, screen.WorkingArea);
				if (intersection.Width > 10 && intersection.Height > 10)
					boundsOK = true;
			}
			if (boundsOK)
				SetWindowBounds(sessionSettings.WindowBounds);
			else
				SetWindowBounds(SessionSettings.DefaultWindowBounds);
			
			this.WindowState = sessionSettings.WindowState;
		}
		
		unsafe IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == NativeMethods.WM_COPYDATA) {
				CopyDataStruct* copyData = (CopyDataStruct*)lParam;
				string data = new string((char*)copyData->Buffer, 0, copyData->Size / sizeof(char));
				if (data.StartsWith("ILSpy:\r\n", StringComparison.Ordinal)) {
					data = data.Substring(8);
					List<string> lines = new List<string>();
					using (StringReader r = new StringReader(data)) {
						string line;
						while ((line = r.ReadLine()) != null)
							lines.Add(line);
					}
					var args = new CommandLineArguments(lines);
					if (HandleCommandLineArguments(args)) {
						if (!args.NoActivate && WindowState == WindowState.Minimized)
							WindowState = WindowState.Normal;
						HandleCommandLineArgumentsAfterShowList(args);
						handled = true;
						return (IntPtr)1;
					}
				}
			}
			return IntPtr.Zero;
		}
		#endregion
		
		public AssemblyList CurrentAssemblyList {
			get { return assemblyList; }
		}
		
		public event NotifyCollectionChangedEventHandler CurrentAssemblyListChanged;
		
		List<LoadedAssembly> commandLineLoadedAssemblies = new List<LoadedAssembly>();
		
		bool HandleCommandLineArguments(CommandLineArguments args)
		{
			foreach (string file in args.AssembliesToLoad) {
				commandLineLoadedAssemblies.Add(assemblyList.OpenAssembly(file));
			}
			if (args.Language != null)
				sessionSettings.FilterSettings.Language = Languages.GetLanguage(args.Language);
			return true;
		}
		
		void HandleCommandLineArgumentsAfterShowList(CommandLineArguments args)
		{
			if (args.NavigateTo != null) {
				bool found = false;
				if (args.NavigateTo.StartsWith("N:", StringComparison.Ordinal)) {
					string namespaceName = args.NavigateTo.Substring(2);
					foreach (LoadedAssembly asm in commandLineLoadedAssemblies) {
						AssemblyTreeNode asmNode = assemblyListTreeNode.FindAssemblyNode(asm);
						if (asmNode != null) {
							NamespaceTreeNode nsNode = asmNode.FindNamespaceNode(namespaceName);
							if (nsNode != null) {
								found = true;
								SelectNode(nsNode);
								break;
							}
						}
					}
				} else {
					foreach (LoadedAssembly asm in commandLineLoadedAssemblies) {
						AssemblyDefinition def = asm.AssemblyDefinition;
						if (def != null) {
							MemberReference mr = XmlDocKeyProvider.FindMemberByKey(def.MainModule, args.NavigateTo);
							if (mr != null) {
								found = true;
								JumpToReference(mr);
								break;
							}
						}
					}
				}
				if (!found) {
					AvalonEditTextOutput output = new AvalonEditTextOutput();
					output.Write(string.Format("Cannot find '{0}' in command line specified assemblies.", args.NavigateTo));
					decompilerTextView.ShowText(output);
				}
			} else if (commandLineLoadedAssemblies.Count == 1) {
				// NavigateTo == null and an assembly was given on the command-line:
				// Select the newly loaded assembly
				JumpToReference(commandLineLoadedAssemblies[0].AssemblyDefinition);
			}
			commandLineLoadedAssemblies.Clear(); // clear references once we don't need them anymore
		}
		
		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			ILSpySettings spySettings = this.spySettings;
			this.spySettings = null;
			
			// Load AssemblyList only in Loaded event so that WPF is initialized before we start the CPU-heavy stuff.
			// This makes the UI come up a bit faster.
			this.assemblyList = assemblyListManager.LoadList(spySettings, sessionSettings.ActiveAssemblyList);
			
			HandleCommandLineArguments(App.CommandLineArguments);
			
			if (assemblyList.GetAssemblies().Length == 0
			    && assemblyList.ListName == AssemblyListManager.DefaultListName)
			{
				LoadInitialAssemblies();
			}
			
			ShowAssemblyList(this.assemblyList);
			
			HandleCommandLineArgumentsAfterShowList(App.CommandLineArguments);
			if (App.CommandLineArguments.NavigateTo == null && App.CommandLineArguments.AssembliesToLoad.Count != 1) {
				SharpTreeNode node = FindNodeByPath(sessionSettings.ActiveTreeViewPath, true);
				if (node != null) {
					SelectNode(node);
					
					// only if not showing the about page, perform the update check:
					ShowMessageIfUpdatesAvailableAsync(spySettings);
				} else {
					AboutPage.Display(decompilerTextView);
				}
			}
			
			NavigationCommands.Search.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
			
			AvalonEditTextOutput output = new AvalonEditTextOutput();
			if (FormatExceptions(App.StartupExceptions.ToArray(), output))
				decompilerTextView.ShowText(output);
		}
		
		bool FormatExceptions(App.ExceptionData[] exceptions, ITextOutput output)
		{
			if (exceptions.Length == 0) return false;
			bool first = true;
			
			foreach (var item in exceptions) {
				if (first)
					first = false;
				else
					output.WriteLine("-------------------------------------------------");
				output.WriteLine("Error(s) loading plugin: " + item.PluginName);
				if (item.Exception is System.Reflection.ReflectionTypeLoadException) {
					var e = (System.Reflection.ReflectionTypeLoadException)item.Exception;
					foreach (var ex in e.LoaderExceptions) {
						output.WriteLine(ex.ToString());
						output.WriteLine();
					}
				} else
					output.WriteLine(item.Exception.ToString());
			}
			
			return true;
		}
		
		#region Update Check
		string updateAvailableDownloadUrl;
		
		void ShowMessageIfUpdatesAvailableAsync(ILSpySettings spySettings)
		{
			AboutPage.CheckForUpdatesIfEnabledAsync(spySettings).ContinueWith(
				delegate (Task<string> task) {
					if (task.Result != null) {
						updateAvailableDownloadUrl = task.Result;
						updateAvailablePanel.Visibility = Visibility.Visible;
					}
				},
				TaskScheduler.FromCurrentSynchronizationContext()
			);
		}
		
		void updateAvailablePanelCloseButtonClick(object sender, RoutedEventArgs e)
		{
			updateAvailablePanel.Visibility = Visibility.Collapsed;
		}
		
		void downloadUpdateButtonClick(object sender, RoutedEventArgs e)
		{
			Process.Start(updateAvailableDownloadUrl);
		}
		#endregion
		
		public void ShowAssemblyList(string name)
		{
			ILSpySettings settings = this.spySettings;
			if (settings == null)
			{
				settings = ILSpySettings.Load();
			}
			AssemblyList list = this.assemblyListManager.LoadList(settings, name);
			//Only load a new list when it is a different one
			if (list.ListName != CurrentAssemblyList.ListName)
			{
				ShowAssemblyList(list);
			}
		}

		void ShowAssemblyList(AssemblyList assemblyList)
		{
			history.Clear();
			this.assemblyList = assemblyList;
			
			assemblyList.assemblies.CollectionChanged += assemblyList_Assemblies_CollectionChanged;
			
			assemblyListTreeNode = new AssemblyListTreeNode(assemblyList);
			assemblyListTreeNode.FilterSettings = sessionSettings.FilterSettings.Clone();
			assemblyListTreeNode.Select = SelectNode;
			treeView.Root = assemblyListTreeNode;
			
			if (assemblyList.ListName == AssemblyListManager.DefaultListName)
				this.Title = "ILSpy";
			else
				this.Title = "ILSpy - " + assemblyList.ListName;
		}

		void assemblyList_Assemblies_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Reset) {
				history.RemoveAll(_ => true);
			}
			if (e.OldItems != null) {
				var oldAssemblies = new HashSet<LoadedAssembly>(e.OldItems.Cast<LoadedAssembly>());
				history.RemoveAll(n => n.TreeNodes.Any(
					nd => nd.AncestorsAndSelf().OfType<AssemblyTreeNode>().Any(
						a => oldAssemblies.Contains(a.LoadedAssembly))));
			}
			if (CurrentAssemblyListChanged != null)
				CurrentAssemblyListChanged(this, e);
		}
		
		void LoadInitialAssemblies()
		{
			// Called when loading an empty assembly list; so that
			// the user can see something initially.
			System.Reflection.Assembly[] initialAssemblies = {
				typeof(object).Assembly,
				typeof(Uri).Assembly,
				typeof(System.Linq.Enumerable).Assembly,
				typeof(System.Xml.XmlDocument).Assembly,
				typeof(System.Windows.Markup.MarkupExtension).Assembly,
				typeof(System.Windows.Rect).Assembly,
				typeof(System.Windows.UIElement).Assembly,
				typeof(System.Windows.FrameworkElement).Assembly,
				typeof(ICSharpCode.TreeView.SharpTreeView).Assembly,
				typeof(Mono.Cecil.AssemblyDefinition).Assembly,
				typeof(ICSharpCode.AvalonEdit.TextEditor).Assembly,
				typeof(ICSharpCode.Decompiler.Ast.AstBuilder).Assembly,
				typeof(MainWindow).Assembly
			};
			foreach (System.Reflection.Assembly asm in initialAssemblies)
				assemblyList.OpenAssembly(asm.Location);
		}

		void filterSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			RefreshTreeViewFilter();
			if (e.PropertyName == "Language") {
				DecompileSelectedNodes();
			}
		}
		
		public void RefreshTreeViewFilter()
		{
			// filterSettings is mutable; but the ILSpyTreeNode filtering assumes that filter settings are immutable.
			// Thus, the main window will use one mutable instance (for data-binding), and assign a new clone to the ILSpyTreeNodes whenever the main
			// mutable instance changes.
			if (assemblyListTreeNode != null)
				assemblyListTreeNode.FilterSettings = sessionSettings.FilterSettings.Clone();
		}
		
		internal AssemblyListTreeNode AssemblyListTreeNode {
			get { return assemblyListTreeNode; }
		}
		
		#region Node Selection
		internal void SelectNode(SharpTreeNode obj)
		{
			if (obj != null) {
				if (!obj.AncestorsAndSelf().Any(node => node.IsHidden)) {
					// Set both the selection and focus to ensure that keyboard navigation works as expected.
					treeView.FocusNode(obj);
					treeView.SelectedItem = obj;
				} else {
					MessageBox.Show("Navigation failed because the target is hidden or a compiler-generated class.\n" +
					                "Please disable all filters that might hide the item (i.e. activate " +
					                "\"View > Show internal types and members\") and try again.",
					                "ILSpy", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
			}
		}
		
		/// <summary>
		/// Retrieves a node using the .ToString() representations of its ancestors.
		/// </summary>
		SharpTreeNode FindNodeByPath(string[] path, bool returnBestMatch)
		{
			if (path == null)
				return null;
			SharpTreeNode node = treeView.Root;
			SharpTreeNode bestMatch = node;
			foreach (var element in path) {
				if (node == null)
					break;
				bestMatch = node;
				node.EnsureLazyChildren();
				node = node.Children.FirstOrDefault(c => c.ToString() == element);
			}
			if (returnBestMatch)
				return node ?? bestMatch;
			else
				return node;
		}
		
		/// <summary>
		/// Gets the .ToString() representation of the node's ancestors.
		/// </summary>
		string[] GetPathForNode(SharpTreeNode node)
		{
			if (node == null)
				return null;
			List<string> path = new List<string>();
			while (node.Parent != null) {
				path.Add(node.ToString());
				node = node.Parent;
			}
			path.Reverse();
			return path.ToArray();
		}
		
		public ILSpyTreeNode FindTreeNode(object reference)
		{
			if (reference is TypeReference) {
				return assemblyListTreeNode.FindTypeNode(((TypeReference)reference).Resolve());
			} else if (reference is MethodReference) {
				return assemblyListTreeNode.FindMethodNode(((MethodReference)reference).Resolve());
			} else if (reference is FieldReference) {
				return assemblyListTreeNode.FindFieldNode(((FieldReference)reference).Resolve());
			} else if (reference is PropertyReference) {
				return assemblyListTreeNode.FindPropertyNode(((PropertyReference)reference).Resolve());
			} else if (reference is EventReference) {
				return assemblyListTreeNode.FindEventNode(((EventReference)reference).Resolve());
			} else if (reference is AssemblyDefinition) {
				return assemblyListTreeNode.FindAssemblyNode((AssemblyDefinition)reference);
			} else {
				return null;
			}
		}
		
		public void JumpToReference(object reference)
		{
			ILSpyTreeNode treeNode = FindTreeNode(reference);
			if (treeNode != null) {
				SelectNode(treeNode);
			} else if (reference is Mono.Cecil.Cil.OpCode) {
				string link = "http://msdn.microsoft.com/library/system.reflection.emit.opcodes." + ((Mono.Cecil.Cil.OpCode)reference).Code.ToString().ToLowerInvariant() + ".aspx";
				try {
					Process.Start(link);
				} catch {
					
				}
			}
		}
		#endregion
		
		#region Open/Refresh
		void OpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = ".NET assemblies|*.dll;*.exe;*.winmd|All files|*.*";
			dlg.Multiselect = true;
			dlg.RestoreDirectory = true;
			if (dlg.ShowDialog() == true) {
				OpenFiles(dlg.FileNames);
			}
		}
		
		public void OpenFiles(string[] fileNames, bool focusNode = true)
		{
			if (fileNames == null)
				throw new ArgumentNullException("fileNames");
			
			if (focusNode)
				treeView.UnselectAll();
			
			SharpTreeNode lastNode = null;
			foreach (string file in fileNames) {
				var asm = assemblyList.OpenAssembly(file);
				if (asm != null) {
					var node = assemblyListTreeNode.FindAssemblyNode(asm);
					if (node != null && focusNode) {
						treeView.SelectedItems.Add(node);
						lastNode = node;
					}
				}
				if (lastNode != null && focusNode)
					treeView.FocusNode(lastNode);
			}
		}
		
		void RefreshCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (!DebugInformation.IsDebuggerLoaded) {
				var path = GetPathForNode(treeView.SelectedItem as SharpTreeNode);
				ShowAssemblyList(assemblyListManager.LoadList(ILSpySettings.Load(), assemblyList.ListName));
				SelectNode(FindNodeByPath(path, true));
			} else {
				e.Handled = false;
			}
		}
		
		void SearchCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			SearchPane.Instance.Show();
		}
		#endregion
		
		#region Decompile (TreeView_SelectionChanged)
		void TreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DecompileSelectedNodes();
		}

		private bool ignoreDecompilationRequests;

		private void DecompileSelectedNodes(DecompilerTextViewState state = null, bool recordHistory = true)
		{
			if (ignoreDecompilationRequests)
				return;

			if (recordHistory) {
				var dtState = decompilerTextView.GetState();
				if(dtState != null)
					history.UpdateCurrent(new NavigationState(dtState));
				history.Record(new NavigationState(treeView.SelectedItems.OfType<SharpTreeNode>()));
			}

			if (treeView.SelectedItems.Count == 1) {
				ILSpyTreeNode node = treeView.SelectedItem as ILSpyTreeNode;
				if (node != null && node.View(decompilerTextView))
					return;
			}
			decompilerTextView.Decompile(this.CurrentLanguage, this.SelectedNodes, new DecompilationOptions() { TextViewState = state });
		}
		
		void SaveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (this.SelectedNodes.Count() == 1) {
				if (this.SelectedNodes.Single().Save(this.TextView))
					return;
			}
			this.TextView.SaveToDisk(this.CurrentLanguage,
			                         this.SelectedNodes,
			                         new DecompilationOptions() { FullDecompilation = true });
		}
		
		public void RefreshDecompiledView()
		{
			DecompileSelectedNodes();
		}
		
		public DecompilerTextView TextView {
			get { return decompilerTextView; }
		}
		
		public Language CurrentLanguage {
			get {
				return sessionSettings.FilterSettings.Language;
			}
		}
		
		public IEnumerable<ILSpyTreeNode> SelectedNodes {
			get {
				return treeView.GetTopLevelSelection().OfType<ILSpyTreeNode>();
			}
		}
		#endregion
		
		#region Back/Forward navigation
		void BackCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = history.CanNavigateBack;
		}
		
		void BackCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (history.CanNavigateBack) {
				e.Handled = true;
				NavigateHistory(false);
			}
		}
		
		void ForwardCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = history.CanNavigateForward;
		}
		
		void ForwardCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (history.CanNavigateForward) {
				e.Handled = true;
				NavigateHistory(true);
			}
		}

		void NavigateHistory(bool forward)
		{
			var dtState = decompilerTextView.GetState();
			if(dtState != null)
				history.UpdateCurrent(new NavigationState(dtState));
			var newState = forward ? history.GoForward() : history.GoBack();

			ignoreDecompilationRequests = true;
			treeView.SelectedItems.Clear();
			foreach (var node in newState.TreeNodes)
			{
				treeView.SelectedItems.Add(node);
			}
			if (newState.TreeNodes.Any())
				treeView.FocusNode(newState.TreeNodes.First());
			ignoreDecompilationRequests = false;
			DecompileSelectedNodes(newState.ViewState, false);
		}

		#endregion
		
		protected override void OnStateChanged(EventArgs e)
		{
			base.OnStateChanged(e);
			// store window state in settings only if it's not minimized
			if (this.WindowState != System.Windows.WindowState.Minimized)
				sessionSettings.WindowState = this.WindowState;
		}
		
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			sessionSettings.ActiveAssemblyList = assemblyList.ListName;
			sessionSettings.ActiveTreeViewPath = GetPathForNode(treeView.SelectedItem as SharpTreeNode);
			sessionSettings.WindowBounds = this.RestoreBounds;
			sessionSettings.SplitterPosition = leftColumn.Width.Value / (leftColumn.Width.Value + rightColumn.Width.Value);
			if (topPane.Visibility == Visibility.Visible)
				sessionSettings.BottomPaneSplitterPosition = topPaneRow.Height.Value / (topPaneRow.Height.Value + textViewRow.Height.Value);
			if (bottomPane.Visibility == Visibility.Visible)
				sessionSettings.BottomPaneSplitterPosition = bottomPaneRow.Height.Value / (bottomPaneRow.Height.Value + textViewRow.Height.Value);
			sessionSettings.Save();
		}
		
		#region Top/Bottom Pane management
		public void ShowInTopPane(string title, object content)
		{
			topPaneRow.MinHeight = 100;
			if (sessionSettings.TopPaneSplitterPosition > 0 && sessionSettings.TopPaneSplitterPosition < 1) {
				textViewRow.Height = new GridLength(1 - sessionSettings.TopPaneSplitterPosition, GridUnitType.Star);
				topPaneRow.Height = new GridLength(sessionSettings.TopPaneSplitterPosition, GridUnitType.Star);
			}
			topPane.Title = title;
			topPane.Content = content;
			topPane.Visibility = Visibility.Visible;
		}
		
		void TopPane_CloseButtonClicked(object sender, EventArgs e)
		{
			sessionSettings.TopPaneSplitterPosition = topPaneRow.Height.Value / (topPaneRow.Height.Value + textViewRow.Height.Value);
			topPaneRow.MinHeight = 0;
			topPaneRow.Height = new GridLength(0);
			topPane.Visibility = Visibility.Collapsed;
			
			IPane pane = topPane.Content as IPane;
			topPane.Content = null;
			if (pane != null)
				pane.Closed();
		}
		
		public void ShowInBottomPane(string title, object content)
		{
			bottomPaneRow.MinHeight = 100;
			if (sessionSettings.BottomPaneSplitterPosition > 0 && sessionSettings.BottomPaneSplitterPosition < 1) {
				textViewRow.Height = new GridLength(1 - sessionSettings.BottomPaneSplitterPosition, GridUnitType.Star);
				bottomPaneRow.Height = new GridLength(sessionSettings.BottomPaneSplitterPosition, GridUnitType.Star);
			}
			bottomPane.Title = title;
			bottomPane.Content = content;
			bottomPane.Visibility = Visibility.Visible;
		}
		
		void BottomPane_CloseButtonClicked(object sender, EventArgs e)
		{
			sessionSettings.BottomPaneSplitterPosition = bottomPaneRow.Height.Value / (bottomPaneRow.Height.Value + textViewRow.Height.Value);
			bottomPaneRow.MinHeight = 0;
			bottomPaneRow.Height = new GridLength(0);
			bottomPane.Visibility = Visibility.Collapsed;
			
			IPane pane = bottomPane.Content as IPane;
			bottomPane.Content = null;
			if (pane != null)
				pane.Closed();
		}
		#endregion

		public void UnselectAll()
		{
			treeView.UnselectAll();
		}
		
		public void SetStatus(string status, Brush foreground)
		{
			if (this.statusBar.Visibility == Visibility.Collapsed)
				this.statusBar.Visibility = Visibility.Visible;
			this.StatusLabel.Foreground = foreground;
			this.StatusLabel.Text = status;
		}
		
		public ItemCollection GetMainMenuItems()
		{
			return mainMenu.Items;
		}
		
		public ItemCollection GetToolBarItems()
		{
			return toolBar.Items;
		}
	}
}

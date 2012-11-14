using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Ink;
using Microsoft.Surface.Presentation.Controls;
using ASG.UI.Enums;
using ActiveStoryTouch.DataModel;



namespace ASG.UI
{
    public class SessionManager : INotifyPropertyChanged
    {
        private PenMode _activePenMode = PenMode.Draw;
        private InkCanvas _targetInkCanvas = null;
        private SurfaceInkCanvas _SurfacetargetInkCanvas = null;
        
        public Project CurrentProject { get; set; }
        public ASGPage CurrentPage { get; set; }
        public GestureManager CommonGestureManager { get; set; }

        public PenMode ActivePenMode
        {
            get
            {
                return _activePenMode;
            }
            set
            {
                _activePenMode = value;
                NotifyPropertyChanged("ActivePenMode");
            }
        }



        // TODO: Add in UndoRedo CommandStack and relevant classes


       
        public SessionManager(SurfaceInkCanvas targetInkCanvas)
        {
            CurrentProject = new Project();
            AddNewPageToProject();
            _SurfacetargetInkCanvas = targetInkCanvas;
            CommonGestureManager = GestureManager.LoadFromFile("gestures.xml");
            ActivePenMode = PenMode.Draw;

            NotifyPropertyChanged("CurrentProject");
            NotifyPropertyChanged("ActivePenMode");
            NotifyPropertyChanged("ActivePenSettings");
        }

        public SessionManager(InkCanvas targetInkCanvas, Canvas root)
        {
            CurrentProject = new Project();
            CurrentProject.DefaultCanvasSettings.Height = root.Height/3;
            CurrentProject.DefaultCanvasSettings.Width = root.Width/3;
            AddNewPageToProject();
            _targetInkCanvas = targetInkCanvas;
            CommonGestureManager = GestureManager.LoadFromFile("gestures.xml");
            ActivePenMode = PenMode.Draw;

            NotifyPropertyChanged("CurrentProject");
            NotifyPropertyChanged("ActivePenMode");
            NotifyPropertyChanged("ActivePenSettings");
        }

        public void AddPrototypeElementToPage(ASGPage page, PrototypeElement element)
        {
            element.UniqueId = CurrentProject.GetNextUniqueId();

            page.PrototypeElementDictionary.Add(element.UniqueId, element);
            NotifyPropertyChanged("PrototypeElementDictionary.Values");
        }

        public void RemovePrototypeElementFromPage(PrototypeElement element)
        {            
            CurrentPage.PrototypeElementDictionary.Remove(element.UniqueId);
            NotifyPropertyChanged("PrototypeElementDictionary.Values");
        }



        public void SaveCurrentPage(StrokeCollection strokes)
        {            
            CurrentPage.Strokes.Clear();
            CurrentPage.Strokes.AddRange(StrokeHelper.ConvertToSerializableStrokeCollection(strokes));          
            NotifyPropertyChanged("ASGPage");
            
        }

        public void AddNewPageToProject()
        {
            // TODO: Generate reasonable page numbers.
            long pageNumber = CurrentProject.GetNextPageNumber();
            CurrentPage = new ASGPage { Name = String.Format("Untitled {0}", pageNumber), PageNumber = pageNumber, UniqueId = CurrentProject.GetNextUniqueId() };
            CurrentProject.PageDictionary.Add(CurrentPage.UniqueId, CurrentPage);
            NotifyPropertyChanged("CurrentPage");
        }


        // Goes to each PrototypeElement and removes the links that are directed to the page to be removed
        // in GestureTargetPageMap
        private void RemoveAllLinksFromRemovedPage(ASGPage pageToRemove)
        {
            IEnumerable<PrototypeElement> elements = from pages in this.CurrentProject.PageDictionary.Values
                                                     from items in pages.PrototypeElementDictionary.Values
                                                     where items.GestureTargetPageMap.ContainsValue(pageToRemove.UniqueId)
                                                     select items;

            foreach (PrototypeElement element in elements.ToList())
            {
                List<string> itemsToRemove = new List<string>();
                foreach (var pair in element.GestureTargetPageMap)
                {
                    if (pair.Value.Equals(pageToRemove.UniqueId))
                        itemsToRemove.Add(pair.Key);
                }
                foreach (string item in itemsToRemove)
                {
                    element.GestureTargetPageMap.Remove(item);
                }
            }
        }

        public bool RemovePage(ASGPage pagetoRemove)
        {
            if (CurrentProject.PageDictionary.Count() <= 1)
            {
                return false;
            }
            

            RemoveAllLinksFromRemovedPage(pagetoRemove);
            CurrentProject.PageDictionary.Remove(pagetoRemove.UniqueId);            
            RefreshPageNumbers(pagetoRemove);
            

          

           

            return true;
        
        
        }

     

        private void RefreshPageNumbers(ASGPage pagetoRemove)
        {
            foreach (ASGPage p in CurrentProject.PageDictionary.Values)
            {
                if (p.PageNumber > pagetoRemove.PageNumber)
                    p.PageNumber--;
            }
        }

        public void LoadPage(long uniqueId)
        {
            CurrentPage = CurrentProject.PageDictionary[uniqueId];
            NotifyPropertyChanged("CurrentPage");
        }

        public void LoadPage(ASGPage pageToLoad)
        {
            CurrentPage = pageToLoad;
            NotifyPropertyChanged("CurrentPage");
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        public void SaveProject(String fileName)
        {
            CurrentProject.SaveToFile(fileName);
            // TODO: Validate target page numbers and elements
        }

        public void LoadProject(String fileName)
        {
            CurrentProject = Project.LoadFromFile(fileName);
            if(CurrentProject.PageDictionary.Count>0)
                LoadPage(CurrentProject.PageDictionary[CurrentProject.PageDictionary.Keys.ToList()[0]]);
            ActivePenMode = PenMode.Draw;


            NotifyPropertyChanged("CurrentProject");
            NotifyPropertyChanged("ActivePenMode");
            NotifyPropertyChanged("ActivePenSettings");
        }
    }
}

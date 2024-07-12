using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MM4Common
{
    /// <summary>
    /// object for reading XmlReader streams relative
    /// to a specified place holder element;
    /// this is essentially the same as Wve.XmlStuff.XmlPlaceHolder
    /// </summary>
    public class XmlLocation
    {
        private readonly XmlReader _reader;
        private readonly string _placeElementName;
        private readonly int _placeElementDepth;
        private bool _isAtEndOfTheElement;


        /// <summary>
        /// construct XmlPlaceHolder on the XmlElement the 
        /// given reader is currently positioned on.  Throws 
        /// error if not on an XmlElement node
        /// </summary>
        /// <param name="reader"></param>
        public XmlLocation(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                _reader = reader;
                _placeElementDepth = reader.Depth;
                _placeElementName = reader.Name;
                _isAtEndOfTheElement = reader.IsEmptyElement;
            }
            else
            {
                throw new Exception("Cannot construct XmlPlaceHolder " +
                    "because reader is not positioned on an XmlElement.");
            }
        }

        /// <summary>
        /// move reader to the next child element of the original placeholder node
        /// and return true if child element found or false if got to end 
        /// of the original parent node
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public bool ReadToNextChild()
        {
            bool foundChild = false; //unless found
            //return false if reader sitting on end of the original element
            if ((_isAtEndOfTheElement) ||
                ((_reader.Name == _placeElementName) &&
                        (_reader.Depth == _placeElementDepth) &&
                        (_reader.NodeType == XmlNodeType.EndElement)))
            {
                foundChild = false;
            }
            else
            {
                while (_reader.Read())
                {
                    //read until get to end element of parent or til we find a child element
                    if ((_reader.Name == _placeElementName) &&
                        (_reader.Depth == _placeElementDepth) &&
                        (_reader.NodeType == XmlNodeType.EndElement))
                    {
                        foundChild = false;
                        _isAtEndOfTheElement = true;
                        break;
                    }
                    else if ((_reader.Depth == _placeElementDepth + 1) &&
                        (_reader.NodeType == XmlNodeType.Element))
                    {
                        foundChild = true;
                        break;
                    }
                }
            }
            return foundChild;
        }

        /// <summary>
        /// read to end of the PlaceHolder Element.  Note: unpredictible
        /// behavior if this is called when reader had read beyond
        /// end of the element by methods outside of XmlPlaceHolder.
        /// </summary>
        public void SkipToEndOfElement()
        {
            //do nothing if already at the end
            if ((_isAtEndOfTheElement) ||
                ((_reader.Name == _placeElementName) &&
                        (_reader.Depth == _placeElementDepth) &&
                        (_reader.NodeType == XmlNodeType.EndElement)))
            {
                while (_reader.Read())
                {
                    if ((_reader.Name == _placeElementName) &&
                        (_reader.Depth == _placeElementDepth) &&
                        (_reader.NodeType == XmlNodeType.EndElement))
                    {
                        _isAtEndOfTheElement = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// safely skip over the Element the reader is positioned on, moving
        /// to the end of the element;  does nothing if 
        /// current node isn't an element node;  
        /// </summary>
        /// <param name="reader"></param>
        public static void SkipElement(XmlReader reader)
        {
            string name;
            if ((reader != null) && (!reader.EOF))
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (!reader.IsEmptyElement) //do nothing if is empty
                    {
                        //read through to end of element
                        name = reader.Name;
                        int depth = reader.Depth;
                        while (reader.Read())
                        {
                            //if we get to end element
                            if ((reader.Depth == depth) &&
                                (reader.NodeType == XmlNodeType.EndElement) &&
                                    (reader.Name == name))
                            {
                                break; //from reader read
                            }
                        }//from while reader read
                    }//if not empty element
                }//from if is element
            }//from if not eof
        }
    }//class
}

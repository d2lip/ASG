#region Using directives

using System;
using System.Windows;

#endregion

namespace ShapeRecognizers.ConvexHull
{
    // Circular doubly linked lists of T

    class CDLL
    {
        private CDLL prev, next;     // not null, except in deleted elements
        public Point val;

        // A new CDLL node is a one-element circular list
        public CDLL(Point val)
        {
            this.val = val; next = prev = this;
        }

        public CDLL Prev
        {
            get { return prev; }
        }

        public CDLL Next
        {
            get { return next; }
        }

        // Delete: adjust the remaining elements, make this one point nowhere
        public void Delete()
        {
            next.prev = prev; prev.next = next;
            next = prev = null;
        }

        public CDLL Prepend(CDLL elt)
        {
            elt.next = this; elt.prev = prev; prev.next = elt; prev = elt;
            return elt;
        }

        public CDLL Append(CDLL elt)
        {
            elt.prev = this; elt.next = next; next.prev = elt; next = elt;
            return elt;
        }

        public int Size()
        {
            int count = 0;
            CDLL node = this;
            do
            {
                count++;
                node = node.next;
            } while (node != this && node != null);
            return count;
        }

        public void PrintFwd()
        {
            CDLL node = this;
            do
            {
                Console.WriteLine(node.val);
                node = node.next;
            } while (node != this);
            Console.WriteLine();
        }

        public void CopyInto(Point[] vals, int i)
        {
            CDLL node = this;
            do
            {
                if (i >= vals.Length)
                    break;

                vals[i++] = node.val;	// still, implicit checkcasts at runtime 
                node = node.next;
            } while (node != this);
        }
    }
}
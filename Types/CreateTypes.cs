using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Types
{
    public class CreateTypes
    {
        /// <summary>
        /// If you pass data by value you'll get a new copy 
        /// If you pass data by reference you'll get a pointer to that data
        /// </summary>
        public void StructAndClasses()
        {

            StoreInAStructure xs, ys;
            ys = new StoreInAStructure();
            ys.Data = 99;
            xs = ys;
            xs.Data = 100;
            Console.WriteLine($"xStruct: {xs.Data}");
            Console.WriteLine($"yStruct: {ys.Data}");

            StoreInAClass siacOne, siacTwo;
            siacOne = new StoreInAClass();
            siacOne.Data = 99;
            siacTwo = siacOne;
            siacTwo.Data = 100;
            Console.WriteLine($"xClass: {siacOne.Data}");
            Console.WriteLine($"yClass: {siacTwo.Data}");
        }

        public void Aliens()
        {
            Alien a = new Alien(10, 10);
            Console.WriteLine("a {0}", a.ToString());

            Alien x = new Alien(50, 50);
            Console.WriteLine("x {0}", x.ToString());

            Alien[] swarm = new Alien[100]; //create 100 aliens, all initialized with zero
            Console.WriteLine("swarm [0] {0}", swarm[0].ToString());
        }

        public void MyStackGeneric()
        {
            MyStack<int> myStack = new MyStack<int>();
            myStack.Push(5);
            myStack.Push(2);
            myStack.Push(6);
            myStack.Push(4);
            myStack.Push(3);
            Console.WriteLine(myStack.Pop());
        }

        public void ThisConstructor()
        {
            AlienConstructor alien = new AlienConstructor();
            Console.WriteLine($"{alien.X} {alien.Z} {alien.Lives}");
            AlienConstructor alien2 = new AlienConstructor();
            Console.WriteLine($"{alien2.X} {alien2.Z} {alien2.Lives}");
        }

        public void DestoryAlien()
        {
            DestroyAlien destroyAlien = new DestroyAlien();
            destroyAlien.AlienDestroyed += (sender, args) => Console.WriteLine($"Current lives: {args}. Alien is destroyed!");
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(1000);
                destroyAlien.RemoveLives(1);
            }
        }

        public void SummaryOfCh1to2_1()
        {
            Invoice invoice = new Invoice();
            invoice.GetDate();
            invoice.DoPrint();
            invoice.DocumentTypePrint(DocumentType.DOCX);
            invoice.OnInvoiceAdded += (sender, args) => { Console.WriteLine($"\t---[Event]:New invoice added {args.DocumentType}"); };
            Task t = Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    invoice.AddInvoice(invoice);
                    Thread.Sleep(1_000);
                }
            });
            Thread.Sleep(500);
            Document prepaidInvoice = new PrePaidInvoice();
            prepaidInvoice.GetDate();
            Thread.Sleep(500);
            prepaidInvoice.DoPrint();
            Thread.Sleep(500);
            prepaidInvoice.DocumentTypePrint(null);
            Task.WaitAll(new[] { t });
        }
    }

    #region [ Summary up to Ch 2.1 ]
    public enum DocumentType
    {
        PDF = 0,
        DOCX = 1
    }
    public abstract class Document
    {
        public DocumentType DocumentType { get; set; }
        protected List<Document> Documents { get; set; }
        public void GetDate()
        {
            Console.WriteLine($"Hello from get date it is {DateTime.Now.Hour}" +
                $":{DateTime.Now.Minute}");
        }
        public Document(DocumentType documentType)
        {
            DocumentType = documentType;
            Documents = new List<Document>();
        }

        public virtual void DoPrint()
        {
            Console.WriteLine("I should print a document");
        }
        public abstract void DocumentTypePrint(DocumentType? documentType);
        public abstract void AddInvoice(Document document);
    }
    public class Invoice : Document
    {
        private DocumentType documentType;
        public event EventHandler<Invoice> OnInvoiceAdded = (sender, args) => { };
        public Invoice(DocumentType documentType) : base(documentType)
        {
            this.documentType = documentType;
        }
        public Invoice() : this(DocumentType.DOCX)
        {
        }
        public override void DoPrint()
        {
            Console.WriteLine("I'm a cool invoice :)");
        }

        public override void DocumentTypePrint(DocumentType? documentType)
        {
            Console.WriteLine($"Printing as a {documentType ?? this.documentType }");
            if (documentType == null)
            {
                DocumentException de = DocumentException.FromDocumentException(null);
                Console.WriteLine(de?.Message);
                throw new DocumentException("You shoud've provide a value", new ArgumentNullException());
            }
        }

        public override void AddInvoice(Document document)
        {
            if (document != null)
            {
                Documents.Add(document);
                OnInvoiceAdded.Invoke(this, document as Invoice);
            }

        }
    }
    public class PrePaidInvoice : Invoice
    {
        public override void DoPrint()
        {
            base.DoPrint();
            Console.WriteLine("Prepaid invoice");
        }
        public override void DocumentTypePrint(DocumentType? documentType)
        {
            Console.WriteLine($"Printing as a {documentType ?? DocumentType.PDF}");
        }
    }
    [Serializable]
    public class DocumentException : Exception
    {
        public DocumentException() { }
        public DocumentException(string message) : base(message) { }
        public DocumentException(string message, Exception innerException = null) : base(message, innerException) { }
        public static DocumentException FromDocumentException(Guid? documentId)
        {
            return documentId == null ? new DocumentException("Document id must exist", new ArgumentNullException()) : null;
        }

    }
    #endregion

    #region [ Learn Ch 2.1 ]
    public class DestroyAlien
    {
        public int X { get; }
        public int Y { get; }
        public int Lives { get; set; }
        public DestroyAlien(int x, int y)
        {
            X = x;
            Y = y;
            Lives = 3;
        }
        public DestroyAlien() : this(10, 10)
        {

        }
        public bool RemoveLives(int lives)
        {
            Lives -= lives;
            Console.WriteLine($"Lives left {Lives}");
            if (Lives < 0)
            {
                AlienDestroyed.Invoke(this, Lives);
                return false;
            }
            return true;
        }
        public event EventHandler<int> AlienDestroyed = (sender, args) => { };

    }
    public class AlienConstructor
    {
        public int X { get; }
        public int Z { get; }
        public int Lives { get; }

        static AlienConstructor()
        {
            Console.WriteLine("Static is only once initialized");
        }
        public AlienConstructor(int x, int z, int lives)
        {
            X = x;
            Z = z;
            Lives = lives;
        }
        public AlienConstructor(int x, int y) : this(x, y, 3)
        {

        }
        public AlienConstructor(int x) : this(x, 10, 3)
        {
        }
        public AlienConstructor() : this(3, 10, 3) { }

    }
    public class MyStack<T>
    {
        private T[] array = new T[100];
        private int stackTop = 0;
        public void Push(T item)
        {
            if (stackTop > array.Length)
                throw new Exception("Stack full");
            array[stackTop] = item;
            stackTop++;
        }
        public T Pop()
        {
            if (stackTop == 0)
                throw new Exception("Stack empty");
            stackTop--;
            return array[stackTop];
        }
    }
    public struct StoreInAStructure
    {
        public int Data { get; set; }

    }
    public class StoreInAClass
    {
        public int Data { get; set; }
    }
    public struct Alien
    {
        public int X { get; }
        public int Y { get; }
        public int Lives { get; }
        public Alien(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            Lives = 3;
        }
        public override string ToString()
        {
            return $"X: {X} Y: {Y} Lives: {Lives}";
        }
    }
    #endregion
}

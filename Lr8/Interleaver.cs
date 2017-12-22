using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lr8
{
    class Interleaver
    {
        int countError;
        List<int> messageArray;
        List<int>[] codeArray;
        List<int>[] newCodeArray;
        List<Hemming> arrayHem;
       // List<int> message;
        int length;
        int column;
        int nXr;
        /// <summary>
        /// Перемежитель
        /// </summary>
        public  Interleaver(string code, int cN) {

           countError = cN;

            messageArray = new List<int>();
            GetIntArrayFromString(code,ref messageArray);
            length = code.Count();
         //   length = code.Count() / 4;
           codeArray=new List<int>[length];
           arrayHem =new List<Hemming>();
          
            //getArrayBit(9);
       
            GenerirtXrSymbols();

            Print(messageArray);
            Console.WriteLine("");

            Print(codeArray,length);
            newCodeArray = new List<int>[column];
            Interleaving(codeArray, length,newCodeArray,column);

            Console.WriteLine("");

            Print(newCodeArray,column);


            GenerirtError();
            Console.WriteLine("");

            Print(newCodeArray, column);

            Console.WriteLine("");
            Interleaving(newCodeArray, column, codeArray, length);
            Print(codeArray, length);

            CorrectErrors();

            Console.WriteLine("");
            Print(codeArray, length);

            ConvertToWord();

        }

       void CorrectErrors() {
           for (int i = 0; i < length; i++)
           {
               arrayHem[i].Yn = codeArray[i];
               arrayHem[i].decoding();
               codeArray[i] = arrayHem[i].Yn;
               nXr = arrayHem[i].Yr.Count;
           }
       }
       void Interleaving(List<int>[] matrix, int length, List<int>[] trmatrix, int column) {
           int k = 0;
           for (int i = 0; i < column; i++)
           {
               trmatrix[i] = new List<int>();
               for (int j = 0; j < length; j++)
               {
                   trmatrix[i].Add(matrix[j].ElementAt(k));
                  
               }
               k++;

           }
       }

       void Print(List<int> arr)
       {
           foreach (int a in arr)
               Console.Write(a+" ");
           Console.WriteLine(" ");
       }
       void Print(List<int>[] arr, int N) {
           for (int i = 0; i < N; i++) {
               foreach (int a in arr[i])
                   Console.Write(a + " ");
               Console.WriteLine("");
           }
       }

       void GenerirtError() {
           Random r = new Random();
           int a = r.Next(0, column-1);
           Console.WriteLine("\nномер строки, в которой генерируются ошибки "+a+"\n");
           for (int i = 0; i < countError; i++)
           {
               if (newCodeArray[a][i] == 1)
                   newCodeArray[a][i] = 0;
               else
                   newCodeArray[a][i] = 1;
           }

       }

        /// <summary>
        /// Делаем Xr при помощи кода Хемминга
        /// </summary>
        void GenerirtXrSymbols()
        {
           for (int i = 0; i < length; i++)
           {
               Hemming a = new Hemming(GetStringFromArrayBit(GetArrayBit( messageArray[i])),0);
               arrayHem.Add(a);
               codeArray[i] = new List<int>();
               foreach (int b in a.Xn)
                   codeArray[i].Add(b);
               column = a.Xn.Count;
           }
        }

       bool[] GetArrayBit(int a) {
           bool[] b = new BitArray(Decimal.GetBits(a)).Cast<bool>().ToArray();
           return b;
       }
       int GetIntFromBoolArray(bool[]arr) {
           bool[] newarr = new bool[arr.Count() - nXr];
           for (int i = 0; i < arr.Count()-nXr; i++)
           {
               newarr[i] = arr[i];
           }
           BitArray arr2 = new BitArray(newarr);
           byte[] data = new byte[arr.Count()-nXr];
           //data = Convert.ToByte();
           arr2.CopyTo(data, 0);
           int a=Convert.ToInt32(data[0]);
           return a;
       }
        void ConvertToWord(){
            int w;
            for (int i = 0; i < length; i++)
			{
               // w=getIntFromBoolArray(getBoolArrayFromListInt(codeArray[i]));
                bool[] arb = GetBoolArrayFromListInt(codeArray[i]);
                byte[] arbyte=new byte[arb.Count()-nXr];
                for (int j = 0;j < arb.Count()-nXr; j++)
                {
                    if (arb[j] == true)
                        arbyte[j] = 1;
                    else
                        arbyte[j] = 0;
                }
                

               // char[] str = Encoding.ASCII.GetChars(arbyte, 0, arbyte.Count());
               // string s = new string(str);
                int bb=getIntFromByteArray(arbyte);
                Console.Write((char)bb);
			}
            Console.WriteLine("");
        }
        int getIntFromByteArray(byte[] arr) {
            string str = "";
            for (int i = 0; i < arr.Count(); i++)
            {
                str = arr[i]+str;
            }
            return Convert.ToInt32(str,2);
        }
       bool[] GetBoolArrayFromListInt(List<int> arr) { 
            bool[] b=new bool[arr.Count];
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] == 1)
                    b[i] = true;
                else
                    b[i] = false;
            }
            return b;
       }
       string GetStringFromArrayBit(bool[] b) {
           string str = "";
          int k= b.ToList().LastIndexOf(true);
          for (int i = 0; i <= k; i++)
          { 
                str= str+Convert.ToString(Convert.ToInt32(b[i]));
          }
           return str;
       }

        /// <summary>
        /// Получаем массив интов из строки при получении кодов в ASCII
        /// </summary>
        void GetIntArrayFromString(string arr, ref List<int> X)
        {
           
            foreach (char a in arr.AsEnumerable())
            {
               byte[] k = Encoding.ASCII.GetBytes(a+"");//(int)a;
               int h = k[0]; //BitConverter.ToInt32(k,0);
               // k = (int)Char.GetNumericValue(a);
                X.Add(h);
            }
        }

    
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Lr8
{
    class Hemming
    {
        List<int> Xr;
        public List<int> Xn;

        public List<int> Yn;
        public List<int> Yr;

        protected List<int> S;

        List<int> E;

        int errorN;

        protected int rN;
        int kN;
        protected int N;
        protected int[,] H;
        int[,] G;

        public Hemming(string arr, int eN)
        {
            errorN = eN;

            Xn = new List<int>();
            Xr = new List<int>();

            Yn = new List<int>();
            Yr = new List<int>();

            S = new List<int>();

            E = new List<int>();

            GetIntArrayFromString(arr);

            kN = arr.Count();
            rN = (int)(Math.Log(kN, 2) + 1);
            N = kN + rN;
            H = new int[rN, N];
            G = new int[rN, N];


            SetGMatrix();


            SetHMatrix();
            getXnMesaage();

         //   goHamingCode();


        }



        public void goHamingCode()
        {
            PrintG();
            PrintH();

            Print(Xn);

            generirtError(errorN);

            decoding();
        }
        protected void PrintH()
        {
            Console.WriteLine("");

            for (int i = 0; i < rN; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(H[i, j] + " ");
                }
                Console.WriteLine("");
            }
        }

        public void getCorrectYn()
        {
            Print(Yn);
        }
        public void decoding()
        {

            getXrSymbol(Yn, ref Yr);
            getS();
            setEinNull();
            searchError();
            CorrectError();
        }
        protected void getS()
        {

            for (int i = 0; i < rN; i++)
            {
                S.Add(Yn.ElementAt(kN + i) ^ Yr.ElementAt(i));
            }
        }

        protected void setEinNull()
        {
            for (int i = 0; i < N; i++)
                E.Add(0);
        }
        protected void searchError()
        {
            int iNull = 0;
            int iSovp = 0;
            for (int j = 0; j < N; j++)
            {
                for (int i = 0; i < rN; i++)
                {
                    if (S.ElementAt(i) == 0)
                        iNull++;
                    if (S.ElementAt(i) == H[i, j])
                        iSovp++;
                }
                if (iNull == rN) return;
                if (iSovp == rN)
                {
                    E[j] = 1;
                //    Console.WriteLine("Ошибка в " + j + "бите");
                    return;
                }
                iSovp = 0;
                iNull = 0;
            }

        }

        protected void CorrectError()
        {
            for (int i = 0; i < N; i++)
                Yn[i] = Yn.ElementAt(i) ^ E.ElementAt(i);
        }
        void SetHMatrix()
        {
            SetIMatrix();
            SetPMatrix();
        }
        void getXnMesaage()
        {
            getXrSymbol(Xn, ref Xr);

            foreach (int a in Xr.AsEnumerable())
            {
                Xn.Add(a);
            }
        }

        protected void getXrSymbol(List<int> Xk, ref List<int> Xr)
        {

            int x;

            for (int i = 0; i < rN; i++)
            {
                x = H[i, 0] & Xk.ElementAt(0);
                for (int j = 0; j < N - rN; j++)
                {
                    if (j != 0)
                    {
                        x = x ^ (H[i, j] & Xk.ElementAt(j));
                    }
                }

                Xr.Add(x);
            }
        }

        /// <summary>
        /// Получение массива int из string
        /// </summary>
        void GetIntArrayFromString(string arr)
        {
            int k;
            foreach (char a in arr.AsEnumerable())
            {
                k = (int)Char.GetNumericValue(a);
                Xn.Add(k);
            }
        }
        void SetIMatrix()
        {
            for (int i = 0; i < rN; i++)
            {
                for (int j = N - rN; j < N; j++)
                {
                    if (i == j - kN)
                        H[i, j] = 1;
                    else
                        H[i, j] = 0;
                }
            }
        }

        bool SravnIandArr(int[] arr)
        {
            int s = 0;
            int j = kN;
            while (j < N)
            {
                for (int i = 0; i < rN; i++)
                {
                    if (arr[i] == H[i, j])
                        s++;

                }
                if (s == rN)
                    return false;
                j++;
                s = 0;
            }
            return true;
        }

        void SetPMatrix()
        {

            int[] narr = new int[rN];
            int jk = 0;

            for (int j = 0; j < N; j++)
            {

                for (int i = 0; i < rN; i++)
                {
                    narr[i] = G[i, j];
                }
                if (SravnIandArr(narr))
                {
                    for (int i = 0; i < rN; i++)
                    {
                        H[i, jk] = narr[i];
                    }
                    jk++;
                }
            }
        }

        void SetGMatrix()
        {
            for (int j = 0; j < N; j++)
            {
                for (int i = 0; i < rN; i++)
                {

                    bool[] b = new BitArray(Decimal.GetBits(j + 1)).Cast<bool>().ToArray();

                    G[i, j] = Convert.ToInt32(b[(rN - 1) - i]);
                }
            }
        }

        void PrintG()
        {
            for (int i = 0; i < rN; i++)
            {
                for (int j = 0; j < N; j++)
                {

                    Console.Write(G[i, j] + " ");
                }
                Console.WriteLine("");
            }
        }

        void Print(List<int> arr)
        {
            foreach (int a in arr)
                Console.Write(a);
            Console.WriteLine("");
        }
        void generirtError(int nE)
        {
            int i = 0;
            Random r = new Random();
            Yn = Xn;
            while (i < nE)
            {

                int a = r.Next(0, N);

                int x = 1 ^ Xn.ElementAt(a);
                Yn[a] = x;
                i++;
            }

            Console.WriteLine("Ошибочный код: ");
            Print(Yn);
        }

    }
}
